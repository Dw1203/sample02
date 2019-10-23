using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using sample02.Data;
using sample02.IRepository;
using sample02.IService;
using sample02.Repository;
using sample02.Service;
using Swashbuckle.AspNetCore.Swagger;

namespace sample02
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);          
            services.AddDbContext<SchoolDbContext>(
                x =>
                {
                    x.UseSqlServer(Configuration["ConnectionStrings:Defalut"]);
                }
                );

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStuClassService, StuClassService>();
            services.AddScoped<IStuClassRepository, StuClassRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IAchievementRepository, AchievementRepository>();
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
            services.AddScoped<IStudentCourseService, StudentCourseService>();
            services.AddScoped<IClassroomRepository, ClassroomRepository>();
            services.AddScoped<IClassroomService, ClassroomService>();
            services.AddScoped<ICourseClassroomRepository, CourseClassroomRepository>();
            services.AddScoped<ICourseClassroomService, CourseClassroomService>();
            services.AddScoped<IPasternRepository, PasternRepositoy>();
            services.AddScoped<IPasternService, PasternService>();
            services.AddScoped<IMajorRepository, MajorRepository>();
            services.AddScoped<IMajorService, MajorService>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IOfficeService, OfficeService>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IPositionService, PositionService>();
            //配置AutoMapper
            var map = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfiguration());
            });
            IMapper mapper = map.CreateMapper();
            services.AddSingleton(mapper);

            //Swagger基本配置
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info()
                {
                    Title = "Sample02",
                    Description = "Sample02Api接口信息",
                    TermsOfService = "None",
                    Version = "v1.0.1"
                });
                //配置读取注释文档
                var path = PlatformServices.Default.Application.ApplicationBasePath;
                var fullpath = Path.Combine(path, "sample02.xml");
                x.IncludeXmlComments(fullpath, true);

                //Swagger配置授权
                var Issuer= Configuration.GetSection("Audience")["Issuer"];
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {
                      Issuer,
                      new string []{}
                    }
                };
                x.AddSecurityRequirement(security);
                x.AddSecurityDefinition(Issuer, new ApiKeyScheme()
                {
                    Name = "Authorizaiton",
                    Description= "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    In="header",
                    Type="apiKey"
                });
            });

            //配置身份认证与授权

            //1.基于策略的授权
            services.AddAuthorization(x =>
            {
                x.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
                x.AddPolicy("Client", policy => policy.RequireRole("Client").Build());
                x.AddPolicy("SystemAdmin", policy => policy.RequireRole("System", "Admin"));
            });

            //读取发行人 订阅人 密钥等信息
            var section = Configuration.GetSection("Audience");
            var issuer = section["Issuer"];
            var audience = section["Audience"];
            var secret = section["Secret"];
            var secretByte = System.Text.Encoding.ASCII.GetBytes(secret);
            var signKey = new SymmetricSecurityKey(secretByte);

            //添加验证
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signKey,
                    ValidateLifetime=true,
                    RequireExpirationTime=true,
                    ClockSkew=TimeSpan.Zero
            };
            });
                

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

          
            app.UseSwagger();
            app.UseSwaggerUI(c=>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json","Sample02测试");
            });

            //启用授权认证
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
