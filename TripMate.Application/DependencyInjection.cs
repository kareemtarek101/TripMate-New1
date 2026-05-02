using Microsoft.Extensions.DependencyInjection;
using TripMate.Application.Auth.Interfaces;
using TripMate.Application.Auth.Services;
using TripMate.Application.Services;
using TripMate.Application.Interface;



namespace TripMate.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Users
            services.AddScoped<IUserService, UserService>();

            // Auth
            services.AddScoped<IAuthService, AuthService>();

            // Destinations
            services.AddScoped<IDestinationService, DestinationService>();
            services.AddHttpClient<ISerpApiService, SerpApiService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IInteractionService, InteractionService>();
            services.AddScoped<IRecommendationService, RecommendationService>();

            return services;
        }
    }
}
