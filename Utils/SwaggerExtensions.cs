﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace aspnet_logger_backend.Utils;

public static class SwaggerExtensions
{
    public static void ConfigureSwaggerBuilder(WebApplicationBuilder builder, ConfigurationManager cfgmgr)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(cfgmgr["api:version"], new OpenApiInfo { Title = cfgmgr["api:name"], Version = cfgmgr["api:version"] });
            c.AddOauth2AuthSchemaSecurityDefinitions(cfgmgr);
        }).AddSwaggerGenNewtonsoftSupport();
    }
    public static void ConfigureSwaggerApp(WebApplication app, ConfigurationManager cfgmgr)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
            c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/" + cfgmgr["api:version"] + "/swagger.json", cfgmgr["api:name"] + " " + cfgmgr["api:version"]);

            //oauth2

            c.OAuthClientId(cfgmgr["auth:clientid"]);
            c.OAuthUsePkce();
            c.OAuthAppName(cfgmgr["api:name"]);
            c.OAuthScopeSeparator(" ");
            c.OAuthUseBasicAuthenticationWithAccessCodeGrant();

        });

    }

    public static SwaggerGenOptions AddOauth2AuthSchemaSecurityDefinitions(this SwaggerGenOptions options, ConfigurationManager cfgmgr)
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Description = "OAuth2.0 Auth Code with PKCE",
            Name = "oauth2",
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows()
            {
                AuthorizationCode = new OpenApiOAuthFlow()
                {
                    AuthorizationUrl = new Uri(cfgmgr["auth:authorizeurl"]),
                    TokenUrl = new Uri(cfgmgr["auth:tokenurl"]),
                    Scopes = new Dictionary<string, string>
                        {
                             { "openid", "Use Openid Connect" }
                        }
                }
            }
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                    Enter 'Bearer' [space] and then your token in the text input below.
                    \r\n\r\nExample: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        },
                        Scheme = "oauth2",
                        Name = "oauth2",
                        In = ParameterLocation.Header
                    },
                    new List < string > ()
                }
            });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                                                {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });

        return options;
    }
}