using Microsoft.AspNetCore.Authorization;
using RecipeProject.Entity;
using RecipeProject.UnitOfWork;
using System.Security.Claims;

namespace RecipeProject.Policy
{
    public class RequireActiveUserHandler : AuthorizationHandler<ActiveUserPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUserPolicy requirement)
        {
            /*
            var recipeClaim = context.User.FindFirst(c => c.Type == ClaimTypes.Deleted);
            if (recipeClaim is null)
            {
                return Task.CompletedTask;
            }
            
            var user = unitOfWork.Context()
                              .Set<Recipe>()
                              .IgnoreQueryFilters()
                              .Where(r => r.Id == recipeClaim.UserId)
                              .FirstOrDefault();
            if (!user.Deleted)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
            */
            return Task.CompletedTask;
        }
    }
}
