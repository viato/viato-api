﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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