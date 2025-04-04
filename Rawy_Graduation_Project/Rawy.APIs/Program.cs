
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rawy.APIs.Helper;
using Rawy.BLL.Interfaces;
using Rawy.BLL.Services;
using Rawy.DAL.Data;
using Rawy.DAL.Models;
using Rawy.BLL.Repositories;
using Rawy.BLL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
namespace Rawy.APIs
{
	public class Program
	{
		public static void Main(string[] args)
		{
			#region ConfigureServices
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			builder.Services.AddDbContext<RawyDBContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

			builder.Services.AddIdentity<AppUser, IdentityRole<int>>()
							.AddEntityFrameworkStores<RawyDBContext>()
							.AddDefaultTokenProviders();

			builder.Services.AddAuthentication(o =>
			{
				o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
				o.DefaultForbidScheme = GoogleDefaults.AuthenticationScheme;
				o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			})
				.AddGoogle(options =>
				{
					IConfigurationSection googleAuth = builder.Configuration.GetSection("Authentication:Google");
					options.ClientId = googleAuth["ClientId"];
					options.ClientSecret = googleAuth["ClientSecret"];
				});

			builder.Services.AddScoped<ITokenService, TokenService>();
			builder.Services.AddScoped<GoogleAuthService>();

			builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

			builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

			//builder.Services.AddAutoMapper(typeof(MappingProfiles));
			builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));  // Allow DI For AutoMapper
			#endregion

			var app = builder.Build();


			#region AutoMigration
			using var Scope = app.Services.CreateScope();

			var Services = Scope.ServiceProvider;

			var _Context = Services.GetRequiredService<RawyDBContext>();
			// Ask CLR To Creating Object From HakawyDBContext

			var loggerFactory = Services.GetRequiredService<ILoggerFactory>();

			try
			{
				_Context.Database.MigrateAsync(); // Update  Database
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "an error has been occured during appling migrations");

			}

			#endregion

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
