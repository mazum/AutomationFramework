using System.Configuration;
using Castle.Components.DictionaryAdapter;

namespace Framework.Utils
{
    public static class AppSettings
    {
        private static IAppSettings context;
        public static IAppSettings Current
        {
            get
            {
                if (context == null)
                {
                    var factory = new DictionaryAdapterFactory();
                    context = factory.GetAdapter<IAppSettings>
                        (ConfigurationManager.AppSettings);
                }
                return context;
            }
        }
        public static void Clear()
        {
            context = null;
        }
    }
}