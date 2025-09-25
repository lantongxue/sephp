using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReactiveUI;
using sephp.ViewModels;
using System;

namespace sephp
{
    public class ViewLocator : IDataTemplate, IViewLocator
    {

        public Control? Build(object? param)
        {
            if (param is null)
                return null;

            var name = param.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }

        public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
        {
            if (viewModel is null)
            {
                return null;
            }

            var viewModelType = viewModel.GetType();
            var assembly = viewModelType.Assembly;

            var viewName = viewModelType.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
            var viewType = assembly.GetType(viewName);
            if (viewType != null)
            {
                var view = Activator.CreateInstance(viewType) as UserControl;
                if (view != null)
                {
                    view.DataContext = viewModel;
                    return (IViewFor)view;
                }
            }
            throw new ArgumentOutOfRangeException(nameof(viewModel));
        }
    }
}
