using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HotDeskDB>(opt => opt.UseInMemoryDatabase("HotDeskDB"));
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

class HotDeskDB : DbContext
{
    public HotDeskDB(DbContextOptions<HotDeskDB> options) : base(options) { }
}