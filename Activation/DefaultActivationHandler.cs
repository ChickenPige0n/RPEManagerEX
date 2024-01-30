using Microsoft.UI.Xaml;

using RPEManagerEX.Contracts.Services;
using RPEManagerEX.ViewModels;

namespace RPEManagerEX.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(列表详细信息ViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
