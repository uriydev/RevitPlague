using Autodesk.Revit.UI;

namespace RevitPlague.Core.Services;

/// <summary>
/// Handles execution of actions or functions in the Revit API context.
/// </summary>
public class RevitApiTaskExecutor
{
    private readonly RevitExternalEventHandler _eventHandler;
    private readonly ExternalEvent _externalEvent;
    private TaskCompletionSource<object?>? _taskCompletionSource;

    public RevitApiTaskExecutor()
    {
        _eventHandler = new RevitExternalEventHandler();
        _externalEvent = ExternalEvent.Create(_eventHandler);
    }

    /// <summary>
    /// Executes an action within the Revit API context.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    public Task ExecuteAsync(Action<UIApplication> action)
    {
        _taskCompletionSource = new TaskCompletionSource<object?>();

        _eventHandler.QueueAction(app =>
        {
            action(app);
            _taskCompletionSource?.SetResult(null);
        });

        _externalEvent.Raise();
        return _taskCompletionSource.Task;
    }

    /// <summary>
    /// Executes a function within the Revit API context and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="func">The function to execute.</param>
    public Task<TResult> ExecuteAsync<TResult>(Func<UIApplication, TResult> func)
    {
        _taskCompletionSource = new TaskCompletionSource<object?>();

        _eventHandler.QueueAction(app =>
        {
            var result = func(app);
            _taskCompletionSource?.SetResult(result);
        });

        _externalEvent.Raise();
        return _taskCompletionSource.Task.ContinueWith(t => (TResult)t.Result);
    }

    /// <summary>
    /// Cancels any pending actions.
    /// </summary>
    public void Cancel()
    {
        _eventHandler.ResetAction();
        _taskCompletionSource?.TrySetCanceled();
    }

    private class RevitExternalEventHandler : IExternalEventHandler
    {
        private Action<UIApplication>? _queuedAction;

        public void QueueAction(Action<UIApplication> action)
        {
            if (_queuedAction == null)
            {
                _queuedAction = action;
            }
            else
            {
                _queuedAction += action;
            }
        }

        public void ResetAction()
        {
            _queuedAction = null;
        }

        public void Execute(UIApplication app)
        {
            if (_queuedAction == null) return;

            try
            {
                _queuedAction(app);
            }
            finally
            {
                ResetAction();
            }
        }

        public string GetName()
        {
            return nameof(RevitExternalEventHandler);
        }
    }
}