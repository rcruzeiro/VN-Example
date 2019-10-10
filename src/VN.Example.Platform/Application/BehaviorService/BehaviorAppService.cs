using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VN.Example.Infrastructure.Provider.MessageBus;
using VN.Example.Platform.Application.BehaviorService.DTOs;
using VN.Example.Platform.Application.BehaviorService.Exceptions;
using VN.Example.Platform.Domain.BehaviorAggregation;
using VN.Example.Platform.Domain.BehaviorAggregation.Events;
using VN.Example.Platform.Domain.BehaviorAggregation.Specifications;

namespace VN.Example.Platform.Application.BehaviorService
{
    public sealed class BehaviorAppService : IBehaviorAppService
    {
        private readonly IBehaviorRepository _behaviorRepository;
        private readonly IMessageService _messageService;

        public BehaviorAppService(IBehaviorRepository behaviorRepository, IMessageService messageService)
        {
            _behaviorRepository = behaviorRepository ?? throw new ArgumentNullException(nameof(behaviorRepository));
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        }

        public async Task<IEnumerable<BehaviorDto>> GetBehaviorsByIPAsync(string ip, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ip)) throw new ArgumentNullException(nameof(ip));

                var spec = new BehaviorByIPSpecification(ip);
                var behaviors = await _behaviorRepository.GetBehaviors(spec, cancellationToken);
                var assembledBehaviors = new List<BehaviorDto>();

                behaviors.ToList().ForEach(b =>
                    assembledBehaviors.Add(b.Assemble()));

                return assembledBehaviors;
            }
            catch (Exception ex)
            {
                throw new GetBehaviorException(
                    $"An error occurred when trying to get behavior for IP {ip}. See inner exception for details.",
                    ex);
            }
        }

        public async Task<IEnumerable<BehaviorDto>> GetBehaviorsByPageNameAsync(string pageName, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pageName)) throw new ArgumentNullException(nameof(pageName));

                var spec = new BehaviorByPageNameSpecification(pageName);
                var behaviors = await _behaviorRepository.GetBehaviors(spec, cancellationToken);
                var assembledBehaviors = new List<BehaviorDto>();

                behaviors.ToList().ForEach(b =>
                    assembledBehaviors.Add(b.Assemble()));

                return assembledBehaviors;
            }
            catch (Exception ex)
            {
                throw new GetBehaviorException(
                    $"An error occurred when trying to get behavior for page name {pageName}. See inner exception for details.",
                    ex);
            }
        }

        public async Task<BehaviorDto> GetBehaviorAsync(string ip, string pageName, string userAgent, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ip)) throw new ArgumentNullException(nameof(ip));
                if (string.IsNullOrWhiteSpace(pageName)) throw new ArgumentNullException(nameof(pageName));
                if (string.IsNullOrWhiteSpace(userAgent)) throw new ArgumentNullException(nameof(userAgent));

                var spec = new BehaviorByUniqueSpecification(ip, pageName, userAgent);
                var behavior = await _behaviorRepository.GetBehaviors(spec, cancellationToken);

                if (!behavior.Any()) return null;

                if (behavior.Count() > 1) throw new InvalidOperationException("There is more behavior elements than expected.");

                return behavior.Single().Assemble();
            }
            catch (Exception ex)
            {
                throw new GetBehaviorException(
                    $"An error occurred when trying to get behavior for ip {ip}, page name {pageName} and user agent {userAgent}. See inner exception for details.",
                    ex);
            }
        }

        public async Task CreateBehaviorAsync(CreateBehaviorDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = dto.Assemble();
                var behavior = new Behavior(command);

                // create behavior in MSSQL
                await _behaviorRepository.CreateBehavior(behavior, cancellationToken);

                // dispatch domain event
                var @event = new BehaviorCreatedEvent(behavior.Id,
                                                      behavior.IP,
                                                      behavior.PageName,
                                                      behavior.UserAgent,
                                                      behavior.PageParameters);
                await _messageService.PublishAsync("behavior_created", @event, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new CreateBehaviorException(
                    $"An error occurred when trying to create behavior for IP {dto.IP} and page name {dto.PageName}. See inner exception for details.",
                    ex);
            }
        }
    }
}
