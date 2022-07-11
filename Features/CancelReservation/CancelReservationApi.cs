namespace VerticalSlice.Features.CancelReservation;

public static class CancelReservationApi
{
    public static void RegisterCancelReservationApi(this WebApplication app)
    {
        app.MapDelete("reservation/{id}", async (IMediator mediator, int id) =>
        {
            await mediator.Send(new CancelReservationCommand(id));

            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent)
        .WithName("CancelReservation")
        .WithDisplayName("Cancel Reservation")
        .WithTags("Reservation");
    }
}

