using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Nice3point.Revit.Extensions;
using Nice3point.Revit.Toolkit;
using RevitPlague.Services.Contracts;
using Wpf.Ui;

namespace RevitPlague.Services;

/// <summary>
/// Класс PlagueService предоставляет механизм выполнения действий, связанных с UI, 
/// на выделенном UI-потоке, а также управляет областью служб для навигации и других операций.
/// Обеспечивает потокобезопасность и инкапсулирует логику, необходимую для работы с WPF UI.
/// </summary>
public sealed class PlagueService : IPlagueService
{
    private static Dispatcher _dispatcher;
    private PlagueServiceImpl _plagueService;

    /// <summary>
    /// Статический конструктор инициализирует выделенный UI-поток и его диспетчер.
    /// Гарантирует возможность выполнения операций WPF на потоке с состоянием STA.
    /// </summary>
    static PlagueService()
    {
        var uiThread = new Thread(Dispatcher.Run);
        uiThread.SetApartmentState(ApartmentState.STA); // Устанавливает для потока состояние STA для работы с WPF.
        uiThread.Start();
        
        EnsureThreadStart(uiThread); // Ожидает, пока диспетчер станет доступным.
    }

    /// <summary>
    /// Конструктор класса. Создает экземпляр реализации PlagueServiceImpl, 
    /// обеспечивая выполнение на UI-потоке.
    /// </summary>
    /// <param name="scopeFactory">Фабрика для создания области службы (Service Scope).</param>
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

    /// <summary>
    /// Метод, задающий зависимость от предоставленного IServiceProvider.
    /// Выполняется на UI-потоке для обеспечения потокобезопасности.
    /// </summary>
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

    /// <summary>
    /// Отображает страницу, указанную типом T, на UI-потоке.
    /// </summary>
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

    /// <summary>
    /// Выполняет указанную обработку для объекта типа T на UI-потоке.
    /// </summary>
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

    /// <summary>
    /// Метод для ожидания готовности диспетчера на выделенном UI-потоке.
    /// </summary>
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

    // Реализация логики PlagueService в отдельном вложенном классе.
    private sealed class PlagueServiceImpl
    {
        private Window _owner;
        private Task _activeTask;
        private readonly IServiceScope _scope;
        private readonly INavigationService _navigationService;
        private readonly Window _window;

        /// <summary>
        /// Конструктор PlagueServiceImpl. Создает область служб и настраивает зависимости для окна и навигации.
        /// </summary>
        public PlagueServiceImpl(IServiceScopeFactory scopeFactory)
        {
            _scope = scopeFactory.CreateScope();
            _window = (Window) _scope.ServiceProvider.GetRequiredService<IWindow>();
            _navigationService = _scope.ServiceProvider.GetRequiredService<INavigationService>();
            
            // Подписка на событие закрытия окна для освобождения области служб.
            _window.Closed += (_, _) => _scope.Dispose();
        }

        /// <summary>
        /// Задает родительское окно для текущего окна.
        /// </summary>
        public void DependsOn(IServiceProvider provider)
        {
            _owner = (Window) provider.GetService<IWindow>();
        }

        /// <summary>
        /// Отображает указанную страницу типа T.
        /// </summary>
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

        /// <summary>
        /// Выполняет действие для объекта типа T.
        /// </summary>
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

        /// <summary>
        /// Вызывает обработчик для объекта типа T.
        /// </summary>
        private void InvokeHandler<T>(Action<T> handler) where T : class
        {
            var service = _scope.ServiceProvider.GetRequiredService<T>();
            handler.Invoke(service);
        }

        /// <summary>
        /// Отображает страницу типа T в текущем окне.
        /// </summary>
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