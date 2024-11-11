//using Ecommerse_shoes_backend.Dbcontext;
//using Ecommerse_shoes_backend.Services.CartService;
//using Ecommerse_shoes_backend.Services.Productservice;
//using Ecommerse_shoes_backend.Services.Userservice;
//using Ecommerse_shoes_backend.Services.WishListService;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;

//namespace Ecommerse_shoes_backend
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add AutoMapper
//            builder.Services.AddAutoMapper(typeof(Program));

//            // Configure DbContext
//            builder.Services.AddDbContext<ApplicationContext>(options =>
//                options.UseSqlServer(builder.Configuration.GetConnectionString("EcommerceDbConnection")));

//            // Add services to the container
//            builder.Services.AddCors(options =>
//            {
//                options.AddPolicy("AllowLocalhost",
//                    policy => policy.WithOrigins("http://localhost:5174")  // Allow requests from frontend URL
//                                    .AllowAnyHeader()
//                                    .AllowAnyMethod());
//            });

//            builder.Services.AddScoped<IUserservice, Userservice>();
//            builder.Services.AddScoped<IProductservice, Productservice>();
//            builder.Services.AddScoped<ICartservice,Cartservice>();
//            builder.Services.AddScoped<IWishlistservice, Wishlistservice>();
//            builder.Services.AddControllers();

//            // Configure Swagger
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1", new() { Title = "Ecommerce API", Version = "v1" });
//                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//                {
//                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//                    Description = "Please enter token in the format **Bearer {your token}**",
//                    Name = "Authorization",
//                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
//                    Scheme = "bearer",
//                    BearerFormat = "JWT"
//                });
//                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
//                {
//                    {
//                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//                        {
//                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
//                            {
//                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
//                                Id = "Bearer"
//                            }
//                        },
//                        new string[] { }
//                    }
//                });
//            });

//            // Configure JWT authentication
//            builder.Services.AddAuthentication(options =>
//            {
//                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            })
//            .AddJwtBearer(options =>
//            {
//                options.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidateAudience = true,
//                    ValidateLifetime = true,
//                    ValidateIssuerSigningKey = true,
//                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
//                    ValidAudience = builder.Configuration["Jwt:Audience"],
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//                };
//            });

//            var app = builder.Build();
//            app.UseCors("AllowLocalhost");

//            // Configure the HTTP request pipeline
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API V1"));
//            }

//            app.UseHttpsRedirection();

//            // Add authentication and authorization middleware
//            app.UseAuthentication(); // Add this line
//            app.UseAuthorization();

//            app.MapControllers();

//            app.Run();
//        }
//    }
//}

using Ecommerse_shoes_backend.Dbcontext;
using Ecommerse_shoes_backend.Middleware;
using Ecommerse_shoes_backend.Services.CartService;
using Ecommerse_shoes_backend.Services.Productservice;
using Ecommerse_shoes_backend.Services.Userservice;
using Ecommerse_shoes_backend.Services.WishListService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ecommerse_shoes_backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add AutoMapper if you are using it
            builder.Services.AddAutoMapper(typeof(Program));

            // Configure DbContext (Ensure your connection string is correct in appsettings.json)
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EcommerceDbConnection")));

            // CORS configuration to allow frontend to connect (update the port if necessary)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    policy => policy.AllowAnyOrigin()  // Adjust to match frontend URL
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            // Register services
            builder.Services.AddScoped<IUserservice, Userservice>();
            builder.Services.AddScoped<IProductservice, Productservice>();
            builder.Services.AddScoped<ICartservice, Cartservice>();
            builder.Services.AddScoped<IWishlistservice, Wishlistservice>();
            builder.Services.AddControllers();

            // Swagger configuration for API documentation and JWT Authentication
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Ecommerce API", Version = "v1" });

                // Add JWT Bearer token definition
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please enter token in the format **Bearer {your token}**",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                // Apply JWT security requirement to all endpoints
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // Configure JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            var app = builder.Build();

            // Use CORS to allow requests from the frontend
            app.UseCors("AllowLocalhost");

            app.UseStaticFiles();
            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API V1"));
            }

            app.UseHttpsRedirection(); // Optional if you're using HTTP locally

            // Add authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<GetUserIdMiddleware>();
            // Map controllers to routes
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}

