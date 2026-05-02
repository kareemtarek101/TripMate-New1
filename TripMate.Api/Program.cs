using TripMate.Api.Middlewares;
using TripMate.Application;
using TripMate.Application.Auth.Interfaces;
using TripMate.Application.Auth.Services;
using TripMate.Infrastructure;
using TripMate.Application.Interface;
using TripMate.Application.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IInteractionService, InteractionService>();
//builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookingService, BookingService>();
var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CheckUserNotDeletedMiddleware>();