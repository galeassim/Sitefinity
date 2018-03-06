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

namespace SitefinityWebApp.ResourcePackages.Bootstrap.MVC.Views.CheckboxesField
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
    
    #line 4 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
    using Telerik.Sitefinity.Frontend.Forms.Mvc.Helpers;
    
    #line default
    #line hidden
    
    #line 5 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
    using Telerik.Sitefinity.Frontend.Mvc.Helpers;
    
    #line default
    #line hidden
    
    #line 6 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
    using Telerik.Sitefinity.Modules.Pages;
    
    #line default
    #line hidden
    
    #line 3 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
    using Telerik.Sitefinity.UI.MVC;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/MVC/Views/CheckboxesField/Write.Default.cshtml")]
    public partial class Write_Default : System.Web.Mvc.WebViewPage<Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.CheckboxesField.CheckboxesFieldViewModel>
    {
        public Write_Default()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 8 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
Write(Html.Script(ScriptRef.JQuery, "top", false));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 10 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
  
    var fieldName = Model.MetaField.FieldName;
    var requiredAttributes = MvcHtmlString.Create(Model.ValidationAttributes);

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteAttribute("class", Tuple.Create(" class=\"", 475), Tuple.Create("\"", 509)
            
            #line 15 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
, Tuple.Create(Tuple.Create("", 483), Tuple.Create<System.Object, System.Int32>(Model.CssClass
            
            #line default
            #line hidden
, 483), false)
, Tuple.Create(Tuple.Create(" ", 498), Tuple.Create("form-group", 499), true)
);

WriteLiteral(" data-sf-role=\"checkboxes-field-container\"");

WriteLiteral(">\r\n    <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-sf-role=\"violation-messages\"");

WriteAttribute("value", Tuple.Create(" value=\'", 613), Tuple.Create("\'", 669)
, Tuple.Create(Tuple.Create("", 621), Tuple.Create("{", 621), true)
, Tuple.Create(Tuple.Create(" ", 622), Tuple.Create("\"required\":", 623), true)
, Tuple.Create(Tuple.Create(" ", 634), Tuple.Create("\"", 635), true)
            
            #line 16 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
  , Tuple.Create(Tuple.Create("", 636), Tuple.Create<System.Object, System.Int32>(Model.RequiredViolationMessage
            
            #line default
            #line hidden
, 636), false)
, Tuple.Create(Tuple.Create("", 667), Tuple.Create("\"}", 667), true)
);

WriteLiteral(" />\r\n\t<input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-sf-role=\"required-validator\"");

WriteAttribute("value", Tuple.Create(" value=\'", 730), Tuple.Create("\'", 766)
            
            #line 17 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
, Tuple.Create(Tuple.Create("", 738), Tuple.Create<System.Object, System.Int32>(Model.IsRequired.ToString()
            
            #line default
            #line hidden
, 738), false)
);

WriteLiteral(" />\r\n   \r\n    <strong>");

            
            #line 19 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
       Write(Model.MetaField.Title);

            
            #line default
            #line hidden
WriteLiteral("</strong>\r\n");

            
            #line 20 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
    
            
            #line default
            #line hidden
            
            #line 20 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
     if (!string.IsNullOrEmpty(Model.MetaField.Description)) 
    {

            
            #line default
            #line hidden
WriteLiteral("        <p");

WriteLiteral(" class=\"text-muted\"");

WriteLiteral(">");

            
            #line 22 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
                         Write(Model.MetaField.Description);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 23 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("    ");

            
            #line 24 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
     foreach (var choice in Model.Choices)
    {
        string value = !string.IsNullOrEmpty(Model.Value as string) ? Model.Value as string : string.Empty;
        string selectedValue = !string.IsNullOrEmpty(value) ? value : Model.MetaField.DefaultValue;
        var selctedAttributes = !string.IsNullOrEmpty(selectedValue) && selectedValue.Contains(choice as string) ? "checked" : string.Empty;

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n            <label>\r\n                <input");

WriteLiteral(" type=\"checkbox\"");

WriteAttribute("name", Tuple.Create(" name=\"", 1457), Tuple.Create("\"", 1474)
            
            #line 31 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
, Tuple.Create(Tuple.Create("", 1464), Tuple.Create<System.Object, System.Int32>(fieldName
            
            #line default
            #line hidden
, 1464), false)
);

WriteAttribute("value", Tuple.Create(" value=\"", 1475), Tuple.Create("\"", 1490)
            
            #line 31 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
, Tuple.Create(Tuple.Create("", 1483), Tuple.Create<System.Object, System.Int32>(choice
            
            #line default
            #line hidden
, 1483), false)
);

WriteLiteral(" data-sf-role=\"checkboxes-field-input\"");

WriteLiteral(" ");

            
            #line 31 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
                                                                                                          Write(selctedAttributes);

            
            #line default
            #line hidden
WriteLiteral(" />\r\n");

WriteLiteral("                ");

            
            #line 32 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
           Write(choice);

            
            #line default
            #line hidden
WriteLiteral("\r\n            </label>\r\n        </div>\r\n");

            
            #line 35 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 37 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
    
            
            #line default
            #line hidden
            
            #line 37 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
     if(Model.HasOtherChoice)
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n            <label>\r\n                <input");

WriteLiteral(" type=\"checkbox\"");

WriteAttribute("name", Tuple.Create(" name=\"", 1754), Tuple.Create("\"", 1771)
            
            #line 41 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
, Tuple.Create(Tuple.Create("", 1761), Tuple.Create<System.Object, System.Int32>(fieldName
            
            #line default
            #line hidden
, 1761), false)
);

WriteLiteral(" data-sf-checkboxes-role=\"other-choice-checkbox\"");

WriteLiteral(" data-sf-role=\"checkboxes-field-input\"");

WriteLiteral("/>\r\n");

WriteLiteral("                ");

            
            #line 42 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
           Write(Html.Resource("Other"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" data-sf-checkboxes-role=\"other-choice-text\"");

WriteLiteral(" />\r\n            </label>\r\n        </div>\r\n");

            
            #line 46 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("    \r\n</div>\r\n\r\n");

            
            #line 50 "..\..MVC\Views\CheckboxesField\Write.Default.cshtml"
Write(Html.Script(Url.WidgetContent("Mvc/Scripts/CheckboxesField/checkboxes-field.js"), "bottom", false));

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
