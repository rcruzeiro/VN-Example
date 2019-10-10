using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VN.Example.Platform.Application.BehaviorService;
using VN.Example.Platform.Application.BehaviorService.DTOs;

namespace VN.Example.Host.Web.Controllers.V1
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/v1.0/[controller]")]
    public class BehaviorController : Controller
    {
        private readonly IBehaviorAppService _behaviorAppService;

        public BehaviorController([FromServices]IBehaviorAppService behaviorAppService)
        {
            _behaviorAppService = behaviorAppService ?? throw new ArgumentNullException(nameof(behaviorAppService));
        }

        /// <summary>
        /// Gets the Behavior by IP.
        /// </summary>
        /// <param name="ip">Client IP.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Behavior data.</returns>
        /// <response code="200">Behaviors for the informed IP was found.</response>
        /// <response code="400">Query parameter is at an invalid state.</response>
        /// <response code="404">No behavior for the informed IP was found.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [ProducesResponseType(typeof(IEnumerable<BehaviorDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("ip/{ip}")]
        public async Task<IActionResult> GetBehaviorByIPAsync([FromRoute]string ip, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ip)) return BadRequest();

                var result = await _behaviorAppService.GetBehaviorsByIPAsync(ip, cancellationToken);

                if (!result.Any()) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Gets the Behavior by Page Name.
        /// </summary>
        /// <param name="pageName">Host Page Name.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Behavior data.</returns>
        /// <response code="200">Behaviors for the informed Page Name was found.</response>
        /// <response code="400">Query parameter is at an invalid state.</response>
        /// <response code="404">No behavior for the informed Page Name was found.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [ProducesResponseType(typeof(IEnumerable<BehaviorDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("page/{pageName}")]
        public async Task<IActionResult> GetBehaviorByPageNameAsync([FromRoute]string pageName, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(pageName)) return BadRequest();

                var result = await _behaviorAppService.GetBehaviorsByPageNameAsync(pageName, cancellationToken);

                if (!result.Any()) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get a specific Behavior.
        /// </summary>
        /// <param name="ip">Client IP.</param>
        /// <param name="pageName">Host Page Name.</param>
        /// <param name="userAgent">Client User Agent.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Behavior data.</returns>
        /// <response code="200">Behavior for the informed parameters was found.</response>
        /// <response code="400">Query parameter is at an invalid state.</response>
        /// <response code="404">No behavior for the informed parameters was found.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [ProducesResponseType(typeof(BehaviorDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpGet("ip/{ip}/page/{pageName}/agent/{userAgent}")]
        public async Task<IActionResult> GetBehavior([FromRoute]string ip,
                                                     [FromRoute]string pageName,
                                                     [FromRoute]string userAgent,
                                                     CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ip) ||
                    string.IsNullOrWhiteSpace(pageName) ||
                    string.IsNullOrWhiteSpace(userAgent))
                    return BadRequest();

                var result = await _behaviorAppService.GetBehaviorAsync(ip, pageName, userAgent, cancellationToken);

                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Create a new Behavior.
        /// </summary>
        /// <param name="request">Request parameters.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Redirects to the home page.</returns>
        /// <response code="201">Behavior created with succees.</response>
        /// <response code="500">Internal Server Error. See response message for details.</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [HttpPost]
        public async Task<IActionResult> CreateBehaviorAsync([FromBody]CreateBehaviorDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                await _behaviorAppService.DispatchBehavior(request, cancellationToken);

                return Created("", null); // 201
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
