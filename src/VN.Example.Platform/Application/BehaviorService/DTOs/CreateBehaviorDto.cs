using System;
using Newtonsoft.Json.Linq;
using VN.Example.Platform.Domain.BehaviorAggregation.Commands;

namespace VN.Example.Platform.Application.BehaviorService.DTOs
{
    public sealed class CreateBehaviorDto
    {
        public string IP { get; set; }

        public string PageName { get; set; }

        public string UserAgent { get; set; }

        public JObject PageParameters { get; set; }
    }

    public static class CreateBehaviorDtoExtensions
    {
        public static CreateBehaviorCommand Assemble(this CreateBehaviorDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var command = new CreateBehaviorCommand(dto.IP,
                                                    dto.PageName,
                                                    dto.UserAgent,
                                                    dto.PageParameters);
            return command;
        }
    }
}
