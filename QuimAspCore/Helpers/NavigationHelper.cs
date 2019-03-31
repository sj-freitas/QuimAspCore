using System;

namespace QuimAspCore.Helpers
{
    public static class NavigationHelper
    {
        public static T ThrowIfCastFails<T>(this object original)
            where T : class
        {
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (!(original is T casted))
            {
                throw new InvalidOperationException($"{original} must be a {typeof(T).Name}!");
            }

            return casted;
        }

        public static TOut ThrowsIfNull<T, TOut>(this T instance, Func<T, TOut> getValue)
            where T : class
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var result = getValue(instance);
            if (result == null)
            {
                throw new InvalidOperationException(
                    $"Expected value of ${instance} to not be null.");
            }

            return result;
        }
    }
}
