namespace VerticalSlice.Data;

public static class SeedData
{
    internal static void Initialize(ReservationsContext context)
    {
        if (context.Reservations.Any())
        {
            return;
        }

        context.Reservations.AddRange(new[]
        {
            new Reservation("Daniel", "daniel@mail.com", DateTime.Parse("22-07-2022"), DateTime.Now),
            new Reservation("Daniel", "daniel@mail.com", DateTime.Parse("22-07-2022"), DateTime.Now),
            new Reservation("Daniel", "daniel@mail.com", DateTime.Parse("22-07-2022"), DateTime.Now),
            new Reservation("Daniel", "daniel@mail.com", DateTime.Parse("22-07-2022"), DateTime.Now),
            new Reservation("Daniel", "daniel@mail.com", DateTime.Parse("22-07-2022"), DateTime.Now),
        });

        context.SaveChanges();
    }
}
