using System;
using System.Security.Claims;

namespace Viato.Api.Auth
{
    public static class IdentityExtensions
    {
        public static long GetUserId(this ClaimsPrincipal principal)
        {
            throw new NotImplementedException();
        }
    }
}
