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

namespace SitefinityWebApp.ResourcePackages.Bootstrap.MVC.Views.Profile
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
    
    #line 3 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
    using Telerik.Sitefinity.Frontend.Identity.Mvc.Models.Profile;
    
    #line default
    #line hidden
    
    #line 4 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
    using Telerik.Sitefinity.Frontend.Mvc.Helpers;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/ResourcePackages/Bootstrap/MVC/Views/Profile/Read.ProfilePreview.cshtml")]
    public partial class Read_ProfilePreview : System.Web.Mvc.WebViewPage<Telerik.Sitefinity.Frontend.Identity.Mvc.Models.Profile.ProfilePreviewViewModel>
    {
        public Read_ProfilePreview()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n<div");

WriteAttribute("class", Tuple.Create(" class=\"", 210), Tuple.Create("\"", 233)
            
            #line 6 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
, Tuple.Create(Tuple.Create("", 218), Tuple.Create<System.Object, System.Int32>(Model.CssClass
            
            #line default
            #line hidden
, 218), false)
);

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"media sf-profile\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"media-left sf-pr-xl\"");

WriteLiteral(">\r\n          <div");

WriteLiteral(" class=\"media-object\"");

WriteLiteral(">\r\n              <img");

WriteAttribute("src", Tuple.Create(" src=\"", 372), Tuple.Create("\"", 399)
            
            #line 10 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
, Tuple.Create(Tuple.Create("", 378), Tuple.Create<System.Object, System.Int32>(Model.AvatarImageUrl
            
            #line default
            #line hidden
, 378), false)
);

WriteAttribute("alt", Tuple.Create(" alt=\"", 400), Tuple.Create("\"", 421)
            
            #line 10 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
, Tuple.Create(Tuple.Create("", 406), Tuple.Create<System.Object, System.Int32>(Model.UserName
            
            #line default
            #line hidden
, 406), false)
);

WriteLiteral("  width=\"100\"");

WriteLiteral(" height=\"100\"");

WriteLiteral("/>\r\n          </div>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"media-body\"");

WriteLiteral(">\r\n           <h3");

WriteLiteral(" class=\"sf-mt-xxs\"");

WriteLiteral(">");

            
            #line 15 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
                            Write(Model.DisplayName);

            
            #line default
            #line hidden
WriteLiteral(" <small>(");

            
            #line 15 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
                                                       Write(Model.Nickname);

            
            #line default
            #line hidden
WriteLiteral(")</small></h3>\r\n           <p>");

            
            #line 16 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
         Write(Model.Email);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n           <p>");

            
            #line 17 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
         Write(Model.About);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n\r\n");

            
            #line 19 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
            
            
            #line default
            #line hidden
            
            #line 19 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
             if (Model.CanEdit && ViewBag.Mode == ViewMode.Both)
            {
                
            
            #line default
            #line hidden
            
            #line 21 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
           Write(Html.ActionLink(Html.Resource("EditProfileLink"), "EditProfile"));

            
            #line default
            #line hidden
            
            #line 21 "..\..\ResourcePackages\Bootstrap\MVC\Views\Profile\Read.ProfilePreview.cshtml"
                                                                                 ;
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
