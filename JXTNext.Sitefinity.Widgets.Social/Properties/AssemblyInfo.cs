using JXTNext.Sitefinity.Widgets.Social;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("JXTNext.Sitefinity.Widgets.Social")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("JXTNext.Sitefinity.Widgets.Social")]
[assembly: AssemblyCopyright("Copyright �  2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("21a841b2-6b3a-46cd-b0c0-7f40ca812686")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]


//SocialLink assets
[assembly: WebResource("JXTNext.Sitefinity.Widgets.Social.Mvc.Scripts.SocialLink.designerview-simple.js", "application/x-javascript")]

[assembly: Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes.ControllerContainer]

[assembly: Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers.Attributes.ResourcePackage]

[assembly: PreApplicationStartMethod(typeof(Startup), "PreApplicationStart")]