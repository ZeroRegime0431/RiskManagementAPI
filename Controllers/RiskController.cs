using Microsoft.AspNetCore.Mvc;
using RiskManagementAPI.Dtos;
using RiskManagementAPI.Services;

namespace RiskManagementAPI.Controllers;

[ApiController]
[Route("risks")]
public class RiskController : ControllerBase
{
    private readonly RiskService _service;
    
    public RiskController(RiskService service)
    {
        _service = service;
    }
    
    // GET /risks
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var risks = await _service.GetAllRisksAsync();
        return Ok(risks);
    }
    
    // GET /risks/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var risk = await _service.GetRiskByIdAsync(id);
        if (risk == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Risk not found",
                Status = 404,
                Detail = $"Risk with ID {id} was not found"
            });
        }
        return Ok(risk);
    }
    
    // POST /risks
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRiskDto createDto)
    {
        try
        {
            // Validate ModelState (for Data Annotations)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var risk = await _service.CreateRiskAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = risk.Id }, risk);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid category",
                Status = 400,
                Detail = ex.Message
            });
        }
    }
    
    // PUT /risks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRiskDto updateDto)
    {
        var risk = await _service.UpdateRiskAsync(id, updateDto);
        if (risk == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Risk not found",
                Status = 404,
                Detail = $"Risk with ID {id} was not found"
            });
        }
        return Ok(risk);
    }
    
    // DELETE /risks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteRiskAsync(id);
            if (!deleted)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Risk not found",
                    Status = 404,
                    Detail = $"Risk with ID {id} was not found"
                });
            }
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new ProblemDetails
            {
                Title = "Cannot delete risk",
                Status = 422,
                Detail = ex.Message
            });
        }
    }
    
    // PATCH /risks/{id}/status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto statusDto)
    {
        try
        {
            var updated = await _service.UpdateRiskStatusAsync(id, statusDto.Status);
            if (!updated)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Risk not found",
                    Status = 404,
                    Detail = $"Risk with ID {id} was not found"
                });
            }
            
            return Ok(new { message = $"Status updated to {statusDto.Status}" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid status",
                Status = 400,
                Detail = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Invalid status transition",
                Status = 409,
                Detail = ex.Message
            });
        }
    }


    // GET /risks/{id}/score-summary
    [HttpGet("{id}/score-summary")]
    public async Task<IActionResult> GetScoreSummary(int id)
    {
        var summary = await _service.GetScoreSummaryAsync(id);
        if (summary == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Risk not found",
                Status = 404,
                Detail = $"Risk with ID {id} was not found"
            });
        }
        return Ok(summary);
    }
}