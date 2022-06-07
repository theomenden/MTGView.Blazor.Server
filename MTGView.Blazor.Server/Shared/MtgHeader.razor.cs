using Blazorise.Localization;

namespace MTGView.Blazor.Server.Shared;

public partial class MtgHeader : ComponentBase
{
    [Inject] public ITextLocalizerService TextLocalizerService { get; init; }
  
    private CultureInfo _selectedCultureName;

    protected override void OnInitialized()
    {
        _selectedCultureName = TextLocalizerService.SelectedCulture;

        SelectedCultureChanged(CultureInfo.CurrentCulture);
    }

    private void SelectedCultureChanged(CultureInfo cultureInfo)
    {
        _selectedCultureName = cultureInfo;

        TextLocalizerService.ChangeLanguage(cultureInfo.Name);
        
        StateHasChanged();
    }

}

