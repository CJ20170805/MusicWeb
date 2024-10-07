using System;

namespace MusicApp.Admin.Services;
public class ThemeService
{
    public event Action? OnChange;
    private bool _isDarkMode = true;

    public bool IsDarkMode
    {
        get => _isDarkMode;
        private set
        {
            if (_isDarkMode != value)
            {
                _isDarkMode = value;
                NotifyStateChanged();
            }
        }
    }

    public void ToggleDarkMode()
    {
        IsDarkMode = !_isDarkMode;
    }

    private void NotifyStateChanged()
    {
        OnChange?.Invoke(); // Raise the event, Blazor components will handle UI updates
    }
}
