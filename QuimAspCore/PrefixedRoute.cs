using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;
using System.Reflection;

namespace Lib.AspNetCore
{
    public class PrefixedRoute : IApplicationModelConvention
    {
        private readonly string _prefix;
        private readonly Assembly _moduleAssembly;

        public PrefixedRoute(string prefix, Assembly assembly)
        {
            _prefix = prefix;
            _moduleAssembly = assembly;
        }

        public void Apply(ApplicationModel application)
        {
            var matchedSelectors = application
                .Controllers
                .Where(t => t.ControllerType.Assembly == _moduleAssembly)
                .SelectMany(t => t.Selectors)
                .Where(t => t.AttributeRouteModel != null);
            
            foreach (var selectorModel in matchedSelectors)
            {
                var mergedAttributes = $"{_prefix}{selectorModel.AttributeRouteModel.Template}";
                var attribute = new RouteAttribute(mergedAttributes);

                selectorModel.AttributeRouteModel = new AttributeRouteModel(attribute);
            }
        }
    }
}
