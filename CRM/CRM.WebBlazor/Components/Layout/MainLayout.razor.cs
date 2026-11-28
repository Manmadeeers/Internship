using Microsoft.JSInterop;
using MudBlazor;

namespace CRM.WebBlazor.Components.Layout;

public partial class MainLayout
{
    private bool _drawerOpen = true;
    private bool _isDarkMode = true;
    private bool _isInitialized = false;

    private string _currentIcon = Icons.Material.Filled.Brightness4;

    private MudTheme _theme = new MudTheme()
    {
        PaletteLight = new PaletteLight()
        {
            Primary = Colors.DeepPurple.Default,
            AppbarBackground = Colors.DeepPurple.Default,
        },
        PaletteDark = new PaletteDark()
        {
            Primary = Colors.DeepPurple.Lighten2,
            AppbarBackground = Colors.DeepPurple.Lighten2,
        }
    };

    private  void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    
    private async Task UpdateDarkMode()
    {
        _isDarkMode = !_isDarkMode;
        _currentIcon = _isDarkMode ? Icons.Material.Filled.Brightness4 : Icons.Material.Filled.Brightness7;
        await JsRuntime.InvokeVoidAsync("localStorage.setItem","dark-mode", _isDarkMode.ToString());
    }

}