#pragma checksum "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b7e3b9927bcfa5d93c14690e243135f3ca5759b2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_RealEstateView), @"mvc.1.0.view", @"/Views/Shared/RealEstateView.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\_ViewImports.cshtml"
using CollateralLoanMVC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\_ViewImports.cshtml"
using CollateralLoanMVC.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b7e3b9927bcfa5d93c14690e243135f3ca5759b2", @"/Views/Shared/RealEstateView.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7d0cad2cdcc657571374ef8f0269f851741b00f5", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_RealEstateView : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CollateralLoanMVC.ViewModels.ViewCollateralViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div");
            BeginWriteAttribute("id", " id=\"", 67, "\"", 91, 1);
#nullable restore
#line 3 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 72, nameof(Collateral), 72, 19, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"container-fluid px-0\">\r\n\t<h4>Collateral</h4>\r\n\t<div class=\"px-3 bg-light border\">\r\n\t\t<div class=\"row py-3\">\r\n\t\t\t<div class=\"col-4\">\r\n\t\t\t\t<label");
            BeginWriteAttribute("for", " for=\"", 243, "\"", 311, 1);
#nullable restore
#line 8 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 249, string.Concat(nameof(Collateral), "_", nameof(Collateral.Id)), 249, 62, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 312, "\"", 320, 0);
            EndWriteAttribute();
            WriteLiteral(">Collateral Id</label>\r\n\t\t\t\t<input");
            BeginWriteAttribute("id", " id=\"", 355, "\"", 422, 1);
#nullable restore
#line 9 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 360, string.Concat(nameof(Collateral), "_", nameof(Collateral.Id)), 360, 62, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"");
            BeginWriteAttribute("value", " value=\"", 444, "\"", 472, 1);
#nullable restore
#line 9 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 452, Model.RealEstate.Id, 452, 20, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" disabled />\r\n\t\t\t</div>\r\n\t\t\t<div class=\"col-4\">\r\n\t\t\t\t<label");
            BeginWriteAttribute("for", " for=\"", 532, "\"", 602, 1);
#nullable restore
#line 12 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 538, string.Concat(nameof(Collateral), "_", nameof(Collateral.Type)), 538, 64, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 603, "\"", 611, 0);
            EndWriteAttribute();
            WriteLiteral(">Type</label>\r\n\t\t\t\t<input");
            BeginWriteAttribute("id", " id=\"", 637, "\"", 706, 1);
#nullable restore
#line 13 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 642, string.Concat(nameof(Collateral), "_", nameof(Collateral.Type)), 642, 64, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"");
            BeginWriteAttribute("value", " value=\"", 728, "\"", 758, 1);
#nullable restore
#line 13 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 736, Model.RealEstate.Type, 736, 22, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" disabled />\r\n\t\t\t</div>\r\n\t\t</div>\r\n\t\t<div class=\"row py-3\">\r\n\t\t\t<div class=\"col-4\">\r\n\t\t\t\t<label");
            BeginWriteAttribute("for", " for=\"", 854, "\"", 936, 1);
#nullable restore
#line 18 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 860, string.Concat(nameof(Collateral), "_", nameof(Collateral.InitialAssesDate)), 860, 76, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 937, "\"", 945, 0);
            EndWriteAttribute();
            WriteLiteral(">Initial Asses Date</label>\r\n\t\t\t\t<input");
            BeginWriteAttribute("id", " id=\"", 985, "\"", 1066, 1);
#nullable restore
#line 19 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 990, string.Concat(nameof(Collateral), "_", nameof(Collateral.InitialAssesDate)), 990, 76, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"");
            BeginWriteAttribute("value", " value=\"", 1088, "\"", 1130, 1);
#nullable restore
#line 19 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 1096, Model.RealEstate.InitialAssesDate, 1096, 34, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" disabled />\r\n\t\t\t</div>\r\n\t\t\t<div class=\"col-4\">\r\n\t\t\t\t<label");
            BeginWriteAttribute("for", " for=\"", 1190, "\"", 1270, 1);
