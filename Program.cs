using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using VerticalSlice.Data;
using VerticalSlice.Features.CancelReservation;
using VerticalSlice.Features.CreateReservation;
using VerticalSlice.Features.ListReservations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ReservationsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReservationsContext"));
});
builder.Services.AddSingleton<ReservationsDapperContext>();

builder.Services.AddMediatR(typeof(CreateReservationCommand).Assembly);
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Restaurant API",
        Description = "API of some random restaurant :)",
        Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ReservationsContext>();
        context.Database.EnsureCreated();
        SeedData.Initialize(context);
    }
}

app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurant API V1");
});

app.RegisterCreateReservationApi();
app.RegisterCancelReservationApi();
app.RegisterGetReservationsApi();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
