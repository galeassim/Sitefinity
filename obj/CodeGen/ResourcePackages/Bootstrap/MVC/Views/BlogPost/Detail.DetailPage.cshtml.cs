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

namespace SitefinityWebApp.ResourcePackages.Bootstrap.MVC.Views.BlogPost
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
    
    #line 3 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
    using Telerik.Sitefinity;
    
    #line default
    #line hidden
    
    #line 4 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
    using Telerik.Sitefinity.Frontend.Mvc.Helpers;
    
    #line default
    #line hidden
    
    #line 5 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
    using Telerik.Sitefinity.Web.DataResolving;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/ResourcePackages/Bootstrap/MVC/Views/BlogPost/Detail.DetailPage.cshtml")]
    public partial class Detail_DetailPage : System.Web.Mvc.WebViewPage<Telerik.Sitefinity.Frontend.Mvc.Models.ContentDetailsViewModel>
    {
        public Detail_DetailPage()
        {
        }
        public override void Execute()
        {
WriteLiteral("\n<div");

WriteAttribute("class", Tuple.Create(" class=\"", 196), Tuple.Create("\"", 219)
            
            #line 7 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
, Tuple.Create(Tuple.Create("", 204), Tuple.Create<System.Object, System.Int32>(Model.CssClass
            
            #line default
            #line hidden
, 204), false)
);

WriteLiteral(" ");

            
            #line 7 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
                        Write(Html.InlineEditingAttributes(Model.ProviderName, Model.ContentType.FullName, (Guid)Model.Item.Fields.Id));

            
            #line default
            #line hidden
WriteLiteral(">\n    <h3>\n        <span ");

            
            #line 9 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
         Write(Html.InlineEditingFieldAttributes("Title", "ShortText"));

            
            #line default
            #line hidden
WriteLiteral(">");

            
            #line 9 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
                                                                  Write(Model.Item.Fields.Title);

            
            #line default
            #line hidden
WriteLiteral("</span>\n    </h3>\n\n    <div>\n");

WriteLiteral("        ");

            
            #line 13 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
   Write(Model.Item.GetDateTime("PublicationDate", "MMM d, yyyy, HH:mm tt"));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("        ");

            
            #line 14 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
   Write(Html.Resource("By"));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("        ");

            
            #line 15 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
   Write(DataResolver.Resolve(@Model.Item.DataItem, "Author", null));

            
            #line default
            #line hidden
WriteLiteral("\n");

WriteLiteral("        ");

            
            #line 16 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
   Write(Html.CommentsCount(string.Empty, @Model.Item.DataItem));

            
            #line default
            #line hidden
WriteLiteral("\n    </div>\n    \n    <div ");

            
            #line 19 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
    Write(Html.InlineEditingFieldAttributes("Summary", "LongText"));

            
            #line default
            #line hidden
WriteLiteral(">");

            
            #line 19 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
                                                              Write(Html.HtmlSanitize((string)Model.Item.Fields.Summary));

            
            #line default
            #line hidden
WriteLiteral("</div>\n\n    <div ");

            
            #line 21 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
    Write(Html.InlineEditingFieldAttributes("Content", "LongText"));

            
            #line default
            #line hidden
WriteLiteral(">");

            
            #line 21 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
                                                              Write(Html.HtmlSanitize((string)Model.Item.Fields.Content));

            
            #line default
            #line hidden
WriteLiteral("</div>\n    \n");

            
            #line 23 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
    
            
            #line default
            #line hidden
            
            #line 23 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
              
        if (Model.EnableSocialSharing)
        {
            var item = Model.Item.DataItem as Telerik.Sitefinity.Model.IHasTitle;
            
            
            #line default
            #line hidden
            
            #line 27 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
       Write(Html.SocialShareOptions(item));

            
            #line default
            #line hidden
            
            #line 27 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
                                          ;
        }
    
            
            #line default
            #line hidden
WriteLiteral("\n\n");

WriteLiteral("    ");

            
            #line 31 "..\..\ResourcePackages\Bootstrap\MVC\Views\BlogPost\Detail.DetailPage.cshtml"
Write(Html.CommentsList(@Model.Item.DataItem));

            
            #line default
            #line hidden
WriteLiteral("\n</div>");

        }
    }
}
#pragma warning restore 1591