#nullable restore
#line 22 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 1196, string.Concat(nameof(Collateral), "_", nameof(Collateral.LastAssessDate)), 1196, 74, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 1271, "\"", 1279, 0);
            EndWriteAttribute();
            WriteLiteral(">Last Assess Date</label>\r\n\t\t\t\t<input");
            BeginWriteAttribute("id", " id=\"", 1317, "\"", 1396, 1);
#nullable restore
#line 23 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 1322, string.Concat(nameof(Collateral), "_", nameof(Collateral.LastAssessDate)), 1322, 74, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"");
            BeginWriteAttribute("value", " value=\"", 1418, "\"", 1458, 1);
#nullable restore
#line 23 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 1426, Model.RealEstate.LastAssessDate, 1426, 32, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" disabled />\r\n\t\t\t</div>\r\n\t\t</div>\r\n\t\t<div class=\"row py-3\">\r\n\t\t\t<div class=\"col-4\">\r\n\t\t\t\t<label");
            BeginWriteAttribute("for", " for=\"", 1554, "\"", 1657, 1);
#nullable restore
#line 28 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 1560, string.Concat(nameof(Collateral), "_", nameof(RealEstate), "_", nameof(RealEstate.InitialValue)), 1560, 97, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 1658, "\"", 1666, 0);
            EndWriteAttribute();
            WriteLiteral(">Initial Value</label>\r\n\t\t\t\t<input");
            BeginWriteAttribute("id", " id=\"", 1701, "\"", 1803, 1);
#nullable restore
#line 29 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 1706, string.Concat(nameof(Collateral), "_", nameof(RealEstate), "_", nameof(RealEstate.InitialValue)), 1706, 97, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"");
            BeginWriteAttribute("value", " value=\"", 1825, "\"", 1863, 1);
#nullable restore
#line 29 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 1833, Model.RealEstate.InitialValue, 1833, 30, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" disabled />\r\n\t\t\t</div>\r\n\t\t\t<div class=\"col-4\">\r\n\t\t\t\t<label");
            BeginWriteAttribute("for", " for=\"", 1923, "\"", 2026, 1);
#nullable restore
#line 32 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 1929, string.Concat(nameof(Collateral), "_", nameof(RealEstate), "_", nameof(RealEstate.CurrentValue)), 1929, 97, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 2027, "\"", 2035, 0);
            EndWriteAttribute();
            WriteLiteral(">Current Value</label>\r\n\t\t\t\t<input");
            BeginWriteAttribute("id", " id=\"", 2070, "\"", 2172, 1);
#nullable restore
#line 33 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 2075, string.Concat(nameof(Collateral), "_", nameof(RealEstate), "_", nameof(RealEstate.CurrentValue)), 2075, 97, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"");
            BeginWriteAttribute("value", " value=\"", 2194, "\"", 2232, 1);
#nullable restore
#line 33 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 2202, Model.RealEstate.CurrentValue, 2202, 30, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" disabled />\r\n\t\t\t</div>\r\n\t\t</div>\r\n\t\t<div class=\"row py-3\">\r\n\t\t\t<div class=\"col-4\">\r\n\t\t\t\t<label");
            BeginWriteAttribute("for", " for=\"", 2328, "\"", 2441, 1);
#nullable restore
#line 38 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 2334, string.Concat(nameof(Collateral), "_", nameof(RealEstate), "_", nameof(RealEstate.InitialLandPriceInSqFt)), 2334, 107, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 2442, "\"", 2450, 0);
            EndWriteAttribute();
            WriteLiteral(">Initial Land Price (per. sq. ft.)</label>\r\n\t\t\t\t<input");
            BeginWriteAttribute("id", " id=\"", 2505, "\"", 2617, 1);
