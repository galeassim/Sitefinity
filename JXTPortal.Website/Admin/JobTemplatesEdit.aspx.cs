﻿
#region Imports...
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using JXTPortal.Web.UI;
using JXTPortal;
using JXTPortal.Common;
using System.Reflection;
using JXTPortal.Entities;
using System.Linq;
using System.IO;
#endregion

public partial class JobTemplatesEdit : System.Web.UI.Page
{
    #region "Properties"

    private JobTemplatesService _jobTemplatesService;

    private JobTemplatesService JobTemplatesService
    {
        get
        {
            if (_jobTemplatesService == null)
                _jobTemplatesService = new JobTemplatesService();
            return _jobTemplatesService;
        }
    }

    private SitesService _sitesService;

    SitesService SitesService
    {
        get
        {
            if (_sitesService == null)
            {
                _sitesService = new SitesService();
            }
            return _sitesService;
        }
    }

    private AdvertisersService _advertisersService;

    AdvertisersService AdvertisersService
    {
        get
        {
            if (_advertisersService == null)
            {
                _advertisersService = new AdvertisersService();
            }
            return _advertisersService;
        }
    }

    private int JobTemplateId
    {
        get
        {
            int _jobTemplateId = 0;

            if (Request.QueryString["JobTemplateId"] != null && Int32.TryParse(Request.QueryString["JobTemplateId"], out _jobTemplateId))
            {
                _jobTemplateId = Convert.ToInt32(Request.QueryString["JobTemplateId"]);
            }
            return _jobTemplateId;
        }
    }

    #endregion

