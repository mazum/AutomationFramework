using Castle.Components.DictionaryAdapter;
using TechTalk.SpecFlow;

namespace Framework.Utils
{
    public static class FeatureContextData
    {
        private static IFeatureContext context;

        public static IFeatureContext Current
        {
            get
            {
                if (context == null)
                {
                    var factory = new DictionaryAdapterFactory();
                    context = factory.GetAdapter<IFeatureContext>(FeatureContext.Current);
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