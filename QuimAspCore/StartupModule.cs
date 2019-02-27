using System;

namespace QuimAspCore
{
    public static class StartupModule
    {
        public static IStartupModule Use<T>(string moduleRoot)
            where T : IStartupModule, new()
        {
            if (moduleRoot == null)
            {
                throw new ArgumentNullException(nameof(moduleRoot));
            }

            return new PrefixedRootModule(new T(), moduleRoot);
        }
    }
}
