using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetJWT_Manually_Validate_Token_Auth
{
    public static class TokenValidator
    {
        public static bool ValidateToken(string authority, string issuer, string authToken)
        {
            authToken = TrimBearer(authToken);
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters(authority, issuer);

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return true;
        }

        private static string TrimBearer(string jwt)
        {
            var tokenType = "bearer ";
            string pureToken = jwt;
            if (jwt.StartsWith(tokenType, StringComparison.InvariantCultureIgnoreCase))
            {
                pureToken = jwt.Substring(tokenType.Length);
            }
            return pureToken;
        }

        private static TokenValidationParameters GetValidationParameters(string authority, string issuer)
        {
            var keyResolver = new OpenIdConnectSigningKeyResolver(authority);
            return new TokenValidationParameters()
            {
                AuthenticationType = "Bearer",
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidateIssuer = true,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                IssuerSigningKeyResolver = (token, securityToken, kid, parameters) => keyResolver.GetSigningKey(kid)
            };
        }

        private class OpenIdConnectSigningKeyResolver
        {
            private readonly OpenIdConnectConfiguration openIdConfig;

            public OpenIdConnectSigningKeyResolver(string authority)
            {
                var cm = new ConfigurationManager<OpenIdConnectConfiguration>($"{authority.TrimEnd('/')}/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());
                openIdConfig = AsyncHelper.RunSync(async () => await cm.GetConfigurationAsync());
            }

            public SecurityKey[] GetSigningKey(string kid)
            {
                // Find the security token which matches the identifier
                return new[] { openIdConfig.JsonWebKeySet.GetSigningKeys().FirstOrDefault(t => t.KeyId == kid) };
            }
        }

        private static class AsyncHelper
        {
            private static readonly TaskFactory TaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

            public static void RunSync(Func<Task> func)
            {
                TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
            }

            public static TResult RunSync<TResult>(Func<Task<TResult>> func)
            {
                return TaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
            }
        }

        private static TokenValidationParameters GetValidationParameters2()
        {
            return new TokenValidationParameters()
            {
                AuthenticationType = "Bearer",
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("A-Security-Key-at-least-16-bits")) // The same key as the one that generate the token
            };
        }
    }
}
