using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HotDeskDB>(opt => opt.UseInMemoryDatabase("HotDeskDB"));

string adminToken = "1234567890";

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.MapGet("/", () => "API made by Seweryn Jarco");

// Administration: get all locations, Desks[] doesnt work here
app.MapGet("/admin/locations", async ([FromHeader(Name = "login-token")] string? loginToken, HotDeskDB db) =>
{
    if(!adminToken.Equals(loginToken)) return Results.Unauthorized();
    return Results.Ok(await db.Locations.ToListAsync());
});

// Administration: get all desks, Reservations[] doesnt work here
app.MapGet("/admin/desks", async ([FromHeader(Name = "login-token")] string? loginToken, HotDeskDB db) =>
{
    if (!adminToken.Equals(loginToken)) return Results.Unauthorized();
    return Results.Ok(await db.Desks.ToListAsync());
});

// Administration: add location
app.MapPost("/admin/locations", async([FromHeader(Name = "login-token")] string ? loginToken, Location location, HotDeskDB db) =>
{
    if (!adminToken.Equals(loginToken)) return Results.Unauthorized();
    db.Locations.Add(location);
    await db.SaveChangesAsync();
    return Results.Created($"/locations/{location.Id}", location);
});

// Administration: remove location
app.MapDelete("/admin/locations/{id}", async([FromHeader(Name = "login-token")] string ? loginToken, int id, HotDeskDB db) =>
{
    if (!adminToken.Equals(loginToken)) return Results.Unauthorized();

    var loc = db.Locations.Where(x => x.Id == id).Include(x => x.Desks).FirstOrDefault();
    if (loc == null) return Results.NotFound();

    // check if there are no desks at this location
    if (loc.Desks.Count > 0) return Results.Conflict();

    db.Locations.Remove(loc);
    await db.SaveChangesAsync();
    return Results.Ok(loc);
});

// Administration: add desk
app.MapPost("/admin/locations/{id}/desks", async ([FromHeader(Name = "login-token")] string ? loginToken, int id, Desk desk, HotDeskDB db) =>
{
    if (!adminToken.Equals(loginToken)) return Results.Unauthorized();

    var loc = db.Locations.Where(x => x.Id == id).Include(x => x.Desks).FirstOrDefault();
    if (loc == null) return Results.NotFound();

    loc.Desks.Add(desk);
    await db.SaveChangesAsync();
    return Results.Created($"/locations/{loc.Id}/desks/{desk.Id}", desk);
});

// Administration: remove desk
app.MapDelete("/admin/locations/{lid}/desks/{id}", async ([FromHeader(Name = "login-token")] string ? loginToken, int lid, int id, HotDeskDB db) =>
{
    if (!adminToken.Equals(loginToken)) return Results.Unauthorized();

    // find location with id
    var loc = db.Locations.Where(x => x.Id == lid).Include(x => x.Desks).ThenInclude(y => y.Reservations).FirstOrDefault();
    if (loc == null) return Results.NotFound();

    // find desk with id
    var desk = loc.Desks.Where(d => d.Id == id).FirstOrDefault();
    if (desk == null) return Results.NotFound();

    // check if there are no pending reservations at this desk
    DateTime now = DateTime.Now;
    foreach (Reservation R in desk.Reservations)
    {
        if (R.EndDate > now) return Results.Conflict();
    }

    db.Desks.Remove(desk);
    loc.Desks.Remove(desk);
    await db.SaveChangesAsync();
    return Results.Ok(desk);
});

// Administration: change desk availability
app.MapPut("/admin/locations/{lid}/desks/{id}", async ([FromHeader(Name = "login-token")] string ? loginToken, int lid, int id, Desk inputDesk, HotDeskDB db) =>
{
    if (!adminToken.Equals(loginToken)) return Results.Unauthorized();

    // find location with id
    var loc = db.Locations.Where(x => x.Id == lid).Include(x => x.Desks).FirstOrDefault();
    if (loc == null) return Results.NotFound();

    // find desk with id
    var desk = loc.Desks.Where(d => d.Id == id).FirstOrDefault();
    if (desk == null) return Results.NotFound();

    desk.IsAvailable = inputDesk.IsAvailable;
    await db.SaveChangesAsync();
    return Results.Ok(desk);
});

// Get info about specific location
app.MapGet("/locations/{id}", ([FromHeader(Name = "login-token")] string? loginToken, int id, HotDeskDB db) => {
    Location? loc;
    if (adminToken.Equals(loginToken)) 
        loc = db.Locations.Where(x => x.Id == id).Include(x => x.Desks).ThenInclude(y => y.Reservations).FirstOrDefault();
    else
        loc = db.Locations.Where(x => x.Id == id).Include(x => x.Desks).FirstOrDefault();
    return loc != null ? Results.Ok(loc) : Results.NotFound();
});

// Get info about specific desk
app.MapGet("/locations/{lid}/desks/{id}", ([FromHeader(Name = "login-token")] string? loginToken, int lid, int id, HotDeskDB db) =>
{
    Location? loc;
    // find location with id
    if (adminToken.Equals(loginToken))
        loc = db.Locations.Where(x => x.Id == lid).Include(x => x.Desks).ThenInclude(y => y.Reservations).FirstOrDefault();
    else
        loc = db.Locations.Where(x => x.Id == lid).Include(x => x.Desks).FirstOrDefault();

    if (loc == null) return Results.NotFound();

    // find desk with id
    var desk = loc.Desks.Where(d => d.Id == id).FirstOrDefault();
    if (desk == null) return Results.NotFound();

    return Results.Ok(desk);
});

