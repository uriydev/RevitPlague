using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Extensions;
using Nice3point.Revit.Toolkit;
using RevitPlague.Services.Contracts;
using Wpf.Ui;

namespace RevitPlague.Services;

public sealed class PlagueService : IPlagueService
{
    private static Dispatcher _dispatcher;
    private PlagueServiceImpl _plagueService;
    
    static PlagueService()
    {
        var uiThread = new Thread(Dispatcher.Run);
        uiThread.SetApartmentState(ApartmentState.STA);
        uiThread.Start();
        
        EnsureThreadStart(uiThread);
    }
    
    public PlagueService(IServiceScopeFactory scopeFactory)
    {
        if (_dispatcher.CheckAccess())
        {
            _plagueService = new PlagueServiceImpl(scopeFactory);
        }
        else
        {
            _dispatcher.Invoke(() => _plagueService = new PlagueServiceImpl(scopeFactory));
        }
    }
    
    public IPlagueServiceShowStage DependsOn(IServiceProvider provider)
    {
        if (_dispatcher.CheckAccess())
        {
            _plagueService.DependsOn(provider);
        }
        else
        {
            _dispatcher.Invoke(() => _plagueService.DependsOn(provider));
        }
        
        return this;
    }
    
    public IPlagueServiceExecuteStage Show<T>() where T : Page
    {
        if (_dispatcher.CheckAccess())
        {
            _plagueService.Show<T>();
        }
        else
        {
            _dispatcher.Invoke(() => _plagueService.Show<T>());
        }
        
        return this;
    }
    
    public void Execute<T>(Action<T> handler) where T : class
    {
        if (_dispatcher.CheckAccess())
        {
            _plagueService.Execute(handler);
        }
        else
        {
            _dispatcher.Invoke(() => _plagueService.Execute(handler));
        }
    }
    
    private static void EnsureThreadStart(Thread thread)
    {
        Dispatcher dispatcher = null;
        SpinWait spinWait = new();
        while (dispatcher is null)
        {
            spinWait.SpinOnce();
            dispatcher = Dispatcher.FromThread(thread);
        }
        
        Thread.Sleep(1);
        
        _dispatcher = dispatcher;
    }
    
    private sealed class PlagueServiceImpl
    {
        private Window _owner;
        private Task _activeTask;
        private readonly IServiceScope _scope;
        private readonly INavigationService _navigationService;
        private readonly Window _window;
        
        public PlagueServiceImpl(IServiceScopeFactory scopeFactory)
        {
            _scope = scopeFactory.CreateScope();
            
            _window = (Window) _scope.ServiceProvider.GetRequiredService<IWindow>();
            _navigationService = _scope.ServiceProvider.GetRequiredService<INavigationService>();
            
            _window.Closed += (_, _) => _scope.Dispose();
        }
        
        public void DependsOn(IServiceProvider provider)
        {
            _owner = (Window) provider.GetService<IWindow>();
        }
        
        public void Show<T>() where T : Page
        {
            if (_activeTask is null || _activeTask.IsCompleted)
            {
                ShowPage<T>();
            }
            else
            {
                _activeTask = _activeTask.ContinueWith(_ => ShowPage<T>(), TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        
        public void Execute<T>(Action<T> handler) where T : class
        {
            if (_activeTask is null || _activeTask.IsCompleted)
            {
                InvokeHandler(handler);
            }
            else
            {
                _activeTask = _activeTask.ContinueWith(_ => InvokeHandler(handler), TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        
        private void InvokeHandler<T>(Action<T> handler) where T : class
        {
            var service = _scope.ServiceProvider.GetRequiredService<T>();
            handler.Invoke(service);
        }
        
        private void ShowPage<T>() where T : Page
        {
            if (_owner is null)
            {
                _window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                _window.Left = _owner.Left + 47;
                _window.Top = _owner.Top + 49;
            }
            
            _window.Show(Context.UiApplication.MainWindowHandle);
            _navigationService.Navigate(typeof(T));
        }
    }
}