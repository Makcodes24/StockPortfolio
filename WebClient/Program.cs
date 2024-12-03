using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Identity.Client;
using WebClient.ViewModel;

namespace WebClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // <ms_docref_add_msal>
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            IEnumerable<string>? initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ');


            builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd")
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                    .AddDownstreamApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
                    .AddInMemoryTokenCaches();
            // </ms_docref_add_msal>

            // <ms_docref_add_default_controller_for_sign-in-out>
            builder.Services.AddRazorPages().AddMvcOptions(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                              .RequireAuthenticatedUser()
                              .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddMicrosoftIdentityUI();
            // </ms_docref_add_default_controller_for_sign-in-out>

            // <ms_docref_enable_authz_capabilities>
            WebApplication app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            // </ms_docref_enable_authz_capabilities>

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();
            app.MapControllers();

            app.Run();
            //CreateHostBuilder(args).Build().Run();

            builder.Services.AddAuthentication(options =>

            {

                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            })

            .AddCookie()

            .AddOpenIdConnect(options =>
            {

              options.ClientId = builder.Configuration["Authentication:OpenIdConnect:ClientId"];

              options.Authority = builder.Configuration["Authentication:OpenIdConnect:Authority"];

              options.ClientSecret = builder.Configuration["Authentication:OpenIdConnect:ClientSecret"];

              options.ResponseType = builder.Configuration["Authentication:OpenIdConnect:ResponseType"];

              options.SaveTokens = bool.Parse(builder.Configuration["Authentication:OpenIdConnect:SaveTokens"]);

              options.CallbackPath = builder.Configuration["Authentication:OpenIdConnect:CallbackPath"];

              options.Scope.Add(builder.Configuration["Authentication:OpenIdConnect:Scopes:0"]);

              options.Scope.Add(builder.Configuration["Authentication:OpenIdConnect:Scopes:1"]);

              options.Scope.Add(builder.Configuration["Authentication:OpenIdConnect:Scopes:2"]);


              options.TokenValidationParameters = new TokenValidationParameters

              {

                  NameClaimType = "name",

                  RoleClaimType = "roles"

              };

              options.Events = new OpenIdConnectEvents
              {

                  OnTokenResponseReceived = async context =>
                  {

                      var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();

                      var confidentialClientApp = ConfidentialClientApplicationBuilder

                          .Create(options.ClientId)

                          .WithClientSecret(options.ClientSecret)

                          .WithAuthority(new Uri(options.Authority))

                          .Build();

                      var userAccessTokenResult = await confidentialClientApp.AcquireTokenOnBehalfOf(

                          new[] { builder.Configuration["Authentication:OpenIdConnect:Scopes:3"] },

                          new UserAssertion(context.TokenEndpointResponse.AccessToken)

                      ).ExecuteAsync();

                      tokenService.AccessToken = userAccessTokenResult.AccessToken;

                      var graphAccessTokenResult = await confidentialClientApp.AcquireTokenOnBehalfOf(

                          new[] { builder.Configuration["Authentication:OpenIdConnect:Scopes:4"] },

                          new UserAssertion(context.TokenEndpointResponse.AccessToken)

                      ).ExecuteAsync();

                      tokenService.AccessToken2 = graphAccessTokenResult.AccessToken;

                  }

              };

                });
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

    }
}
