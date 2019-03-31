using System;
using System.Linq;

namespace QuimAspCore
{
    public static class UriHelper
    {
        public static string Combine(params string[] parts)
        {
            if (parts == null)
            {
                throw new ArgumentNullException(nameof(parts));
            }

            var values = string.Join("/", parts)
                .Split('/')
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => $"/{t.Trim()}");

            return string.Join("", values);
        }
    }
}
