using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace VN.Example.Platform.Domain.BehaviorAggregation.Commands
{
    public sealed class CreateBehaviorCommand : ICommand
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "IP is a required parameter.")]
        [StringLength(15, ErrorMessage = "Invalid IP length")]
        public string IP { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Page name is a required parameter.")]
        public string PageName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "User agent name is a required parameter.")]
        public string UserAgent { get; set; }

        public JObject PageParameters { get; set; }

        public CreateBehaviorCommand(string ip, string pageName, string userAgent, JObject pageParameters = null)
        {
            IP = ip;
            PageName = pageName;
            UserAgent = userAgent;
            PageParameters = pageParameters;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => new List<ValidationResult>();
    }
}
