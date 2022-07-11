namespace VerticalSlice.Features.ListReservations;

public static class GetReservationsApi
{
    public static void RegisterGetReservationsApi(this WebApplication app)
    {
        app.MapGet("reservation", async (IMediator mediator, string? day) =>
        {
            day ??= DateTime.Now.Date.ToString();

            var dateParsed = DateOnly.TryParse(day, out var dateOnly);
            if (!dateParsed)
            {
                return Results.BadRequest($"Parameter in incorrect format: {nameof(day)}. Acceptable format: dd-MM-yyyy");
            }

            var result = await mediator.Send(new GetReservationsQuery(dateOnly));

            return Results.Ok(result);
        })
        .Produces<ReservationsListDto>(StatusCodes.Status200OK)
        .WithName("GetReservations")
        .WithDisplayName("Get Reservations List")
        .WithTags("Reservation");

        app.MapGet("reservation/{id}", async (IMediator mediator, int id) =>
        {
            var result = await mediator.Send(new GetReservationDetailsQuery(id));

            if (result is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);
        })
        .Produces<GetReservationDetailsDto>(StatusCodes.Status200OK)
        .WithName("GetReservationDetails")
        .WithDisplayName("Get Reservation Details")
        .WithTags("Reservation");
    }
}
