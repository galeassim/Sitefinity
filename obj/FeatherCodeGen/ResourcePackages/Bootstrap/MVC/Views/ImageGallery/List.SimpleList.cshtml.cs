#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SitefinityWebApp.ResourcePackages.Bootstrap.MVC.Views.ImageGallery
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 3 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
    using Telerik.Sitefinity;
    
    #line default
    #line hidden
    
    #line 4 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
    using Telerik.Sitefinity.Frontend.Media.Mvc.Helpers;
    
    #line default
    #line hidden
    
    #line 7 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
    using Telerik.Sitefinity.Frontend.Media.Mvc.Models.ImageGallery;
    
    #line default
    #line hidden
    
    #line 5 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
    using Telerik.Sitefinity.Frontend.Mvc.Helpers;
    
    #line default
    #line hidden
    
    #line 6 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
    using Telerik.Sitefinity.Modules.Pages;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/MVC/Views/ImageGallery/List.SimpleList.cshtml")]
    public partial class List_SimpleList : System.Web.Mvc.WebViewPage<Telerik.Sitefinity.Frontend.Mvc.Models.ContentListViewModel>
    {
        public List_SimpleList()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 9 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
Write(Html.Script(ScriptRef.JQuery, "top"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteAttribute("class", Tuple.Create(" class=\"", 358), Tuple.Create("\"", 381)
            
            #line 11 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
, Tuple.Create(Tuple.Create("", 366), Tuple.Create<System.Object, System.Int32>(Model.CssClass
            
            #line default
            #line hidden
, 366), false)
);

WriteLiteral(">\r\n\r\n");

            
            #line 13 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
    
            
            #line default
            #line hidden
            
            #line 13 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
     foreach (var item in Model.Items)
    {
        var thumbnailViewModel = (ThumbnailViewModel)item;


            
            #line default
            #line hidden
WriteLiteral("    <a");

WriteLiteral(" class=\"text-center\"");

WriteAttribute("title", Tuple.Create(" title=\"", 522), Tuple.Create("\"", 624)
            
            #line 17 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
, Tuple.Create(Tuple.Create("", 530), Tuple.Create<System.Object, System.Int32>(string.IsNullOrEmpty(item.Fields.Description) ? item.Fields.Title : item.Fields.Description
            
            #line default
            #line hidden
, 530), false)
);

WriteLiteral(">\r\n      <img");

WriteAttribute("src", Tuple.Create(" src=\"", 638), Tuple.Create("\"", 678)
            
            #line 18 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
, Tuple.Create(Tuple.Create("", 644), Tuple.Create<System.Object, System.Int32>(thumbnailViewModel.ThumbnailUrl
            
            #line default
            #line hidden
, 644), false)
);

WriteAttribute("alt", Tuple.Create(" alt=\'", 679), Tuple.Create("\'", 777)
            
            #line 18 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
, Tuple.Create(Tuple.Create("", 685), Tuple.Create<System.Object, System.Int32>(System.Text.RegularExpressions.Regex.Replace(item.Fields.AlternativeText, @"[^\w\d_-]", "")
            
            #line default
            #line hidden
, 685), false)
);

WriteLiteral(" \r\n                ");

            
            #line 19 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
           Write(Html.GetWidthAttributeIfExists(thumbnailViewModel.Width));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 20 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
           Write(Html.GetHeightAttributeIfExists(thumbnailViewModel.Height));

            
            #line default
            #line hidden
WriteLiteral(" />\r\n    </a>\r\n");

            
            #line 22 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");

            
            #line 24 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
 if (Model.ShowPager)
{
    
            
            #line default
            #line hidden
            
            #line 26 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
Write(Html.Action("Index", "ContentPager", new
       {
           currentPage = Model.CurrentPage,
           totalPagesCount = Model.TotalPagesCount.Value,
           redirectUrlTemplate = ViewBag.RedirectPageUrlTemplate
       }));

            
            #line default
            #line hidden
            
            #line 31 "..\..MVC\Views\ImageGallery\List.SimpleList.cshtml"
         
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
