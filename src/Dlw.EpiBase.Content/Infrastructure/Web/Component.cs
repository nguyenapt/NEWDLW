using System;
using System.Web.Mvc;

namespace Dlw.EpiBase.Content.Infrastructure.Web
{
    public class Component : IDisposable
    {
        private readonly ViewContext _viewContext;
        private readonly Action<ViewContext> _disposeAction;

        private bool _disposed = false;

        public Component(ViewContext viewContext, Action<ViewContext> disposeAction = null)
        {
            _viewContext = viewContext;
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (_disposeAction != null) _disposeAction(_viewContext);
            }
        }

        public void EndComponent()
        {
            Dispose(true);
        }
    }
}