using Microsoft.AspNetCore.Mvc;
using RiskManagementAPI.Dtos;
using RiskManagementAPI.Services;

namespace RiskManagementAPI.Controllers;

[ApiController]
[Route("risks/{riskId}/assessments")]
public class AssessmentsController : ControllerBase
{
    private readonly AssessmentService _assessmentService;
    
    public AssessmentsController(AssessmentService assessmentService)
    {
        _assessmentService = assessmentService;
    }
    
    // GET /risks/{riskId}/assessments
    [HttpGet]
    public async Task<IActionResult> GetAssessments(int riskId)
    {
        try
        {
            var assessments = await _assessmentService.GetAssessmentsByRiskIdAsync(riskId);
            return Ok(assessments);
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
    
    // GET /risks/{riskId}/assessments/latest
    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestAssessment(int riskId)
    {
        try
        {
            var assessment = await _assessmentService.GetLatestAssessmentAsync(riskId);
            if (assessment == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "No assessments found",
                    Status = 404,
                    Detail = $"No assessments found for risk {riskId}"
                });
            }
            return Ok(assessment);
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
    
    // POST /risks/{riskId}/assessments
    [HttpPost]
    public async Task<IActionResult> CreateAssessment(int riskId, [FromBody] CreateAssessmentDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var assessment = await _assessmentService.CreateAssessmentAsync(riskId, createDto);
            
            return CreatedAtAction(nameof(GetLatestAssessment), 
                new { riskId = riskId }, 
                assessment);
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
                Title = "Invalid input",
                Status = 400,
                Detail = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            // Check if it's a duplicate or status issue
            if (ex.Message.Contains("already exists"))
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Duplicate assessment",
                    Status = 409,
                    Detail = ex.Message
                });
            }
            
            return UnprocessableEntity(new ProblemDetails
            {
                Title = "Cannot create assessment",
                Status = 422,
                Detail = ex.Message
            });
        }
    }
}