using System;
using System.Linq;
using System.Security.Claims;

namespace Viato.Api.Auth
{
    public static class IdentityExtensions
    {
        public static long GetUserId(this ClaimsPrincipal principal)
        {
            var subject = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (subject == null)
            {
                throw new Exception("It's impossible that authenticated user don't have name identifier claim.");
            }

            if (!long.TryParse(subject.Value, out long userId))
            {
                throw new Exception("It's impossible that name identifier is not long.");
            }

            return userId;
        }
    }
}
