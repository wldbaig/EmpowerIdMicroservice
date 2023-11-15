using EmpowerIdMicroservice.Infrastructure;
using EmpowerIdMicroservice.WebApi.Grpc.Post;
using EmpowerIdMicroservice.WebApi.Proto;
using Microsoft.EntityFrameworkCore;
using Autofac;
using EmpowerIdMicroservice.Application.AutofacModules;
using Autofac.Extensions.DependencyInjection;
using EmpowerIdMicroservice.WebApi.Grpc.Comment;
using AspNetCoreRateLimit;
using EmpowerIdMicroservice.Domain.AggregateModules.ApplicationUserAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using EmpowerIdMicroservice.WebApi.Filters;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        var postGrpcServiceUri = builder.Configuration["GrpcServices:PostGrpcServiceUri"];
        var commentGrpcServiceUri = builder.Configuration["GrpcServices:CommentGrpcServiceUri"];

        // Add services to the container.
        builder.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
        });

        builder.Services.AddControllers(); 
        builder.Services.AddEndpointsApiExplorer();
         

        builder.Services.AddGrpcClient<PostGrpc.PostGrpcClient>(o => o.Address = new Uri(postGrpcServiceUri));
        builder.Services.AddGrpcClient<CommentGrpc.CommentGrpcClient>(o => o.Address = new Uri(commentGrpcServiceUri));

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(x =>
    {
        x.IncludeErrorDetails = true;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes("16e3853c54f14b66a7fb19e9c962a1b3")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    });

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "POS API",
                Description = "An ASP.NET Core Web API for POS"
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });

        builder.Services.Configure<IpRateLimitOptions>(builder =>
        {
            builder.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 50, // Adjust the limit based on your requirements
                    Period = "1m" // Adjust the period based on your requirements (e.g., 1 minute)
                }
            };
        });

        // Register rate limiting services
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>(); // Add this line


        var containerbd = new ContainerBuilder(); 
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MediatorModule()));
        builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new ApplicationModule(connectionString)));

        var app = builder.Build(); 
          
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
       
        app.UseRouting(); // Add this line to configure routing
        app.UseAuthentication();
        app.UseAuthorization();
        // Enable rate limiting middleware
        app.UseIpRateLimiting();

        app.MapControllers();
        // Add a route for gRPC service
        app.MapGrpcService<GrpcPostService>();
        app.MapGrpcService<GrpcCommentService>();
        

        app.Run();
    }
}