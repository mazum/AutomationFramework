using System.Collections.Concurrent;

namespace Framework.Utils
{
    public interface IFeatureContext
    {
        ConcurrentBag<MailItem> UsedEmailsBag { get; set; }
    }
}