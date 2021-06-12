using Domain.Exceptions;
using System;
using System.Security.Claims;

namespace Api.Authorization
{
    public static class Authorize
    {
        public static void TokenAgainstResource(ClaimsPrincipal tokenClaims, string requestedUserId)
        {
            if (tokenClaims.FindFirst(ClaimTypes.NameIdentifier).Value != requestedUserId)
            {
                throw new NotAuthorizedException("Token not authorized to access resource");
            }
        }
    }
}