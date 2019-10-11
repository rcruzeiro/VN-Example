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
        private readonly BehaviorRepositoryResolver _behaviorRepository;
        private readonly IMessageService _messageService;

        public BehaviorAppService(BehaviorRepositoryResolver behaviorRepository, IMessageService messageService)
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
                var behaviors = await _behaviorRepository("MSSQL").GetBehaviors(spec, cancellationToken);
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
                var behaviors = await _behaviorRepository("MSSQL").GetBehaviors(spec, cancellationToken);
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

        public async Task<BehaviorDto> GetBehaviorAsync(string ip, string pageName, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ip)) throw new ArgumentNullException(nameof(ip));
                if (string.IsNullOrWhiteSpace(pageName)) throw new ArgumentNullException(nameof(pageName));

                var spec = new BehaviorByIpAndPageNameSpecification(ip, pageName);
                var behavior = await _behaviorRepository("MSSQL").GetBehaviors(spec, cancellationToken);

                if (!behavior.Any()) return null;

                return behavior.Last().Assemble();
            }
            catch (Exception ex)
            {
                throw new GetBehaviorException(
                    $"An error occurred when trying to get behavior for ip {ip}, page name {pageName}. See inner exception for details.",
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
                await _behaviorRepository("MSSQL").CreateBehavior(behavior, cancellationToken);

                // create behavior in Couchbase
                await _behaviorRepository("Couch").CreateBehavior(behavior, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new CreateBehaviorException(
                    $"An error occurred when trying to create behavior for IP {dto.IP} and page name {dto.PageName}. See inner exception for details.",
                    ex);
            }
        }

        public async Task DispatchBehavior(CreateBehaviorDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = dto.Assemble();
                var behavior = new Behavior(command);

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
