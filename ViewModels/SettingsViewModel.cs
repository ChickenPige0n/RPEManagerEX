using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using RPEManagerEX.Contracts.Services;
using RPEManagerEX.Helpers;

using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace RPEManagerEX.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    [ObservableProperty]
    private ElementTheme _elementTheme;

    [ObservableProperty]
    private string _rpePath;

    partial void OnRpePathChanged(string value)
    {
        ApplicationData.Current.LocalSettings.SaveString(RPEPathKey,value);
    }

    public const string RPEPathKey = "RPEPath";


    [ObservableProperty]
    private string _versionDescription;

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public Task Initialize
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        var value = ApplicationData.Current.LocalSettings.Values[RPEPathKey];
        if (value == null || !Path.Exists((string)value))
        {
            _rpePath = "";
            Initialize = SelectRPEPath();
        }
        else
        {
            _rpePath = (string)value;
            Initialize = Task.CompletedTask;
        }
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });
    }

    [RelayCommand]
    private async Task SelectRPEPath()
    {
        var folderPicker = new FolderPicker()
        {
            SettingsIdentifier = RPEPathKey
        };
        folderPicker.FileTypeFilter.Add("*");
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, App.MainWindow.GetWindowHandle());
        var folder = await folderPicker.PickSingleFolderAsync();
        if(folder is null) { return; }
        RpePath = folder.Path;
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
