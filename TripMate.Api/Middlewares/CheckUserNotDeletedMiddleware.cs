using System.Security.Claims;
using TripMate.Infrastructure.Persistence;
public class CheckUserNotDeletedMiddleware
{
    private readonly RequestDelegate _next;

    public CheckUserNotDeletedMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, TripMateDbContext db)
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            await _next(context);
            return;
        }

        var userId = context.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null)
        {
            var user = await db.Users.FindAsync(int.Parse(userId));

            if (user == null || user.IsDeleted)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Account deleted");
                return;
            }
        }

        await _next(context);
    }
}