    #region "Events handler"

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnUpdate);
        ltlMessage.Text = string.Empty;
        // Only if Admin User then Enable the Global Template
        if (SessionData.AdminUser != null && !SessionData.AdminUser.isAdminUser)
        {
            chkEducationGlobalTemplate.Enabled = false;
        }

        if (JobTemplateId > 0)
        {
            RequiredFieldValidator1.Enabled = false;
        }

        if (!Page.IsPostBack)
        {
            LoadAdvertiser();
            LoadJobTemplates();
            LoadTokens();
        }
    }

    #endregion

    #region "Methods"

    private void LoadAdvertiser()
    {
        TList<JXTPortal.Entities.Advertisers> advertisers;
        advertisers = AdvertisersService.GetBySiteId(SessionData.Site.SiteId);
        ddlAdvertiser.Items.Clear();

        ddlAdvertiser.DataSource = advertisers;
        ddlAdvertiser.DataTextField = "CompanyName";
        ddlAdvertiser.DataValueField = "AdvertiserID";
        ddlAdvertiser.DataBind();

        ddlAdvertiser.Items.Insert(0, new ListItem(" Please Choose ...", "0"));
    }

    private void LoadJobTemplates()
    {
        if (JobTemplateId > 0)
        {
            using (JXTPortal.Entities.JobTemplates template = JobTemplatesService.GetByJobTemplateId(JobTemplateId))
            {
                if (template == null || template.SiteId != SessionData.Site.SiteId)
                {
                    Response.Redirect("jobtemplates.aspx");
                }

                txtJobTemplateDescription.Text = template.JobTemplateDescription;
                txtJobTemplateHTML.Text = template.JobTemplateHtml;
                chkEducationGlobalTemplate.Checked = template.GlobalTemplate;
                if (template.JobTemplateLogo.Length > 0)
                {
                    imgAdvJobTemplateLogo.Visible = true;
                }
                imgAdvJobTemplateLogo.ImageUrl = Page.ResolveUrl("~/getfile.aspx") + "?jobtemplateid=" + Convert.ToString(JobTemplateId);

                JXTPortal.Website.CommonFunction.SetDropDownByValue(ddlAdvertiser, template.AdvertiserId.ToString());

            }
        }
    }

    private void LoadTokens()
    {
        JXTPortal.Entities.ViewJobs objJob = new JXTPortal.Entities.ViewJobs();
        lstJobFields.Items.Clear();

        List<string> attributes = Utils.AttributeExtract(objJob);
        attributes.Sort();

        foreach (string attribute in attributes)
        {
            if (attribute != "{JobTemplateHtml}")
            {
                lstJobFields.Items.Add(attribute);
            }
        }

        // Hardcoded the View Count.
        lstJobFields.Items.Add("{JobViewCount}");
    }

    private byte[] getArray(HttpPostedFile f)
    {
        int i = 0;
        byte[] b = new byte[f.ContentLength];

        f.InputStream.Read(b, 0, f.ContentLength);

        return b;
    }

    #endregion

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("jobtemplates.aspx");
    }

    protected void cvalThumbnailName_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
    {
        if ((this.docThumbnail.PostedFile != null) && !string.IsNullOrEmpty(this.docThumbnail.PostedFile.FileName))
        {
            string strExt = Path.GetExtension(docThumbnail.PostedFile.FileName).Trim();

            switch (strExt.ToUpper().Trim())
            {
                case ".GIF":
                case ".JPG":
                case ".JPEG":
                case ".PNG":
                    args.IsValid = true;
                    break;

                default:
                    if (string.IsNullOrEmpty(strExt))
                    {
                        this.cvalThumbnailName.ErrorMessage = "The uploaded image has no extension";
                        args.IsValid = false;
                    }
                    else
                    {
                        this.cvalThumbnailName.ErrorMessage = "The uploaded image extension '" + String.Format(@"<strong>{0}</strong>", strExt) + "' is not accepted";
                        args.IsValid = false;
                    }
                    break;
            }
        }
        else
        {
            args.IsValid = true;
        }
    }

    protected void cvalThumbnail_ServerValidate(System.Object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
    {
        if ((this.docThumbnail.PostedFile != null) && !string.IsNullOrEmpty(this.docThumbnail.PostedFile.FileName))
        {
            if (docThumbnail.PostedFile.ContentLength == 0)
            {
                this.cvalThumbnail.ErrorMessage = "The uploaded image has an invalid size. Please check the image and try again.";
                args.IsValid = false;

            }
            else if ((docThumbnail.PostedFile.ContentLength / (Math.Pow(1024, 2))) > 1)
            {
                this.cvalThumbnail.ErrorMessage = "The uploaded image exceeds the maximum 1Mb limit.";
                args.IsValid = false;

            }
            else
            {
                args.IsValid = true;
            }
        }
        else if (JobTemplateId > 0)
        {
            args.IsValid = true;
        }
        else
        {
            args.IsValid = false;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                JXTPortal.Entities.JobTemplates template = new JXTPortal.Entities.JobTemplates();

                if (JobTemplateId > 0)
                {
                    template = JobTemplatesService.GetByJobTemplateId(JobTemplateId);

                    if (template.SiteId != SessionData.Site.SiteId)
                        Response.Redirect("/admin/jobtemplates.aspx");

                }

                //Upload Thumbnail
                if ((docThumbnail.PostedFile != null) && docThumbnail.PostedFile.ContentLength > 0)
                {
                    System.Drawing.Image objOriginalImage = System.Drawing.Image.FromStream(this.docThumbnail.PostedFile.InputStream);
                    System.Drawing.Image objResizedImage = JXTPortal.Common.Utils.ResizeImage(objOriginalImage, 240, 150);

                    System.IO.MemoryStream objOutputMemorySTream = new System.IO.MemoryStream();
                    objResizedImage.Save(objOutputMemorySTream, objOriginalImage.RawFormat);

                    byte[] abytFile = new byte[Convert.ToInt32(objOutputMemorySTream.Length)];
                    objOutputMemorySTream.Position = 0;
                    objOutputMemorySTream.Read(abytFile, 0, abytFile.Length);

                    template.JobTemplateLogo = abytFile;
                }

                template.SiteId = SessionData.Site.SiteId;
                template.JobTemplateDescription = txtJobTemplateDescription.Text;
                template.JobTemplateHtml = txtJobTemplateHTML.Text;
                template.AdvertiserId = Convert.ToInt32(ddlAdvertiser.SelectedValue);
                template.GlobalTemplate = chkEducationGlobalTemplate.Checked;

                if (JobTemplateId > 0)
                {
                    template.IsNew = false;
                    //don't need this as a get above has already got this info
                    //template.JobTemplateId = JobTemplateId;
                    JobTemplatesService.Update(template);
                }
                else
                {
                    JobTemplatesService.Insert(template);
                }
            }
            catch (Exception ex)
            {
                ltlMessage.Text = ex.Message;

                if (ex.Message == "Parameter is not valid.")
                {
                    ltlMessage.Text = "Thumbnail image is not valid.";
                }

                return;
            }

            if (((Button)sender).Text == "Save")
                Response.Redirect("jobtemplates.aspx");

        }

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (JobTemplateId > 0)
        {
            //JobTemplatesService.Delete(JobTemplateId);
            //Response.Redirect("jobtemplates.aspx");
        }
    }

}

