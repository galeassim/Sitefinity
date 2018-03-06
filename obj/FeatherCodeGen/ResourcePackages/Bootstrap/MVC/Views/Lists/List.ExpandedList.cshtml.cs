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

namespace SitefinityWebApp.ResourcePackages.Bootstrap.MVC.Views.Lists
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
    
    #line 3 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
    using Telerik.Sitefinity.Frontend.Lists.Mvc.Models;
    
    #line default
    #line hidden
    
    #line 4 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
    using Telerik.Sitefinity.Frontend.Mvc.Helpers;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/MVC/Views/Lists/List.ExpandedList.cshtml")]
    public partial class List_ExpandedList : System.Web.Mvc.WebViewPage<Telerik.Sitefinity.Frontend.Mvc.Models.ContentListViewModel>
    {
        public List_ExpandedList()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n<div");

WriteAttribute("class", Tuple.Create(" class=\"", 179), Tuple.Create("\"", 202)
            
            #line 6 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
, Tuple.Create(Tuple.Create("", 187), Tuple.Create<System.Object, System.Int32>(Model.CssClass
            
            #line default
            #line hidden
, 187), false)
);

WriteLiteral(">\r\n\r\n");

            
            #line 8 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
    
            
            #line default
            #line hidden
            
            #line 8 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
     foreach (var item in Model.Items)
    {

            
            #line default
            #line hidden
WriteLiteral("        <h1 ");

            
            #line 10 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
       Write(Html.InlineEditingAttributes(Model.ProviderName, Model.ContentType.FullName, (Guid)item.Fields.Id));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 11 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
       Write(Html.InlineEditingFieldAttributes("Title", "ShortText"));

            
            #line default
            #line hidden
WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 12 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
       Write(item.Fields.Title);

            
            #line default
            #line hidden
WriteLiteral("\r\n        </h1>\r\n");

            
            #line 14 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"

        foreach (var listItem in ((ListViewModel)item).ListItemViewModel.Items)
        {

            
            #line default
            #line hidden
WriteLiteral("            <div ");

            
            #line 17 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
            Write(Html.InlineEditingAttributes(Model.ProviderName, ((ListViewModel)item).ListItemViewModel.ContentType.FullName, (Guid)listItem.Fields.Id));

            
            #line default
            #line hidden
WriteLiteral(">\r\n                <h3 ");

            
            #line 18 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
               Write(Html.InlineEditingFieldAttributes("Title", "ShortText"));

            
            #line default
            #line hidden
WriteLiteral(">");

            
            #line 18 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
                                                                        Write(listItem.Fields.Title);

            
            #line default
            #line hidden
WriteLiteral("</h3>\r\n                <p ");

            
            #line 19 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
              Write(Html.InlineEditingFieldAttributes("Content", "LongText"));

            
            #line default
            #line hidden
WriteLiteral(">");

            
            #line 19 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
                                                                        Write(Html.HtmlSanitize((string)listItem.Fields.Content));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n            </div>\r\n");

            
            #line 21 "..\..MVC\Views\Lists\List.ExpandedList.cshtml"
        }
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
