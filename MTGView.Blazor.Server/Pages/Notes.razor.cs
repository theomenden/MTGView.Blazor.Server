using Blazored.LocalStorage;
namespace MTGView.Blazor.Server.Pages;

public partial class Notes : ComponentBase
{
    [Inject] private INotificationService NotificationService { get; init; }

    [Inject] private ILocalStorageService LocalStorageService { get; init; }

    private string? _markdownValue;

    private string? _markdownHtml;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _markdownValue ??= await LocalStorageService.GetItemAsStringAsync("plainNotes");
            _markdownHtml ??= await LocalStorageService.GetItemAsStringAsync("htmlNotes");

            LocalStorageService.Changed += (sender, e) =>
            {
                Console.WriteLine($"Value for key {e.Key} changed from {e.OldValue} to {e.NewValue}");
            };

            StateHasChanged();
        }
    }

    private async Task OnMarkdownValueChanged(string value)
    {
        _markdownValue = value;

        _markdownHtml = Markdown.ToHtml(_markdownValue ?? string.Empty);

        await LocalStorageService.SetItemAsStringAsync("plainNotes", _markdownValue);

        await LocalStorageService.SetItemAsStringAsync("htmlNotes", _markdownHtml);

        await InvokeAsync(StateHasChanged);
    }

}