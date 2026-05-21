using Microsoft.AspNetCore.Mvc;
using RiskManagementAPI.Dtos;
using RiskManagementAPI.Services;

namespace RiskManagementAPI.Controllers;

[ApiController]
[Route("risks/{riskId}/controls")]
public class ControlsController : ControllerBase
{
    private readonly ControlService _controlService;
    
    public ControlsController(ControlService controlService)
    {
        _controlService = controlService;
    }
    
    // GET /risks/{riskId}/controls
    [HttpGet]
    public async Task<IActionResult> GetControls(int riskId)
    {
        try
        {
            var controls = await _controlService.GetControlsByRiskIdAsync(riskId);
            return Ok(controls);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Risk not found",
                Status = 404,
                Detail = ex.Message
            });
        }
    }
    
    // POST /risks/{riskId}/controls
    [HttpPost]
    public async Task<IActionResult> CreateControl(int riskId, [FromBody] CreateControlDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var control = await _controlService.CreateControlAsync(riskId, createDto);
            
            // Return 201 with location header
            return CreatedAtAction(
                nameof(GetControls), 
                new { riskId = riskId }, 
                control);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Risk not found",
                Status = 404,
                Detail = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid control type",
                Status = 400,
                Detail = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new ProblemDetails
            {
                Title = "Cannot add control",
                Status = 422,
                Detail = ex.Message
            });
        }
    }
    
    // PUT /risks/{riskId}/controls/{controlId}
    [HttpPut("{controlId}")]
    public async Task<IActionResult> UpdateControl(
        int riskId, 
        int controlId, 
        [FromBody] UpdateControlDto updateDto)
    {
        try
        {
            var control = await _controlService.UpdateControlAsync(riskId, controlId, updateDto);
            return Ok(control);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Control not found",
                Status = 404,
                Detail = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new ProblemDetails
            {
                Title = "Cannot update control",
                Status = 422,
                Detail = ex.Message
            });
        }
    }
    
    // DELETE /risks/{riskId}/controls/{controlId}
    [HttpDelete("{controlId}")]
    public async Task<IActionResult> DeleteControl(int riskId, int controlId)
    {
        try
        {
            var deleted = await _controlService.DeleteControlAsync(riskId, controlId);
            if (!deleted)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Control not found",
                    Status = 404,
                    Detail = $"Control with ID {controlId} not found for risk {riskId}"
                });
            }
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new ProblemDetails
            {
                Title = "Cannot delete control",
                Status = 422,
                Detail = ex.Message
            });
        }
    }
}