using Autodesk.Revit.UI;

namespace RevitPlague.Core.Services;

/// <summary>
/// A task handler to execute actions within the Revit API context.
/// </summary>
public class RevitApiTaskHandler
{
    private readonly ExternalEventHandler _handler;
    private readonly ExternalEvent _externalEvent;
    private TaskCompletionSource<object>? _tcs;

    public RevitApiTaskHandler()
    {
        _handler = new ExternalEventHandler();
        _externalEvent = ExternalEvent.Create(_handler);
    }

    /// <summary>
    /// Executes an action within the Revit API context.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    public Task Run(Action<UIApplication> action)
    {
        _tcs = new TaskCompletionSource<object>();

        _handler.SetAction(app =>
        {
            action(app);
            _tcs?.SetResult(null);
        });

        _externalEvent.Raise();
        return _tcs.Task;
    }

    /// <summary>
    /// Executes a function within the Revit API context and returns a result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="func">The function to execute.</param>
    public Task<TResult> Run<TResult>(Func<UIApplication, TResult> func)
    {
        _tcs = new TaskCompletionSource<object>();

        _handler.SetAction(app =>
        {
            var result = func(app);
            _tcs?.SetResult(result);
        });

        _externalEvent.Raise();
        return _tcs.Task.ContinueWith(t => (TResult)t.Result);
    }

    /// <summary>
    /// Cancels any pending actions.
    /// </summary>
    public void Cancel()
    {
        _handler.ClearAction();
        _tcs?.TrySetCanceled();
    }

    private class ExternalEventHandler : IExternalEventHandler
    {
        private Action<UIApplication>? _action;

        public void SetAction(Action<UIApplication> action)
        {
            if (_action == null)
            {
                _action = action;
            }
            else
            {
                _action += action;
            }
        }

        public void ClearAction()
        {
            _action = null;
        }

        public void Execute(UIApplication app)
        {
            if (_action == null) return;

            try
            {
                _action(app);
            }
            finally
            {
                ClearAction();
            }
        }

        public string GetName()
        {
            return "RevitApiTaskHandler";
        }
    }
}