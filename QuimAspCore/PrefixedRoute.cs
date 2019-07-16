using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace QuimAspCore
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
                .Select(controller => new
                {
                    ControllerSelectors = controller.Selectors,
                    ActionSelectors = controller
                        .Actions
                        .SelectMany(action => action.Selectors)
                });

            foreach (var selectorModel in matchedSelectors)
            {
                var root = _prefix;
                foreach (var curr in selectorModel
                    .ControllerSelectors
                    .Select(t => t.AttributeRouteModel)
                    .Where(t => t != null))
                {
                    root = UriHelper.Combine(_prefix, curr.Template);
                    curr.Template = root;
                }

                foreach (var curr in selectorModel
                    .ActionSelectors
                    .Select(t => t.AttributeRouteModel)
                    .Where(t => t != null))
                {
                    var template = UriHelper.Combine(root, curr.Template);
                    curr.Template = template;
                }
            }
        }
    }
}
