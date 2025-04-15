using GameStoreData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//add Authentication
builder.Services.AddAuthentication("BasicAuth")
    .AddScheme<AuthenticationSchemeOptions, GameStoreAPI.Authentication.AuthenticationHandler>("BasicAuth", null);

//seting up Entity Framework and SQL Server
builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("GameStoreData")          //tell EF to store migrations here
));


//allow CORS so Postman and our Python client can access it
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

var app = builder.Build();

//enable CORS
app.UseCors();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();        //to enable authentication in the middleware chain

app.UseAuthorization();

app.MapControllers();

app.Run();
