using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.Simplification;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Security.Claims;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Security.Cryptography;
using System.Threading;

namespace NaturalFirstWebApp.Models
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Check user role
            if (!string.IsNullOrEmpty(Roles) && !context.HttpContext.User.IsInRole(Roles))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
