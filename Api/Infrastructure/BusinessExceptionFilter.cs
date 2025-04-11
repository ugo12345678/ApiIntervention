using Business.Abstraction.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Principal;

namespace Api.Infrastructure
{
    public class BusinessExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        ///     Logger
        /// </summary>
        private readonly ILogger<BusinessExceptionFilter> _logger;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        public BusinessExceptionFilter(ILogger<BusinessExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Raises the exception event. Log exception and set InternalServerError (500) response code status with exception message as content
        /// </summary>
        /// <param name="context">The context for the action.</param>
        public override void OnException(ExceptionContext context)
        {
            // BadRequest result (400)
            if (context.Exception is BusinessException businessException)
            {
                _logger.LogWarning(businessException, "{message} - {@businessErrors}", businessException.Message, businessException.BusinessErrors);

                // Check if the exception occurs for a call on the Bo API, or on the Pda/Public API.
                // This differentiation is needed to return a different error body for BadRequest depending on the API: 
                //  - On the internal API (Bo), we should return the content of our exceptions (an enumerable of BusinessError objects)
                //  - On external APIs (Pda and Publics), we should return a standardized and siplified error
                bool isBoApi = context.ActionDescriptor is ControllerActionDescriptor controller && (controller.AttributeRouteInfo?.Template?.StartsWith("bo") ?? false);

                if (isBoApi)
                {
                    context.Result = new BadRequestObjectResult(businessException.BusinessErrors);
                }
                else
                {
                    ProblemDetails problemDetail = new()
                    {
                        Title = HttpStatusCode.BadRequest.ToString(),
                        Status = (int)HttpStatusCode.BadRequest,
                        Detail = businessException.Message
                    };

                    problemDetail.Extensions.Add("errors", businessException.BusinessErrors.Select(e => new { Code = e.Code.ToString(), e.Message }));
                    context.Result = new BadRequestObjectResult(problemDetail);
                }
            }
            // NotFound result (404)
            else if (context.Exception is ResourceNotFoundException resourceNotFoundException)
            {
                _logger.LogWarning(resourceNotFoundException, resourceNotFoundException.Message);

                ProblemDetails problemDetail = new()
                {
                    Title = HttpStatusCode.NotFound.ToString(),
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = resourceNotFoundException.Message
                };

                context.Result = new NotFoundObjectResult(problemDetail);
            }
            // InternalServerError result (500)
            else
            {
                // Gets action name from ActionDescriptor
                ActionDescriptor actionDescriptor = context.ActionDescriptor;
                string actionName = actionDescriptor.DisplayName;

                // Gets controller name from ControllerActionDescriptor
                ControllerActionDescriptor controllerActionDescriptor = actionDescriptor as ControllerActionDescriptor;
                string controllerName = controllerActionDescriptor?.ControllerName;

                IList<string> messages = new List<string>
                {
                    "ControllerName: " + controllerName,
                    "Action:" + actionName
                };

                // Gets user identity
                IIdentity identity = context.HttpContext?.User?.Identity;
                bool isAuthenticated = identity?.IsAuthenticated ?? false;

                if (isAuthenticated && identity is not null)
                {
                    messages.Add($"Authenticated user:{identity.Name}");
                }

                // Adds message and stacktrace of the exception raised
                messages.Add($"Exception: {context.Exception.Message}");
                messages.Add(context.Exception.StackTrace);

                // Adds details about inner exception if it is not null
                Exception exc = context.Exception.InnerException;
                while (exc != null)
                {
                    messages.Add($"--- Inner Exception: {exc.Message}");
                    messages.Add(exc.StackTrace);
                    exc = exc.InnerException;
                }

                _logger.LogError(string.Join(Environment.NewLine, messages));

                context.Result = new ObjectResult(context.Exception.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}

