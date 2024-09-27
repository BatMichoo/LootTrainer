using Core.AutoMapper;
using Core.Repositories.GuildApplicationRepo;
using Core.Services.AppFormService;
using Core.Services.BattleNet;
using Core.Services.UserService;
using Core.Utilities;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LootTrainer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            string dbAccessCreds = Environment.GetEnvironmentVariable(builder.Configuration["ConnectionStrings:DbAccessEnvName"]) ?? throw new ArgumentNullException("No connection string to the DB.");

            builder.Services.AddDbContext<LootDbContext>(opt =>
            {
                opt.UseSqlServer(string.Format(builder.Configuration.GetConnectionString("LootingTrainer"), dbAccessCreds));
            })
            .AddIdentity<BlizzUser, IdentityRole>(opt =>
             {
                opt.SignIn.RequireConfirmedAccount = false;
                opt.SignIn.RequireConfirmedEmail = false;
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = false;
                opt.Password.RequiredUniqueChars = 0;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
             })
            .AddEntityFrameworkStores<LootDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<BlizzUser>>()
            .AddUserManager<UserManager<BlizzUser>>()
            .AddRoleManager<RoleManager<IdentityRole>>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Blizzard";
            })
            .AddCookie(c =>
            {
                c.Cookie.IsEssential = true;
                c.Cookie.HttpOnly = true;
            })
            .AddOAuth("Blizzard", options =>
            {
                ConfigureBlizzardOAuth(options, builder);
            });

            builder.Services.AddScoped<IAppFormService, AppFormService>();
            builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
            builder.Services.AddScoped<BattleNetService>();
            builder.Services.AddScoped<UserService>();

            builder.Services.AddHttpClient<BattleNetService>(c =>
            {
                c.BaseAddress = new Uri(string.Format(Endpoints.Base.BlizzardAPI, ApiRegions.EU));
                c.DefaultRequestHeaders.Add(BlizzardApiHeaders.Namespace, string.Format(BlizzardNamespaces.Profile.Progression, ApiRegions.EU));
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAutoMapper(o =>
            {
                o.AddProfile(new AppFormProfile());
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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

        private static void ConfigureBlizzardOAuth(OAuthOptions options, WebApplicationBuilder builder)
        {
            string clientId = Environment.GetEnvironmentVariable(builder.Configuration["BlizzardAPI:ClientIdEnvName"])
                                                                                ?? throw new ArgumentNullException(nameof(clientId));

            string clientSecret = Environment.GetEnvironmentVariable(builder.Configuration["BlizzardAPI:ClientSecretName"])
                                                                ?? throw new ArgumentNullException(nameof(clientSecret));

            options.ClientId = clientId;
            options.ClientSecret = clientSecret;

            options.CallbackPath = new PathString("/login");

            options.AuthorizationEndpoint = $"{Endpoints.Base.OAuth}authorize";
            options.TokenEndpoint = $"{Endpoints.Base.OAuth}token";
            options.UserInformationEndpoint = $"{Endpoints.Base.OAuth}userinfo";

            options.SaveTokens = true;

            options.Scope.Add("openid wow.profile");

            options.ClaimActions.MapJsonKey(Claims.BattleNetId, "sub");
            options.ClaimActions.MapJsonKey(Claims.BattleTag, "battletag");

            options.Events = new OAuthEvents
            {
                OnCreatingTicket = async context =>
                {
                    await ConfigureOnCreatingTicket(context);
                },
                OnTicketReceived = async context =>
                {
                    await ConfigureOnTicketReceived(context);
                }
            };
        }

        private static async Task ConfigureOnCreatingTicket(OAuthCreatingTicketContext context)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            var response = await context.Backchannel.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var userDoc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            context.RunClaimActions(userDoc.RootElement);

            context.Principal!.Identities.FirstOrDefault()!.AddClaim(new Claim(Claims.AccessToken, context.AccessToken!));
        }

        private static async Task ConfigureOnTicketReceived(TicketReceivedContext context)
        {
            var services = context.HttpContext.RequestServices;

            var userManager = (UserManager<BlizzUser>)services.GetRequiredService(typeof(UserManager<BlizzUser>));

            string battleTag = context.Principal!.Claims.FirstOrDefault(c => c.Type == Claims.BattleTag)!.Value;

            if (battleTag != null)
            {
                string userName = battleTag.Substring(0, battleTag.IndexOf('#'));

                string email = string.Format(EmailPlaceholder.LootingTrainer, userName);

                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    user = new BlizzUser { UserName = userName, Email = email };

                    var result = await userManager.CreateAsync(user);

                    var claimsResult = await userManager.AddClaimsAsync(user, context.Principal.Claims);

                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.NameIdentifier, user.Id));
                }

                context.Principal.Identities.FirstOrDefault()!.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

                var signInManager = (SignInManager<BlizzUser>)services.GetRequiredService(typeof(SignInManager<BlizzUser>));

                await signInManager.SignInAsync(user!, false);
            }
        }
    }
}
