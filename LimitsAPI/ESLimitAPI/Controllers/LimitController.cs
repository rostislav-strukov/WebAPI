using ESLimitAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ESLimitAPI.Controllers
{
    /// <summary> Controller which have managed ESLimits DB </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LimitController : Controller
    {
        ESLimits db;
        public LimitController(ESLimits context) 
        {
            db = context;
        }

        /// <summary> Get document </summary>
        /// <param name="id">if id empty return all document</param>
        /// <returns></returns>
        [HttpGet("GetDocument")]
        [ProducesResponseType(typeof(Document[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDocument(int? id) 
        {
            var document = await db.GetDocument(id).ToListAsync();
            return Ok(document);
        }

        /// <summary> Get source </summary>
        /// <param name="id">if id empty return all sources</param>
        /// <returns></returns>
        [HttpGet("GetSource")]
        [ProducesResponseType(typeof(Source[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSource(string id)
        {
            var sources = await db.GetSource(id).ToListAsync();
            return Ok(sources);
        }

        /// <summary> Add new source </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost("AddSource")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AddSource([FromBody]Source source)
        {
            await db.AddSource(source);
            return NoContent();
        }

        /// <summary> Update sources </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost("UpdateSource")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateSource([FromBody]Source source)
        {
            await db.UpdateSource(source);
            return NoContent();
        }

        /// <summary> Get information about limit </summary>
        /// <param name="id">if id empty return all limits</param>
        /// <returns></returns>
        [HttpGet("GetLimit")]
        [ProducesResponseType(typeof(Limit[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLimit(int? id)
        {
            var limits = await db.GetLimit(id).ToListAsync();
            return Ok(limits);
        }

        /// <summary> Update limit </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpPost("UpdateLimit")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateLimit([FromBody]LimitView limit)
        {
            await db.UpdateLimit(limit);
            return NoContent();
        }

        /// <summary> Check limit </summary>
        /// <param name="limitType"></param>
        /// <param name="amount"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="whereColumn"></param>
        /// <param name="whereValue"></param>
        /// <returns></returns>
        [HttpGet("CheckLimit")]
        [ProducesResponseType(typeof(CheckLimit[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckLimit(int limitType, decimal amount, string whereColumn, string whereValue, DateTime? dateFrom, DateTime? dateTo) 
        {
            var limits = await db.CheckLimits(limitType, amount, dateFrom, dateTo, whereColumn, whereValue).ToListAsync();
            return Ok(limits);
        }
        /// <summary> Get all limit types </summary>
        /// <returns></returns>
        [HttpGet("GetLimitTypes")]
        [ProducesResponseType(typeof(LimitType[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLimitTypes()
        {
            var limitTypes = await db.GetLimitTypes().ToListAsync();
            return Ok(limitTypes);
        }

        /// <summary> Add new transaction </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        [HttpPost("SetTransaction")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SetTransaction([FromBody]LimitTransaction transaction)
        {
            await db.SetTransaction(transaction);
            return NoContent();
        }

        /// <summary>
        /// Cancel transaction
        /// </summary>
        /// <param name="externalId"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        [HttpPost("CancelTransaction")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CancelTransaction(int externalId, string source) 
        {
            await db.CancelTransaction(externalId, source);
            return NoContent();
        }
    }
}
