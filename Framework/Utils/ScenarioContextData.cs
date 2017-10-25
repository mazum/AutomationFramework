using Castle.Components.DictionaryAdapter;
using TechTalk.SpecFlow;

namespace Framework.Utils
{
    public static class ScenarioContextData
    {
        private static IScenarioContext context;
        public static IScenarioContext Current
        {
            get
            {
                if (context == null)
                {
                    var factory = new DictionaryAdapterFactory();
                    context = factory.GetAdapter<IScenarioContext>
                        (ScenarioContext.Current);
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