using AuthSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AuthSystem.RefreshTokens.DependencyInjection;
using AuthSystem.RefreshTokens;
using AuthSystem.Jwt.DependencyInjection;
using AuthSystem.Jwt;
using AuthSystem.DataModel;
using CPMethod.WebAPI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string? connectionString = builder.Configuration.GetConnectionString("AuthSystem.Database");

            //Add services to the container.

            //if (builder.Environment.IsProduction())
            //{
            //    builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(connectionString));
            //}

            //if (builder.Environment.IsDevelopment())
            //{
                var connection = new SqliteConnection(connectionString);
                connection.Open();
                builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connection));
            //}

            builder.Services.AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;

                // TODO: Set up SendGrid.

                // TODO: Configure email confirmation.

                //options.SignIn.RequireConfirmedEmail = true;

                // TODO: Configure account confirmation.

                //options.SignIn.RequireConfirmedAccount = true;

                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();

            builder.Services.AddControllers()
                            .AddNewtonsoftJson(options => 
                            {
                                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                                options.SerializerSettings.Formatting = Formatting.None;
                                options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                                options.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
                                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                            });

            builder.Services.AddAuthentication()
                            .AddJwtBearer(builder.Configuration.GetSection(nameof(JwtOptions)),
                                          builder.Configuration);

            builder.Services.AddRefreshTokens(builder.Configuration.GetSection(nameof(RefreshTokenOptions)))
                            .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.Configure<PasswordHasherOptions>(
                options => options.IterationCount = 500000);

            builder.Services.AddTransient<ISvgService, SvgService>();

            builder.Services.AddAuthorizationCore();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMemoryCache();

            builder.Services.ConfigureSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "CPMethod",
                    Version = "v1"
                });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>()!;

                //dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
          
            app.UseHttpsRedirection();

            app.UseHsts();
            app.UseAuthentication();
            app.UseJwtBlacklist();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}