using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FinanceApplication.API.Extensions;
using FinanceApplication.Business.Constants;
using FinanceApplication.Business.DependencyResolvers.Autofac;
using FinanceApplication.Core.Result;
using FinanceApplication.Core.Security;
using FinanceApplication.Dal.Concrete.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new AutofacBusinessModule());
    });

builder.Services.AddControllers();

builder.Services.AddDbContext<FinanceAppDbContext>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "FinanceApplication API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Cookie,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
var key = Encoding.ASCII.GetBytes(tokenOptions?.SecurityKey);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
                //JWT settings.
                .AddJwtBearer(x =>
                {
                    //Accept only SSL or HTTPS?
                    x.RequireHttpsMetadata = false;
                    //Save to db if approved
                    x.SaveToken = false;
                    //Defines whats going to be controlled
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Sign check
                        ValidateIssuerSigningKey = true,
                        //Control it with
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        //Don't validate
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var endpoint = context.HttpContext.GetEndpoint();
                            var allowAnonymous = endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null;

                            if (allowAnonymous)
                            {
                                return Task.CompletedTask;  // Skip further validation
                            }

                            context.Request.Headers.TryGetValue("Authorization", out var token);
                            if (string.IsNullOrEmpty(token))
                            {
                                var result = new ErrorDataResult<bool>(false, Messages.TokenError, StatusCodes.Status401Unauthorized);
                                return Task.FromResult(result);
                            }
                            context.Token = token.ToString().Split(" ")[1];
                            var tokenResult = ServiceTool.ServiceProvider.GetService<ITokenHelper>().GetTokenInfo(context.Token);
                            if (!tokenResult.Success)
                            {
                                return Task.FromResult(tokenResult);
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = async context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(context.Exception.Message);
                            await context.Response.CompleteAsync();
                        }
                    };
                });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHealthChecks();

var app = builder.Build();

ServiceTool.ServiceProvider = app.Services;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json", "FinanceApplication.API v1");
    });
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
