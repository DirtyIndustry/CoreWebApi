using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApi.Authorization;
using CoreWebApi.Caching;
using CoreWebApi.Caching.Redis;
using CoreWebApi.Dtos;
using CoreWebApi.Entities;
using CoreWebApi.Extensions;
using CoreWebApi.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace CoreWebApi
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            #region CORS
            services.AddCors(options => options.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().AllowAnyOrigin()));
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v1.0.0",
                    Title = "Web Core API",
                    Description = "Extendable Information Management System Framework.",
                    // TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "Dirty",
                        Email = "xiao1_yu3@163.com",
                        Url = "http://www.dirtyindustry.cn"
                    }
                });
            });
            #endregion

            #region JwtBearer
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Jti", policy =>
                {
                    policy.Requirements.Add(new Authorization.ValidJtiRequirement());
                });
            })
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
            .AddJwtBearer("JwtBearer", jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("the secret that needs to be at least 16 characeters long for HmacSha256")),

                    ValidateIssuer = false,
                    // ValidIssuer = "The name of the issuer",

                    ValidateAudience = false,
                    // ValidAudience = "The name of the audience",

                    ValidateLifetime = true,    // validate the expiration and not-before values in the token
                    
                    ClockSkew = TimeSpan.FromMinutes(0) // n minute tolerance for the expiration date
                };
            });

            services.AddScoped<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, Authorization.ValidJtiHandler>();
            #endregion

            #region Database
            var connectionString = @"Server=127.0.0.1;database=entrance;uid=myuser;pwd=mypass";
            services.AddDbContext<EntranceContext>(options => options.UseMySql(connectionString));
            var connectionString2 = @"Server=127.0.0.1;database=DefaultCompany;uid=myuser;pwd=mypass";
            services.AddDbContext<CompanyContext>(options => options.UseMySql(connectionString2));
            #endregion

            #region Repositories
            services.AddScoped<IUnitOfWork<EntranceContext>, UnitOfWork<EntranceContext>>();
            services.AddScoped<IUnitOfWork<CompanyContext>, UnitOfWork<CompanyContext>>();
            services.AddScoped<IDeletedTokenRepository, DeletedTokenRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion

            #region Caching
            services.AddStackExchangeRedisCache(options =>
            {
                //用于连接Redis的配置
                options.Configuration = "localhost,password=mypass";// Configuration.GetConnectionString("RedisConnectionString");
                //Redis实例名 会加在Key前面
                //options.InstanceName = "RedisDistributedCache";
            });
            services.AddScoped<IDeletedTokenCache, DeletedTokenCache>();
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, EntranceContext entranceContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseJsonExceptionHandler(loggerFactory);
            }

            // entranceContext.EnsureSeedDataForContext();

            app.UseMiddleware<JwtInCookieMiddleware>();
            // app.UseMiddleware<JwtReissueMiddleware>();
            app.UseAuthentication();

            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<LoginDto, Login>();
                config.CreateMap<LoginCreateDto, Login>();
                config.CreateMap<LoginCreateDto, User>();
                config.CreateMap<Login, UserInfoDto>();
                config.CreateMap<User, UserInfoDto>();
                config.CreateMap<UserModificationDto, User>();
            });

            app.UseCors("cors");

            app.UseMvc();

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp v1");
            });
            #endregion
        }
    }
}
