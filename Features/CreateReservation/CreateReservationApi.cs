using VerticalSlice.Data;

namespace VerticalSlice.Features.CreateReservation;

public static class CreateReservationApi
{
    public static void RegisterCreateReservationApi(this WebApplication app)
    {
        app.MapPost("reservation", async (IMediator mediator, CreateReservationDto dto) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Results.BadRequest($"Missing required parameter: {nameof(dto.Name)}");
            }
            if (string.IsNullOrWhiteSpace(dto.ContactEmail))
            {
                return Results.BadRequest($"Missing required parameter: {nameof(dto.ContactEmail)}");
            }
            if (string.IsNullOrWhiteSpace(dto.Day))
            {
                return Results.BadRequest($"Missing required parameter: {nameof(dto.Day)}");
            }

            var dayParsed = DateOnly.TryParse(dto.Day, out var day);
            if (!dayParsed)
            {
                return Results.BadRequest($"Parameter in incorrect format: {nameof(dto.Day)}. Acceptable format: dd-MM-yyyy");
            }

            var result = await mediator.Send(new CreateReservationCommand(dto.Name, dto.ContactEmail, day));

            return result.IsSuccess
                ? Results.Ok(new CreatedReservationDto(result.Reservation!))
                : Results.BadRequest(new { error = result.Error });

        })
        .Accepts<CreateReservationDto>("application/json")
        .Produces<CreatedReservationDto>(StatusCodes.Status200OK)
        .WithName("CreateReservation")
        .WithDisplayName("Create Reservation")
        .WithTags("Reservation");
    }

    public record CreateReservationDto(
        string? Name, 
        string? ContactEmail, 
        string? Day);

    public record CreatedReservationDto(
        int Id, 
        string Name, 
        string ContactEmail, 
        string Day)
    {
        public CreatedReservationDto(Reservation reservation) 
            : this(reservation.Id, reservation.Name, reservation.ContactEmail, reservation.Day.ToString())
        {
        }
    }
}