// Filter desks in specific location
app.MapGet("/locations/{id}/desks", ([FromHeader(Name = "login-token")] string ? loginToken, int id, HotDeskDB db) =>
{
    // find location with id
    Location? loc;
    // find location with id
    if (adminToken.Equals(loginToken))
        loc = db.Locations.Where(x => x.Id == id).Include(x => x.Desks).ThenInclude(y => y.Reservations).FirstOrDefault();
    else
        loc = db.Locations.Where(x => x.Id == id).Include(x => x.Desks).FirstOrDefault();

    if (loc == null) return Results.NotFound();

    return Results.Ok(loc.Desks.ToList());
});

// Check if desk is available
app.MapGet("/locations/{lid}/desks/available/{id}", (int lid, int id, HotDeskDB db) =>
{
    // find location with id
    var loc = db.Locations.Where(x => x.Id == lid).Include(x => x.Desks).FirstOrDefault();
    if (loc == null) return Results.NotFound();

    // find desk with id
    var desk = loc.Desks.Where(d => d.Id == id).FirstOrDefault();
    if (desk == null) return Results.NotFound();

    if (desk.IsAvailable) return Results.Json(true);
    else return Results.Json(false);
});

// Get available desks at specific location
app.MapGet("/locations/{lid}/desks/available", (int lid, HotDeskDB db) =>
{
    // find location with id
    var loc = db.Locations.Where(x => x.Id == lid).Include(x => x.Desks).FirstOrDefault();
    if (loc == null) return Results.NotFound();
    
    var list = loc.Desks.Where(x => x.IsAvailable).ToList();
    return Results.Ok(list);
});

// Book a desk
app.MapPost("/locations/{lid}/desks/{id}/book", async (int lid, int id, Reservation res, HotDeskDB db) =>
{
    // find location with id
    var loc = db.Locations.Where(x => x.Id == lid).Include(x => x.Desks).ThenInclude(y => y.Reservations).FirstOrDefault();
    if (loc == null) return Results.NotFound();

    // find desk with id
    var desk = loc.Desks.Where(d => d.Id == id).FirstOrDefault();
    if (desk == null) return Results.NotFound();

    // check if reservation is for > 0 and < 7 days
    var days = (res.EndDate - res.StartDate).TotalDays;
    if (days < 0 || days > 7) return Results.Conflict();

    // check if desk is available
    if (!desk.IsAvailable) return Results.Conflict();

    // check wether new reservation date collides with other
    bool DateAvailable = true;
    foreach (Reservation R in desk.Reservations)
    {
        // check if two booking dates overlaps
        if(res.StartDate <= R.StartDate && res.EndDate >= R.StartDate)
        {
            DateAvailable = false;
            break;
        }
    }

    if (!DateAvailable) return Results.Conflict();

    res.desk = desk;
    desk.Reservations.Add(res);
    await db.SaveChangesAsync();
    return Results.Ok(res);
});

// Change desk
app.MapPost("/locations/{lid}/reservation/{id}", async (int lid, int id, ChangeReservationData newres, HotDeskDB db) =>
{
    // find location with id
    var loc = db.Locations.Where(x => x.Id == lid).Include(x => x.Desks).ThenInclude(y => y.Reservations).FirstOrDefault();
    if (loc == null) return Results.NotFound();

    // find reservation
    var res = db.Reservations.Where(x => x.Id == id).FirstOrDefault();
    if (res == null) return Results.NotFound();

    // check if reservation is in less than 24 h
    DateTime dateTime = DateTime.Now;
    if((res.StartDate - dateTime).TotalHours < 24) return Results.Conflict();

    // get desk
    Desk? desk = null;
    foreach(Desk D in db.Desks.Include(x => x.Reservations))
    {
        if(D.Reservations.Where(x => x == res).FirstOrDefault() != null)
        {
            desk = D;
            break;
        }
    }

    if (desk == null) return Results.NotFound();

    var newdeskid = newres.newdeskid;
    if (newres == null) return Results.NotFound();

    // get new desk
    var newdesk = db.Desks.Where(x => x.Id == newdeskid).Include(x => x.Reservations).FirstOrDefault();
    if (newdesk == null) return Results.NotFound();

    // check if reservation doesnt overlap any in new desk
    bool DateAvailable = true;
    foreach (Reservation R in newdesk.Reservations)
    {
        // check if two booking dates overlaps
        if (res.StartDate <= R.StartDate && res.EndDate >= R.StartDate)
        {
            DateAvailable = false;
            break;
        }
    }
    if (!DateAvailable) return Results.Conflict();

    // switch reservation to another desk
    desk.Reservations.Remove(res);
    newdesk.Reservations.Add(res);
    await db.SaveChangesAsync();
    return Results.Ok(res);
});

app.Run("https://localhost:7009");

class Location
{
    public int Id { set; get; }
    public string? Name { set; get; }
    public List<Desk> Desks { set; get; }
    public Location()
    {
        Desks = new List<Desk>();
    }
}

class Desk
{
    public int Id { set; get; }
    public string? Description { set; get; }
    public List<Reservation> Reservations { set; get; }
    public Desk()
    {
        Reservations = new List<Reservation>();
    }
    public bool IsAvailable { set; get; } = true;

    public Location location;
}

class Reservation
{
    public int Id { set; get; }
    public DateTime StartDate { set; get; } = DateTime.Now;
    public DateTime EndDate { set; get; } = DateTime.Now.AddHours(1);
    public string? ReservedBy { set; get; }
    public Desk desk;
}

class ChangeReservationData
{
    public int newdeskid { set; get; }
};

class HotDeskDB : DbContext
{
    public HotDeskDB(DbContextOptions<HotDeskDB> options) : base(options) { }
    public HotDeskDB() { }
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Desk> Desks => Set<Desk>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
}

public partial class Program { }
