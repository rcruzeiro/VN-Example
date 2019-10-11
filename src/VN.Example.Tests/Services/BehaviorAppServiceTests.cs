using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using VN.Example.Platform.Application.BehaviorService;
using VN.Example.Platform.Application.BehaviorService.DTOs;
using VN.Example.Platform.Domain.BehaviorAggregation;
using Xunit;

namespace VN.Example.Tests.Services
{
    public class BehaviorAppServiceTests
    {
        private static readonly List<Behavior> _behaviors = new List<Behavior>();
        private readonly Mock<IBehaviorAppService> _behaviorAppService;

        public BehaviorAppServiceTests()
        {
            _behaviorAppService = new Mock<IBehaviorAppService>();

            BehaviorAppServiceMockSetup();
        }

        [Fact]
        public async Task CreateBehavior_Should_Succeed()
        {
            // Arrange
            string ip = "127.0.0.1";
            string pageName = "home";
            string userAgent = "safari";
            var dto = new CreateBehaviorDto { IP = ip, PageName = pageName, UserAgent = userAgent };

            // Act
            var service = _behaviorAppService.Object;
            var task = Task.Run(() => service.CreateBehaviorAsync(dto));
            Exception ex = await Record.ExceptionAsync(async () => await task);

            // Assert
            Assert.Null(ex);
        }

        private void BehaviorAppServiceMockSetup()
        {
            _behaviorAppService.Setup(bas => bas.CreateBehaviorAsync(It.IsAny<CreateBehaviorDto>(), It.IsAny<CancellationToken>()))
                .Callback<CreateBehaviorDto, CancellationToken>((dto, ct) => Task.Run(() =>
                {
                    var command = dto.Assemble();
                    var behavior = new Behavior(command);

                    _behaviors.Add(behavior);
                }))
                .Returns(Task.CompletedTask);
        }
    }
}
