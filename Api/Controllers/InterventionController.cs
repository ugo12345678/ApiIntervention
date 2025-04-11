using Business.Abstraction.Exceptions;
using Business.Abstraction.Manager;
using Business.Abstraction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;

namespace Api.Controllers
{
    /// <summary>
    /// Public API Controller for clients processes
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public partial class InterventionController : Controller
    {
        private readonly IInterventionManager _interventionManager;

        public InterventionController(IInterventionManager interventionManager)
        {
            _interventionManager = interventionManager;
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Technician,Client")]
        [SwaggerOperation(OperationId = "SearchInterventions")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<InterventionModel>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Incorrect pagination data", Type = typeof(BusinessError[]))]
        public async Task<IActionResult> SearchInterventionsAsync()
        {
            List<InterventionModel> listInterventions = await _interventionManager.SearchAsync(this.User.IsInRole("Admin"), this.User.Identity!.Name!);
            return Ok(listInterventions);
        }


        [HttpGet("{interventionId}")]
        [Authorize(Roles = "Admin,Technician,Client")]
        [SwaggerOperation(OperationId = "GetInterventionById")]
        [SwaggerResponse(StatusCodes.Status200OK, "The requested intervention.", typeof(GetInterventionModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Unknown intervention id.")]
        public async Task<IActionResult> GetArticleByIdAsync(long interventionId)
        {
            GetInterventionModel intervention = await _interventionManager.GetByIdAsync(interventionId);
            return Ok(intervention);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(OperationId = "CreateIntervention")]
        [SwaggerResponse(StatusCodes.Status201Created, "The id of the Intervention", typeof(long))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Incorrect data in supplied model", Type = typeof(BusinessError[]))]
        public async Task<IActionResult> CreateInterventionAsync(InterventionModel intervention)
        {
            long interventionId = await _interventionManager.CreateAsync(intervention,this.User.Identity!.Name!);
            return Ok(interventionId);
        }


        [HttpPut("{interventionId}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(OperationId = "UpdateIntervention")]
        [SwaggerResponse(StatusCodes.Status201Created, "Intervention updated successfully.", typeof(long))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid data provided.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Unknown intervention id.")]
        public async Task<IActionResult> UpdateInterventionAsync(long interventionId, InterventionModel intervention)
        {
            long updatedinterventionId  = await _interventionManager.UpdateAsync(interventionId, intervention, this.User.Identity!.Name!);
            return Ok(updatedinterventionId);
        }


        [HttpDelete("{interventionId}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(OperationId = "DeleteIntervention")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Intervention delete successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Description = "Invalid data provided.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, Description = "Unknown intervention id.")]
        public async Task<IActionResult> DeleteInterventionAsync(long interventionId)
        {
            await _interventionManager.DeleteAsync(interventionId);
            return NoContent();
        }
    }
}
