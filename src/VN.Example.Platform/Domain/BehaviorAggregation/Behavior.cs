using System;
using Newtonsoft.Json.Linq;
using VN.Example.Platform.Domain.BehaviorAggregation.Commands;

namespace VN.Example.Platform.Domain.BehaviorAggregation
{
    public class Behavior : IEntity<long>, IAggregation
    {
        public long Id { get; private set; }

        public string IP { get; private set; }

        public string PageName { get; private set; }

        public string UserAgent { get; private set; }

        public JObject PageParameters { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        protected Behavior()
        { }

        public Behavior(CreateBehaviorCommand command)
        {
            command.Validate();

            IP = command.IP;
            PageName = command.PageName;
            UserAgent = command.UserAgent;
            PageParameters = command.PageParameters;
            CreatedAt = DateTimeOffset.UtcNow;
        }
    }
}
