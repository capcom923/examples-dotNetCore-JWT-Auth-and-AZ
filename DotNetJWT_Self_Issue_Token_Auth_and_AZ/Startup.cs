using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetJWT_Self_Issue_Token_Auth_and_AZ.AuthEvents;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DotNetJWT_Self_Issue_Token_Auth_and_AZ
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            string pem = Configuration["InternalToken:RsaPublicKeyInPem"];
            var pubKey = RsaUtils.CreateRsaPublicKeyByPem(pem);

            services.AddScoped<MyAuthenticateEvents>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomPolicyRequirement", policy => policy.Requirements.Add(new MinAgePolicyRequirement(18)));
                options.AddPolicy("Claim1Policy", policy => policy
                       .RequireAssertion(context =>
                                context.User.HasClaim(c => (c.Type == "Claim1"))
                                )
                   );
                options.AddPolicy("Role1Policy", policy => policy.RequireRole("role1")
                   );
                options.AddPolicy("IsInRolePolicy", policy => policy
                       .RequireAssertion(context =>
                                context.User.IsInRole("role2")
                                )
                   );
                options.AddPolicy("DynamicClaimPolicy", policy => policy
                       .RequireAssertion(context =>
                                context.User.HasClaim(c => (c.Type == "ClaimAddDynamically"))
                                )
                   );
            })
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("MyBearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = pubKey
                };
                options.EventsType = typeof(MyAuthenticateEvents);
            });
            services.AddSingleton<IAuthorizationHandler, MinAgeHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
