﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using JXTNext.Sitefinity.Widgets.Job.Mvc.Models;
using JXTNext.Sitefinity.Connector.BusinessLogics;
using JXTNext.Sitefinity.Connector.Options;
using System.Dynamic;
using JXTNext.Sitefinity.Connector.Options.Models.Job;
using Newtonsoft.Json;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes;
using System.ComponentModel;
using Telerik.Sitefinity.Taxonomies.Model;
using JXTNext.Sitefinity.Common.Helpers;

namespace JXTNext.Sitefinity.Widgets.Job.Mvc.Controllers
{
    [EnhanceViewEngines]
    [ControllerToolboxItem(Name = "JobFilters_MVC", Title = "Filters Listing", SectionName = "JXTNext.Job", CssClass = JobFiltersController.WidgetIconCssClass)]
    public class JobFiltersController : Controller
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public JobSearchModel Model
        {
            get
            {
                if (this.model == null)
                    this.model = new JobSearchModel();

                return this.model;
            }
        }


        public string PrefixIdText { get; set; }
        /// <summary>
        /// Gets or sets the name of the template that widget will be displayed.
        /// </summary>
        /// <value></value>
        private string _templateName;
        public string TemplateName
        {
            get {
                if (string.IsNullOrEmpty(_templateName))
                    _templateName = "T_Simple";
                return _templateName;
            }
            set { _templateName = value; }
        }

        IBusinessLogicsConnector _BLConnector;
        IOptionsConnector _OConnector;

        public JobFiltersController(IEnumerable<IBusinessLogicsConnector> _bConnectors, IEnumerable<IOptionsConnector> _oConnectors)
        {
            _BLConnector = _bConnectors.Where(c => c.ConnectorType == JXTNext.Sitefinity.Connector.IntegrationConnectorType.JXTNext).FirstOrDefault();
            _OConnector = _oConnectors.Where(c => c.ConnectorType == JXTNext.Sitefinity.Connector.IntegrationConnectorType.JXTNext).FirstOrDefault();
        }

        // GET: JobFilters
        public ActionResult Index([ModelBinder(typeof(JobSearchResultsFilterBinder))] JobSearchResultsFilterModel filterModel)
        {
            dynamic dynamicFilterResponse = null;
            JXTNext_GetJobFiltersRequest request = new JXTNext_GetJobFiltersRequest();
            IGetJobFiltersResponse filtersResponse = _OConnector.JobFilters<JXTNext_GetJobFiltersRequest, JXTNext_GetJobFiltersResponse>(request);

            List<JobFilterRoot> filtersVMList = null;
            if (filtersResponse != null && filtersResponse.Filters != null
                && filtersResponse.Filters.Data != null)
            {
                filtersVMList = filtersResponse.Filters.Data;
                dynamicFilterResponse = filtersResponse.Filters.Data as dynamic;
            }

            var filtersSelected = filterModel.Filters;

            if(filtersSelected != null && filtersSelected.Count > 0)
                ProcessFilters(filtersSelected, filtersVMList);
     
            ViewBag.FilterModel = JsonConvert.SerializeObject(filterModel);
            ViewBag.Keywords = filterModel.Keywords;

            var jobFilterComponents = this.SerializedJobSearchParams == null ? null : JsonConvert.DeserializeObject<List<JobSearchModel>>(this.SerializedJobSearchParams);

            if (jobFilterComponents != null || this.DisplayCompanyName)
            {
                if (jobFilterComponents != null)
                {
                    foreach (JobSearchModel item in jobFilterComponents)
                    {
                        FilterData(item.Filters);
                        item.Filters = item.Filters.Where(d => d.Show == true || d.Filters?.Count > 0).ToList();
                    }
                }

                var selectedConfigFilters = GetSelecctedFiltersFromConfig(filtersVMList, jobFilterComponents, this.DisplayCompanyName);
                AppendParentIds(selectedConfigFilters);
                dynamicFilterResponse = selectedConfigFilters as dynamic;
            }
            else
            {
                AppendParentIds(filtersVMList);
                dynamicFilterResponse = filtersVMList as dynamic;
            }

            ViewBag.PrefixIdsText = this.PrefixIdText == null ? "" : this.PrefixIdText;

            return View(this.TemplateName, dynamicFilterResponse);
        }

