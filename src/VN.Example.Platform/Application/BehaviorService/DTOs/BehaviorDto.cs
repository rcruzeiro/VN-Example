using System;
using Newtonsoft.Json.Linq;
using VN.Example.Platform.Domain.BehaviorAggregation;

namespace VN.Example.Platform.Application.BehaviorService.DTOs
{
    public sealed class BehaviorDto
    {
        public long Id { get; internal set; }

        public string IP { get; internal set; }

        public string PageName { get; internal set; }

        public string UserAgent { get; internal set; }

        public JObject PageParameters { get; internal set; }

        public DateTimeOffset CreatedAt { get; internal set; }
    }

    public static class BehaviorDtoExtensions
    {
        public static BehaviorDto Assemble(this Behavior behavior)
        {
            if (behavior == null) return null;

            var dto = new BehaviorDto
            {
                Id = behavior.Id,
                IP = behavior.IP,
                PageName = behavior.PageName,
                UserAgent = behavior.UserAgent,
                PageParameters = behavior.PageParameters,
                CreatedAt = behavior.CreatedAt
            };

            return dto;
        }
    }
}
