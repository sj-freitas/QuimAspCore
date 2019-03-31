using Microsoft.AspNetCore.Mvc;
using QuimAspCore.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QuimAspCore
{
    public class ActionHelper<TController>
    {
        public string Controller => typeof(TController)
            .Name
            .RemoveEnding(
                "Controller", 
                StringComparison.InvariantCultureIgnoreCase);

        public IUrlHelper Url { get; }

        public ActionHelper(IUrlHelper url)
        {
            Url = url;
        }
    }

    public static class UrlHelper
    {
        public static ActionHelper<TController> Controller<TController>(this IUrlHelper helper)
        {
            return new ActionHelper<TController>(helper);
        }

        public static string Action<TController, TResult>(
            this ActionHelper<TController> actionHelper,
            Expression<Func<TController, TResult>> actionInvocationExpression)
        {
            if (actionInvocationExpression == null)
            {
                throw new ArgumentNullException(nameof(actionInvocationExpression));
            }

            var expressionBody = actionInvocationExpression.ThrowsIfNull(t => t.Body);
            var invocation = expressionBody.ThrowIfCastFails<MethodCallExpression>();

            // Get the method
            var method = invocation.ThrowsIfNull(t => t.Method);
            var methodParameters = method.ThrowsIfNull(t => t.GetParameters());

            // Get all the arguments
            var parameters = invocation
                .Arguments
                .Select(t => Expression.Convert(t, typeof(object)))
                .Select(t => Expression.Lambda<Func<object>>(t))
                .Select(t => t.Compile())
                .Select(t => t());

            var result = CreateValue(methodParameters, parameters);

            return actionHelper
                .Url
                .Action(method.Name, actionHelper.Controller, result);
        }

        private static object CreateValue(IEnumerable<ParameterInfo> methodParameters,
            IEnumerable<object> values)
        {
            var valuesToAdd = methodParameters
                .Zip(values, (p, v) => new
                {
                    p.Name,
                    Value = v
                });

            dynamic expando = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expando;
            foreach (var curr in valuesToAdd)
            {
                dictionary.Add(curr.Name, curr.Value);
            }

            return expando;
        }
    }

    public static class StringExtensions
    {
        public static string RemoveEnding(this string text, string value,
            StringComparison stringComparison)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var lastIndex = text.LastIndexOf(value, stringComparison);
            if (lastIndex != text.Length - value.Length)
            {
                return text;
            }

            return text.Substring(0, text.Length - value.Length);
        }
    }
}
