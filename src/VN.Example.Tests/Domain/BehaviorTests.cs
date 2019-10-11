using Newtonsoft.Json.Linq;
using VN.Example.Platform.Domain.BehaviorAggregation;
using VN.Example.Platform.Domain.BehaviorAggregation.Commands;
using Xunit;

namespace VN.Example.Tests.Domain
{
    public class BehaviorTests
    {
        [Fact]
        public void CreateBehavior_Should_Succeed()
        {
            // Arrange
            string ip = "127.0.0.1";
            string pageName = "home";
            string userAgent = "safari";
            string pageParameters = "{ 'Content-Type':'application/json' }";
            var command = new CreateBehaviorCommand(ip, pageName, userAgent, JObject.Parse(pageParameters));

            // Act
            var behavior = new Behavior(command);

            // Assert
            Assert.NotNull(behavior);
            Assert.Equal(ip, behavior.IP);
            Assert.Equal(pageName, behavior.PageName);
            Assert.Equal(userAgent, behavior.UserAgent);
        }
    }
}
