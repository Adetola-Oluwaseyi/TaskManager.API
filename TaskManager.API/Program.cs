
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManager.API.Configurations;
using TaskManager.API.Contracts;
using TaskManager.API.Data;
using TaskManager.API.Repository;
namespace TaskManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionstring = builder.Configuration
                .GetConnectionString("TaskManagerDbConnectionString");

            builder.Services.AddDbContext<TaskManagerDbContext>(options => options
            .UseSqlServer(connectionstring));

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters
                .Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
            });

            builder.Services.AddAutoMapper(typeof(MapperConfig));

            builder.Services.AddScoped<IAuthManager, AuthManager>();
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

            builder.Services.AddIdentityCore<ApiUser>().AddRoles<IdentityRole>().
                AddEntityFrameworkStores<TaskManagerDbContext>();

            //for bearer token   
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(builder.Configuration["Jwt:Key"])),
                    //ClockSkew = TimeSpan.Zero //token grace period
                };
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin());  //policy.WithOrigins("https://yourfrontend.example")
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
