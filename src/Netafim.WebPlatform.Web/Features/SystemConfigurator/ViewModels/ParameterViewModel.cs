using System.Collections;
using System.Collections.Generic;

namespace Netafim.WebPlatform.Web.Features.SystemConfigurator.ViewModels
{
    public class ParameterViewModel : IEnumerable<ConfiguratorParameterViewModel>
    {
        private readonly IEnumerable<ConfiguratorParameterViewModel> parameters;

        public ParameterViewModel(IEnumerable<ConfiguratorParameterViewModel> parameters)
        {
            this.parameters = parameters;   
        }

        public string Title { get; set; }

        public string ValidationError { get; set; }

        public string Tooltip { get; set; }

        public ViewMode ViewMode { get; set; }

        public string Name { get; set; }

        public IEnumerator<ConfiguratorParameterViewModel> GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)parameters).GetEnumerator();
        }
    }

    public class ConfiguratorParameterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class NumbericParameterViewModel : ConfiguratorParameterViewModel
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public int Step { get; set; }
    }

    public class ParameterGroupViewModel
    {
        public string Title { get; set; }

        public IEnumerable<ParameterViewModel> Parameters { get; set; }
    }


    public enum ViewMode
    {
        Dropdown,
        Selection,
        Numberic
    }
}