#nullable restore
#line 39 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 2510, string.Concat(nameof(Collateral), "_", nameof(RealEstate), "_", nameof(RealEstate.InitialLandPriceInSqFt)), 2510, 107, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"");
            BeginWriteAttribute("value", " value=\"", 2639, "\"", 2687, 1);
#nullable restore
#line 39 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 2647, Model.RealEstate.InitialLandPriceInSqFt, 2647, 40, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" disabled />\r\n\t\t\t</div>\r\n\t\t\t<div class=\"col-4\">\r\n\t\t\t\t<label");
            BeginWriteAttribute("for", " for=\"", 2747, "\"", 2860, 1);
#nullable restore
#line 42 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 2753, string.Concat(nameof(Collateral), "_", nameof(RealEstate), "_", nameof(RealEstate.CurrentLandPriceInSqFt)), 2753, 107, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 2861, "\"", 2869, 0);
            EndWriteAttribute();
            WriteLiteral(">Current Land Price (per sq. ft.)</label>\r\n\t\t\t\t<input");
            BeginWriteAttribute("id", " id=\"", 2923, "\"", 3035, 1);
#nullable restore
#line 43 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 2928, string.Concat(nameof(Collateral), "_", nameof(RealEstate), "_", nameof(RealEstate.CurrentLandPriceInSqFt)), 2928, 107, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"");
            BeginWriteAttribute("value", " value=\"", 3057, "\"", 3105, 1);
#nullable restore
#line 43 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 3065, Model.RealEstate.CurrentLandPriceInSqFt, 3065, 40, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" disabled />\r\n\t\t\t</div>\r\n\t\t</div>\r\n\t\t<div class=\"row py-3\">\r\n\t\t\t<div class=\"col-4\">\r\n\t\t\t\t<label");
            BeginWriteAttribute("for", " for=\"", 3201, "\"", 3313, 1);
#nullable restore
#line 48 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 3207, string.Concat(nameof(Collateral), "_", nameof(RealEstate), "_", nameof(RealEstate.InitialStructurePrice)), 3207, 106, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 3314, "\"", 3322, 0);
            EndWriteAttribute();
            WriteLiteral(">Initial Structure Price (per. sq. ft.)</label>\r\n\t\t\t\t<input");
            BeginWriteAttribute("id", " id=\"", 3382, "\"", 3493, 1);
#nullable restore
#line 49 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 3387, string.Concat(nameof(Collateral), "_", nameof(RealEstate), "_", nameof(RealEstate.InitialStructurePrice)), 3387, 106, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"");
            BeginWriteAttribute("value", " value=\"", 3515, "\"", 3562, 1);
#nullable restore
#line 49 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 3523, Model.RealEstate.InitialStructurePrice, 3523, 39, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" disabled />\r\n\t\t\t</div>\r\n\t\t\t<div class=\"col-4\">\r\n\t\t\t\t<label");
            BeginWriteAttribute("for", " for=\"", 3622, "\"", 3728, 1);
#nullable restore
#line 52 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 3628, string.Concat(nameof(Collateral), "_", nameof(Land), "_", nameof(RealEstate.CurrentStructurePrice)), 3628, 100, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 3729, "\"", 3737, 0);
            EndWriteAttribute();
            WriteLiteral(">Current Structure Price (per sq. ft.)</label>\r\n\t\t\t\t<input");
            BeginWriteAttribute("id", " id=\"", 3796, "\"", 3901, 1);
#nullable restore
#line 53 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 3801, string.Concat(nameof(Collateral), "_", nameof(Land), "_", nameof(RealEstate.CurrentStructurePrice)), 3801, 100, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"");
            BeginWriteAttribute("value", " value=\"", 3923, "\"", 3970, 1);
#nullable restore
#line 53 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Shared\RealEstateView.cshtml"
WriteAttributeValue("", 3931, Model.RealEstate.CurrentStructurePrice, 3931, 39, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" disabled />\r\n\t\t\t</div>\r\n\t\t</div>\r\n\t</div>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<CollateralLoanMVC.ViewModels.ViewCollateralViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
