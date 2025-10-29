using Microsoft.AspNetCore.Mvc;
using SalesOrderSystem_BackEnd.Models;
using SalesOrderSystem_BackEnd.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesOrderSystem_BackEnd.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesRequestLineController : ControllerBase
    {
        private readonly ISalesRequestLineService _service;

        public SalesRequestLineController(ISalesRequestLineService service)
        {
            _service = service;
        }

        // Create a new line
        [HttpPost]
        public async Task<IActionResult> CreateLine([FromBody] SalesRequestLineModel line)
        {
            var result = await _service.CreateLineAsync(line);
            return StatusCode((int)result.StatusCode, result);
        }

        // Get all lines of a request
        [HttpGet("request/{requestId}")]
        public async Task<IActionResult> GetLinesByRequest(int requestId)
        {
            var result = await _service.GetLinesByRequestAsync(requestId);
            return StatusCode((int)result.StatusCode, result);
        }

        // Get line by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLine(int id)
        {
            var result = await _service.GetLineAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        // Update line
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLine(int id, [FromBody] SalesRequestLineModel line)
        {
            var result = await _service.UpdateLineAsync(id, line);
            return StatusCode((int)result.StatusCode, result);
        }

        // Delete line (or cancel)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLine(int id)
        {
            var result = await _service.DeleteLineAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
