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

namespace SitefinityWebApp.ResourcePackages.Bootstrap.MVC.Views.Form
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
    
    #line 4 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
    using Telerik.Sitefinity.Frontend.Forms.Mvc.Helpers;
    
    #line default
    #line hidden
    
    #line 5 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
    using Telerik.Sitefinity.Frontend.Forms.Mvc.Models;
    
    #line default
    #line hidden
    
    #line 6 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
    using Telerik.Sitefinity.Frontend.Mvc.Helpers;
    
    #line default
    #line hidden
    
    #line 7 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
    using Telerik.Sitefinity.Modules.Pages;
    
    #line default
    #line hidden
    
    #line 3 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
    using Telerik.Sitefinity.UI.MVC;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/ResourcePackages/Bootstrap/MVC/Views/Form/Index.cshtml")]
    public partial class Index : System.Web.Mvc.WebViewPage<Telerik.Sitefinity.Frontend.Forms.Mvc.Models.FormViewModel>
    {
        public Index()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n<div");

WriteLiteral(" data-sf-role=\"form-container\"");

WriteAttribute("class", Tuple.Create(" class=\"", 340), Tuple.Create("\"", 363)
            
            #line 9 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
, Tuple.Create(Tuple.Create("", 348), Tuple.Create<System.Object, System.Int32>(Model.CssClass
            
            #line default
            #line hidden
, 348), false)
);

WriteLiteral(">\r\n    <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-sf-role=\"form-id\"");

WriteAttribute("value", Tuple.Create(" value=\"", 414), Tuple.Create("\"", 435)
            
            #line 10 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
, Tuple.Create(Tuple.Create("", 422), Tuple.Create<System.Object, System.Int32>(Model.FormId
            
            #line default
            #line hidden
, 422), false)
);

WriteLiteral(" name=\"FormId\"");

WriteLiteral(" />\r\n");

            
            #line 11 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
    
            
            #line default
            #line hidden
            
            #line 11 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
      
        if (Model.UseAjaxSubmit)
        {

            
            #line default
            #line hidden
WriteLiteral("        <h3");

WriteLiteral(" data-sf-role=\"success-message\"");

WriteLiteral(" style=\"display: none;\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 15 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
       Write(Model.SuccessMessage);

            
            #line default
            #line hidden
WriteLiteral("\r\n        </h3> \r\n");

WriteLiteral("        <div");

WriteLiteral(" data-sf-role=\"error-message\"");

WriteLiteral(" style=\"display: none;\"");

WriteLiteral(" class=\"alert alert-danger\"");

WriteLiteral(">\r\n        </div> \r\n");

WriteLiteral("        <img");

WriteLiteral(" data-sf-role=\"loading-img\"");

WriteAttribute("src", Tuple.Create(" src=\'", 777), Tuple.Create("\'", 925)
            
            #line 19 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
, Tuple.Create(Tuple.Create("", 783), Tuple.Create<System.Object, System.Int32>(Url.EmbeddedResource("Telerik.Sitefinity.Resources.Reference", "Telerik.Sitefinity.Resources.Themes.Light.Images.Loadings.sfLoadingData.gif")
            
            #line default
            #line hidden
, 783), false)
);

WriteLiteral(" style=\"display:none;\"");

WriteLiteral(" />\r\n");

WriteLiteral("        <div");

WriteLiteral(" data-sf-role=\"fields-container\"");

WriteLiteral(">\r\n            ");

WriteLiteral("\r\n        </div>\r\n");

            
            #line 23 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
        }
        else
        {
            using (Html.BeginFormSitefinity("", null, (System.Web.Routing.RouteValueDictionary)null, FormMethod.Post, new Dictionary<string, object> { { "enctype", "multipart/form-data" } }, true))
            {

            
            #line default
            #line hidden
            
            #line 28 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
                   
            }
        }

        if (Model.UseAjaxSubmit)
        { 

            
            #line default
            #line hidden
WriteLiteral("        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-sf-role=\"ajax-submit-url\"");

WriteAttribute("value", Tuple.Create(" value=\"", 1453), Tuple.Create("\"", 1481)
            
            #line 34 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
, Tuple.Create(Tuple.Create("", 1461), Tuple.Create<System.Object, System.Int32>(Model.AjaxSubmitUrl
            
            #line default
            #line hidden
, 1461), false)
);

WriteLiteral(" />\r\n");

WriteLiteral("        <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-sf-role=\"redirect-url\"");

WriteAttribute("value", Tuple.Create(" value=\"", 1543), Tuple.Create("\"", 1569)
            
            #line 35 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
, Tuple.Create(Tuple.Create("", 1551), Tuple.Create<System.Object, System.Int32>(Model.RedirectUrl
            
            #line default
            #line hidden
, 1551), false)
);

WriteLiteral(" />\r\n");

            
            #line 36 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
        
            
            #line default
            #line hidden
            
            #line 36 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
   Write(Html.Script(ScriptRef.JQuery, "top", false));

            
            #line default
            #line hidden
            
            #line 36 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
                                                    
        
            
            #line default
            #line hidden
            
            #line 37 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
   Write(Html.Script(Url.WidgetContent("Mvc/Scripts/Form/form-ajax.js"), "bottom", false));

            
            #line default
            #line hidden
            
            #line 37 "..\..\ResourcePackages\Bootstrap\MVC\Views\Form\Index.cshtml"
                                                                                         
        }
    
            
            #line default
            #line hidden
WriteLiteral("\r\n</div>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
