
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rawy.APIs.Helper;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Data;
using Rawy.DAL.Models;
using Rawy.BLL.Repositories;
using Rawy.BLL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Rawy.APIs.Services.Auth;
using Rawy.APIs.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Rawy.APIs.Services.AccountService;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Rawy.APIs.Services.Photo;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Diagnostics;
using Rawy.DAL.Models.Hubs;
using Rawy.APIs.Services.CommentService;
using Rawy.APIs.Services.StoryService;
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

			// Cloudinary Configuration
			builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

			builder.Services.AddScoped<IPhotoService,PhotoService>();

			builder.Services.AddIdentity<AppUser, IdentityRole<int>>()
							.AddEntityFrameworkStores<RawyDBContext>()
							.AddDefaultTokenProviders();

			builder.Services.AddAuthentication(o =>
			{
				o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
				o.DefaultForbidScheme = GoogleDefaults.AuthenticationScheme;
				o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			}).AddCookie()
			  .AddGoogle(options =>
				{
					var ClientId = builder.Configuration["Authentication:Google:ClientId"];
					if(ClientId == null)
						throw new ArgumentException(nameof(ClientId));

					var ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
					if (ClientSecret == null)
						throw new ArgumentException(nameof(ClientSecret));


				//	IConfigurationSection googleAuth = builder.Configuration.GetSection("Authentication:Google");
					options.ClientId = ClientId;
					options.ClientSecret = ClientSecret;
					options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				});

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = builder.Configuration["Jwt:Issuer"],
					ValidAudience = builder.Configuration["Jwt:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
				};
			});
			#region FacebookConfiguration
			//.AddFacebook(options =>
			//{
			// var appId = builder.Configuration["Authentication:Facebook:AppId"];
			// if (appId == null)
			//  throw new ArgumentException(nameof(appId));

			// var appSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
			// if (appSecret == null)
			//  throw new ArgumentException(nameof(appSecret));

			// options.AppId = appId;
			// options.AppSecret = appSecret;
			//}); 
			#endregion

			builder.Services.AddHttpContextAccessor();

			builder.Services.AddScoped<ITokenService, TokenService>();
			builder.Services.AddScoped<IGoogleAuthServices,GoogleAuthService>();
			//builder.Services.AddScoped<IFacebookAuthServices, FacebookAuthServices>();

			builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

			builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
			builder.Services.AddScoped<IAccountService, AccountService>();
			builder.Services.AddScoped<ICommentService, CommentService>();
			builder.Services.AddScoped<IStoryService, StoryService>();

			//builder.Services.AddAutoMapper(typeof(MappingProfiles));
			builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));  // Allow DI For AutoMapper

			builder.Services.AddSignalR();


			builder.Services.AddCors(options =>
			{
				options.AddPolicy("MyPolicy", config =>
				{
					config.AllowAnyHeader();
					config.AllowAnyMethod();
					config.AllowAnyOrigin();
					//config.WithOrigins("http://localhost:4200");
					//(builder.Configuration["FronEndUrl"]);

				});

			});
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

		//	app.UseMiddleware<ExceptionHandlerMiddleware>();
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			
			app.UseCors("MyPolicy");

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();
			app.MapHub<PostHub>("/posthub");

			app.Run();
		}
	}
}
