using Marten;
using MartenEventSourcing.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MartenEventSourcing.API.Controllers
{
    [ApiController]
    public class PoliciesController : ControllerBase
    {
        private readonly IDocumentStore _store;
        private readonly ILogger<PoliciesController> _logger;

        public PoliciesController(IDocumentStore store, ILogger<PoliciesController> logger)
        {
            _store = store;
            _logger = logger;
        }

        [HttpPost]
        [Route("api/v{version:apiVersion}/[controller]")]
        [ProducesResponseType(typeof(Events.PolicyCreated), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> CreatePolicy([FromBody] CreatePolicyRequest request)
        {
            var policy = new Events.PolicyCreated
            {
                Plates = request.Plates,
                Insured = request.Insured
            };
            try
            {
                
                await using var session = _store.LightweightSession();
                session.Events.StartStream<Policy>(policy.Id, policy);
                await session.SaveChangesAsync();
                return Ok(policy);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/v{version:apiVersion}/[controller]/{policyId}")]
        public async Task<IActionResult> GetPolicy(Guid policyId)
        {

            await using var session = _store.QuerySession();
            var policy = await session.LoadAsync<Policy>(policyId); //from projection
            //var policy = await session.Events.AggregateStreamAsync<Policy>(policyId); //from event
            if (policy == null)
            {
                return NotFound();
            }
            return Ok(policy);
        }

        [HttpGet]
        [Route("api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> GetPolicies()
        {

            await using var session = _store.QuerySession();
            var policies = await session.Query<Policy>().ToListAsync();
            if (policies.Count == 0 )
            {
                return NotFound();
            }
            return Ok(policies);


        }

        [HttpPut]
        [Route("api/v{version:apiVersion}/[controller]/{policyId}/plates")]
        [ProducesResponseType(typeof(Events.PolicyPlatesUpdated), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> UpdatePlates([FromBody] UpdatePlatesRequest request, Guid policyId)
        {
            var platesUpdated = new Events.PolicyPlatesUpdated
            {
                Id = policyId,
                Plates = request.Plates
            };
            try
            {

                await using var session = _store.LightweightSession();
                session.Events.Append(policyId, platesUpdated);
                await session.SaveChangesAsync();
                return Ok(platesUpdated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("api/v{version:apiVersion}/[controller]/{policyId}/renew")]
        [ProducesResponseType(typeof(Events.PolicyRenewed), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> RenewPolicy(Guid policyId)
        {
            var policyRenew = new Events.PolicyRenewed
            {
                Id = policyId,
                RenewedAtUtc = DateTime.UtcNow
            };
            try
            {
                await using var session = _store.LightweightSession();
                session.Events.Append(policyId, policyRenew);
                await session.SaveChangesAsync();
                return Ok(policyRenew);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("api/v{version:apiVersion}/[controller]/{policyId}/cancel")]
        [ProducesResponseType(typeof(Events.PolicyCanceled), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> CancelPolicy(Guid policyId)
        {
            var policyCancel = new Events.PolicyCanceled
            {
                Id = policyId,
                CanceledAtUtc = DateTime.UtcNow
            };
            try
            {
                await using var session = _store.LightweightSession();
                session.Events.Append(policyId, policyCancel);
                await session.SaveChangesAsync();
                return Ok(policyCancel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
