using Ecoeden.Catalogue.Api.Services;
using Ecoeden.Catalogue.Application.Contracts.Security;
using Ecoeden.Catalogue.Application.Extensions;
using Ecoeden.Catalogue.Domain.Models.Constants;
using Ecoeden.Catalogue.Domain.Models.Core;
using Ecoeden.Catalogue.Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecoeden.Catalogue.Api.Filters;

public sealed class RequirePermissionAttribute : TypeFilterAttribute
{
    public RequirePermissionAttribute(ApiAccess requiredPermission) : base(typeof(RequirePermissionExecutor))
    {
        Arguments = [requiredPermission];
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequirePermissionExecutor(ApiAccess role,
        IIdentityService identityService,
        ILogger logger,
        IPermissionMapper permissionMapper) : Attribute, IActionFilter
    {
        private readonly IIdentityService _identityService = identityService;
        private readonly ILogger _logger = logger;
        private readonly ApiAccess _requiredRole = role;
        private readonly IPermissionMapper _permissionMapper = permissionMapper;

        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.Here().MethodEntered();
            var currentUser = _identityService.PrepareUser();
            List<string> requiredPermissions = [.. _permissionMapper.GetPermissionsForRole(_requiredRole)];

            var commonPermissions = requiredPermissions.Intersect(currentUser.AuthorizationDto.Permissions).ToList();

            if (commonPermissions.Count == 0)
            {
                _logger.Here().Error("No matching permission found");
                context.Result = new UnauthorizedObjectResult(new ApiError
                {
                    Code = ErrorCodes.Unauthorized.ToString(),
                    Message = "Access denied"
                });
            }
            _logger.Here().MethodExited();
        }
    }
}
