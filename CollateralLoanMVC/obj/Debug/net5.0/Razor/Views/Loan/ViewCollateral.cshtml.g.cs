#pragma checksum "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Loan\ViewCollateral.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e8bdc5e7f5f3134fe530b7b61b7938792a50c0b9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Loan_ViewCollateral), @"mvc.1.0.view", @"/Views/Loan/ViewCollateral.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e8bdc5e7f5f3134fe530b7b61b7938792a50c0b9", @"/Views/Loan/ViewCollateral.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7d0cad2cdcc657571374ef8f0269f851741b00f5", @"/Views/_ViewImports.cshtml")]
    public class Views_Loan_ViewCollateral : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CollateralLoanMVC.ViewModels.ViewCollateralViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Loan\ViewCollateral.cshtml"
 if (@Model.Land != null)
{
	

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Loan\ViewCollateral.cshtml"
Write(await Html.PartialAsync("LandView"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Loan\ViewCollateral.cshtml"
                                        
}
else
{
	

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Loan\ViewCollateral.cshtml"
Write(await Html.PartialAsync("RealEstateView"));

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\Dell\Visual Studio Workplace\CollateralLoans\CollateralLoanMVC\Views\Loan\ViewCollateral.cshtml"
                                              
}

#line default
#line hidden
#nullable disable
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
