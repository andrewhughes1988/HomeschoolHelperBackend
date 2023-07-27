using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeschoolHelperApi.Data;
using HomeschoolHelperApi.Services.RecordService;
using HomeschoolHelperApi.Services.StudentService;
using HomeschoolHelperApi.Services.SubjectService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors;
using MySql.Data.EntityFrameworkCore;

namespace HomeschoolHelperApi
{
    public class Startup
    {

        readonly string corsPolicyOrigins = "corsPolicyOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy(name: corsPolicyOrigins, 
                builder => {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey  //can add to configuration or env file and read it
                            (System.Text.Encoding.UTF8.GetBytes("ULTRASUPERSECRETKEYFORTHEAPI")), 
                        ValidateIssuer = false,
                        ValidateAudience = false

                    };
                 });

            services.AddDbContext<DataContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DataContext")));

            services.AddControllers();

            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddScoped<IAuthenticationRepo, AuthenticationRepo>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IRecordService, RecordService>();
            services.AddScoped<ISubjectService, SubjectService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(corsPolicyOrigins);

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
