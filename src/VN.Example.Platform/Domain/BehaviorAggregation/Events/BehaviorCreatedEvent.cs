using Newtonsoft.Json.Linq;

namespace VN.Example.Platform.Domain.BehaviorAggregation.Events
{
    public class BehaviorCreatedEvent
    {
        public long Id { get; private set; }

        public string IP { get; private set; }

        public string PageName { get; private set; }

        public string UserAgent { get; private set; }

        public JObject PageParameters { get; private set; }

        public BehaviorCreatedEvent(long id, string ip, string pageName, string userAgent, JObject pageParameters)
        {
            Id = id;
            IP = ip;
            PageName = pageName;
            UserAgent = userAgent;
            PageParameters = pageParameters;
        }
    }
}
