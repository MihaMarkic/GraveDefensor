using Autofac;
using GraveDefensor.Engine.Designer.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GraveDefensor.Engine.Designer.ViewModels
{
    public sealed class MainViewModel : ViewModel
    {
        public event EventHandler? ExitRequested;
        public RelayCommand ExitCommand { get; }
        public RelayCommandAsync ShowSettingsCommand { get; }
        public ContentViewModel Content { get; private set; } = default!;
        public bool IsInitializing { get; private set; }
        ILifetimeScope containerScope = default!;
        readonly SettingsViewModel settingsViewModel;
        Type? previousContentType;
        public MainViewModel(SettingsViewModel settingsViewModel)
        {
            this.settingsViewModel = settingsViewModel;
            ShowSettingsCommand = new RelayCommandAsync(() => ShowContentForAsync<SettingsViewModel>());
            ExitCommand = new RelayCommand(Exit);
            var ignore = ShowContentForAsync<HomeViewModel>();
        }
        void OnExitRequested(EventArgs e) => ExitRequested?.Invoke(this, e);
        public async Task InitAsync(CancellationToken ct = default)
        {
            IsInitializing = true;
            try
            {
                await settingsViewModel.LoadConfigurationAsync();
            }
            finally
            {
                IsInitializing = false;
            }
        }
        public void Exit() => OnExitRequested(EventArgs.Empty);

        public Task ShowContentForAsync<T>(CancellationToken ct = default)
            where T : notnull, ContentViewModel
        {
            return ShowContentForAsync(typeof(T), ct);
        }
        public async Task ShowContentForAsync(Type contentType, CancellationToken ct = default)
        {
            if (Content != null)
            {
                bool canClose = await Content.CanCloseAsync(ct);
                if (!canClose)
                {
                    return;
                }
                await Content.CloseAsync();
                Content.CloseRequested -= Content_CloseRequested;
                previousContentType = Content.GetType();
            }
            containerScope?.Dispose();
            containerScope = IoC.BeginLifetimeScope();
            var settings = (ContentViewModel)containerScope.Resolve(contentType);
            Content = settings;
            Content.CloseRequested += Content_CloseRequested;
            await Content.InitAsync(ct);
        }

        async void Content_CloseRequested(object? sender, EventArgs e)
        {
            if (previousContentType != null)
            {
                await ShowContentForAsync(previousContentType);
            }
        }
    }
}
