using System.Reflection;
using Microsoft.AspNetCore.Components.Authorization;

namespace MTGView.Blazor.Server.Shared;

public partial class MtgFooter : ComponentBase
{
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; init; }

    [Parameter] public EventCallback<string> ThemeColorChanged { get; init; }


    private static string AssemblyProductVersion
    {
        get
        {
            var attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
            return attributes.Length == 0 ?
                String.Empty :
                ((AssemblyFileVersionAttribute)attributes[0]).Version;
        }
    }

    private static string ApplicationDevelopmentCompany
    {
        get
        {
            var attributes = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            return attributes.Length == 0 ?
                String.Empty :
                ((AssemblyCompanyAttribute)attributes[0]).Company;
        }
    }
}