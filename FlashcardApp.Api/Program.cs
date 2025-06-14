using FlashcardApp.Api.Interfaces;

namespace FlashcardApp.Api
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var corsPolicyName = "AllowedHosts";

            // Configure services
            builder.Services.AddControllers();
            var (AiGeneration, Global) = builder.Services.AddOpenApi(options => options.AddDocumentTransformer<BearerSecuritySchemeTransformer>())
                .AddConfigurationService(builder.Configuration)
                .AddCorsService(corsPolicyName)
                .AddDatabaseService()
                .AddIdentityService()
                .AddAuthenticationService()
                .AddCacheService()
                .AddAutoMapperService()
                .AddJsonService()
                .AddExceptionHandler<GlobalExceptionHandler>()
                .Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(2))
                .ConfigureFormOptions()
                .AddExceptionHandlerService()
                .AddHttpContextAccessor()
                .AddRateLimitingService();

            // Register services
            builder.Services.AddScoped<IUsersService, UsersService>()
                .AddScoped<IDecksService, DecksService>()
                .AddScoped<ICardsService, CardsService>()
                .AddScoped<IReviewsService, ReviewsService>()
                .AddScoped<ISettingsService, SettingsService>()
                .AddScoped<IEmailSenderService, EmailSenderService>()
                .AddScoped<IUploadsService, UploadsService>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<ResponseTemplate>()
                .AddScoped<ResetCode>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "Flashcard Application");
                });
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseExceptionHandler("/error");
            app.UseHttpsRedirection();
            app.UseCors(corsPolicyName);
            app.UseRateLimiter();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers().RequireRateLimiting(Global);

            app.Run();
        }
    }
}