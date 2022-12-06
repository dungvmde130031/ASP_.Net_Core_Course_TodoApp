using System;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace TodoApp.WebMvc.PolicyRequirements
{
	public class AdminOnlyRequirement : IAuthorizationRequirement
	{
        public string Role { get; set; }
        public string SpecialName { get; set; }

		public AdminOnlyRequirement(string role, string specialName)
		{
			Role = role;
            SpecialName = specialName;
        }
	}

    public class AdminOnlyRequirementHandler : AuthorizationHandler<AdminOnlyRequirement>
    {
        protected override Task HandleRequirementAsync(
			AuthorizationHandlerContext context,
			AdminOnlyRequirement requirement)
        {
			var currentUserRole = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

			if (currentUserRole == requirement.Role)
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
        }
    }

    public class GenderRequirement : IAuthorizationRequirement
    {
        public string Gender { get; set; }

        public GenderRequirement(string gender)
        {
            Gender = gender;
        }
    }

    public class GenderRequirementHandler : AuthorizationHandler<GenderRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            GenderRequirement requirement)
        {
            var currentUserRole = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Gender)?.Value;

            if (currentUserRole == requirement.Gender)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class PermissionHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var pendingRequirements = context.PendingRequirements.ToList();

            foreach (var item in pendingRequirements)
            {
                if (item is AdminOnlyRequirement)
                {
                    var adminOnlyReq = item as AdminOnlyRequirement;

                    if (adminOnlyReq != null)
                    {
                        var currentUserRole = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

                        if (currentUserRole == adminOnlyReq.Role)
                        {
                            context.Succeed(adminOnlyReq);
                        }
                    }
                }
                else if (item is GenderRequirement)
                {
                    var genderReq = item as GenderRequirement;

                    if (genderReq != null)
                    {
                        var currentUserRole = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Gender)?.Value;

                        if (currentUserRole == genderReq.Gender)
                        {
                            context.Succeed(genderReq);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }

    public class AdminOnlyHandlerOne : AuthorizationHandler<AdminOnlyRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminOnlyRequirement requirement)
        {
            var currentUserRole = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            if (currentUserRole == requirement.Role)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class AdminOnlyHandlerTwo : AuthorizationHandler<AdminOnlyRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AdminOnlyRequirement requirement)
        {
            var currentUserRole = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            if (currentUserRole == requirement.SpecialName)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