        private void ProcessConfigSubFilters(JobSearchItem configFilterItem, JobFilter newFilter, List<JobFilter> backendJobFilters)
        {
            if (configFilterItem != null && newFilter != null && backendJobFilters != null)
            {
                foreach (var backendFilterItem in backendJobFilters)
                {
                    if(configFilterItem.ID.Equals(backendFilterItem.ID, StringComparison.OrdinalIgnoreCase))
                    {
                        newFilter.ID = backendFilterItem.ID;
                        newFilter.Label = backendFilterItem.Label;
                        newFilter.Count = backendFilterItem.Count;
                        newFilter.Selected = backendFilterItem.Selected;

                        foreach (var configSubFilterItem in configFilterItem.Filters)
                        {
                            if (backendFilterItem.Filters != null && backendFilterItem.Filters.Count > 0)
                            {
                                var newSubFilter = new JobFilter() { Filters = new List<JobFilter>() };
                                ProcessConfigSubFilters(configSubFilterItem, newSubFilter, backendFilterItem.Filters);
                                newFilter.Filters.Add(newSubFilter);
                            }
                        }
                        break;
                    }
                }
            }
        }

        private List<JobFilterRoot> GetSelecctedFiltersFromConfig(List<JobFilterRoot> filtersVMList, List<JobSearchModel> designerViewModel, bool displayCompanyName)
        {
            List<JobFilterRoot> selectedConfigFilters = new List<JobFilterRoot>();
            if (designerViewModel != null && filtersVMList != null)
            {
                foreach (var item in designerViewModel)
                {
                    foreach (var filterRoot in filtersVMList)
                    {
                        if (item.FilterType.Equals(filterRoot.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            if (item.Filters == null || item.Filters.Count <= 0)
                            {
                                selectedConfigFilters.Add(filterRoot);
                                break;
                            }
                            else
                            {
                                JobFilterRoot rootItem = new JobFilterRoot() { Filters = new List<JobFilter>() };
                                rootItem.ID = filterRoot.ID;
                                rootItem.Name = filterRoot.Name;
                                rootItem.Type = filterRoot.Type;
                                foreach (var configFilterItem in item.Filters)
                                {
                                    var newSubFilter = new JobFilter() { Filters = new List<JobFilter>(), Label = null };
                                    ProcessConfigSubFilters(configFilterItem, newSubFilter, filterRoot.Filters);
                                    if (newSubFilter.Label != null)
                                        rootItem.Filters.Add(newSubFilter);
                                }

                                selectedConfigFilters.Add(rootItem);
                            }
                        }
                    }
                }
            }

            if(displayCompanyName)
            {
                var companyFilter = filtersVMList.Where(item => item.Name == "CompanyName").FirstOrDefault();
                if (companyFilter != null)
                    selectedConfigFilters.Add(companyFilter);
            }

            return selectedConfigFilters;

        }

        static void FilterData(List<JobSearchItem> data)
        {
            if (data == null || data.Count == 0)
                return;

            foreach (JobSearchItem item in data)
            {
                if (item.Filters != null && item.Filters.Count > 0)
                {
                    FilterData(item.Filters);
                    item.Filters = item.Filters.Where(d => d.Show == true || d.Filters?.Count > 0).ToList();
                }
            }

            data = data.Where(d => d.Show == true || d.Filters?.Count > 0).ToList();
        }



        static void ProcessFiltersIds(List<JobFilter> filters, string parentId)
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (var filter in filters)
                {
                    filter.ID = parentId + "_" + filter.ID;
                    if (filter.Filters != null && filter.Filters.Count > 0)
                    {
                        ProcessFiltersIds(filter.Filters, filter.ID);
                    }
                }
            }
        }

        static void AppendParentIds(List<JobFilterRoot> filtersVMList)
        {
            if (filtersVMList != null && filtersVMList.Count > 0)
            {
                foreach (var filterRoot in filtersVMList)
                {
                    if (filterRoot.Filters != null && filterRoot.Filters.Count > 0)
                    {
                        foreach (var filter in filterRoot.Filters)
                        {
                            ProcessFiltersIds(filter.Filters, filter.ID);
                        }
                    }
                }
            }
        }

        static void ProcessFilters(List<JobSearchFilterReceiver> selectedFilters, List<JobFilterRoot> filtersVMList)
        {
            if (selectedFilters != null && selectedFilters.Count > 0)
            {
                foreach (var rootItem in selectedFilters)
                {
                    if (rootItem != null)
                    {
                        if (filtersVMList != null && filtersVMList.Count > 0)
                        {
                            foreach (var filterVMRootItem in filtersVMList)
                            {
                                if (filterVMRootItem.Name == rootItem.rootId)
                                {
                                    if (filterVMRootItem.Filters != null && filterVMRootItem.Filters.Count > 0)
                                    {
                                        foreach (var filterItem in filterVMRootItem.Filters)
                                        {
                                            MergeFilters(filterItem, rootItem.values);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static bool CheckSubTragets(JobSearchFilterReceiverItem filterRecItem, string Id)
        {
            if(filterRecItem != null)
            {
                if(filterRecItem.ItemID.Equals(Id, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    if(filterRecItem.SubTargets != null && filterRecItem.SubTargets.Count > 0)
                    {
                        foreach(var item in filterRecItem.SubTargets)
                        {
                            if (CheckSubTragets(item, Id))
                                return true;
                        }
                    }
                }
            }

            return false;
        }

        public static void ProcessCategories(HierarchicalTaxon category, JobFilter jobFilter)
        {
            if (category != null && jobFilter != null)
            {
                jobFilter.ID = category.Id.ToString().ToUpper();
                jobFilter.Label = category.Title;
                if (category.Subtaxa != null && category.Subtaxa.Count > 0)
                {
                    foreach (var subTaxon in category.Subtaxa)
                    {
                        var subFilter = new JobFilter() { Filters = new List<JobFilter>() };
                        ProcessCategories(subTaxon, subFilter);
                        jobFilter.Filters.Add(subFilter);
                    }
                }
            }
        }

        public static JobFiltersData GetFiltersData()
        {
            JobFiltersData filtersData = new JobFiltersData() { Data = new List<JobFilterRoot>() };
            var topLovelCategories = SitefinityHelper.GetTopLevelCategories();

            foreach (var taxon in topLovelCategories)
            {
                JobFilterRoot filterRoot = new JobFilterRoot() { Filters = new List<JobFilter>() };
                filterRoot.ID = taxon.Id.ToString().ToUpper();
                filterRoot.Name = taxon.Title;

                var hierarchicalTaxon = taxon as HierarchicalTaxon;
                if (hierarchicalTaxon != null)
                {
                    foreach (var childTaxon in hierarchicalTaxon.Subtaxa)
                    {
                        var jobFilter = new JobFilter() { Filters = new List<JobFilter>() };
                        ProcessCategories(childTaxon, jobFilter);
                        filterRoot.Filters.Add(jobFilter);
                    }
                }

                filtersData.Data.Add(filterRoot);
            }

            return filtersData;
        }

        static void MergeFilters(JobFilter filterItem, List<JobSearchFilterReceiverItem> values)
        {
            if(filterItem != null)
            {
                if (values != null && values.Count > 0)
                {
                    foreach(var item in values)
                    {
                        if(item.ItemID.Equals(filterItem.ID, StringComparison.OrdinalIgnoreCase))
                        {
                            filterItem.Selected = true;
                            break;
                        }

                        if(item.SubTargets != null && item.SubTargets.Count > 0)
                        {
                            foreach(var subItem in item.SubTargets)
                            {
                                if(CheckSubTragets(item, filterItem.ID))
                                {
                                    filterItem.Selected = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (filterItem.Filters != null && filterItem.Filters.Count > 0)
                    {
                        foreach (var item in filterItem.Filters)
                        {
                            MergeFilters(item, values);
                        }
                    }
                }
            }
        }

        public string SerializedJobSearchParams { get; set; }
        public bool DisplayCompanyName { get; set; }

        private string _serializedFilterData;
        public string SerializedFilterData
        {
            get
            {
                if (string.IsNullOrEmpty(_serializedFilterData))
                {
                    //JXTNext_GetJobFiltersRequest filterOptionRequest = new JXTNext_GetJobFiltersRequest { SiteId = 1 };
                    //IGetJobFiltersResponse filtersResponse = _OConnector.JobFilters<JXTNext_GetJobFiltersRequest, JXTNext_GetJobFiltersResponse>(filterOptionRequest);
                    var filtersData = GetFiltersData();
                    _serializedFilterData = JsonConvert.SerializeObject(filtersData.Data);
                }
                return _serializedFilterData;
            }
        }

        internal const string WidgetIconCssClass = "sfMvcIcn";
        private JobSearchModel model;
    }
}