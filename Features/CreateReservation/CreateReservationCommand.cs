using VerticalSlice.Data;

namespace VerticalSlice.Features.CreateReservation;

public record CreateReservationCommand(
    string Name, 
    string ContactEmail, 
    DateOnly Day) : IRequest<CreateReservationResult>;

public record CreateReservationResult(
    bool IsSuccess, 
    Reservation? Reservation, 
    string? Error = null)
{
    public static CreateReservationResult Success(Reservation reservation) => new(true, reservation);
    public static CreateReservationResult Fail(string error) => new(false, null, error);
}

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, CreateReservationResult>
{
    private const int NumberOfTables = 5;
    private readonly ReservationsContext reservationsContext;

    public CreateReservationCommandHandler(ReservationsContext reservationsContext)
    {
        this.reservationsContext = reservationsContext;
    }

    public async Task<CreateReservationResult> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
    {
        if (IsDateInPast(command.Day))
        {
            return CreateReservationResult.Fail("Reservation cannot be in the past");
        }

        if (IsRestaurantClosed(command.Day))
        {
            return CreateReservationResult.Fail("Restaurant is closed on that day");
        }

        if (IsRestaurantAlreadyFullyBooked(command.Day))
        {
            return CreateReservationResult.Fail("Restaurant is fully booked");
        }

        var reservation = new Reservation(
            command.Name,
            command.ContactEmail,
            day: command.Day.ToDateTime(TimeOnly.MinValue),
            createdAt: DateTimeOffset.Now);

        reservationsContext.Add(reservation);
        await reservationsContext.SaveChangesAsync(cancellationToken);

        return CreateReservationResult.Success(reservation);
    }

    private static bool IsDateInPast(DateOnly day) => day < DateOnly.FromDateTime(DateTime.Now);

    private static bool IsRestaurantClosed(DateOnly date)
    {
        return date.DayOfWeek == DayOfWeek.Sunday
            || date.DayOfWeek == DayOfWeek.Monday;
    }

    private bool IsRestaurantAlreadyFullyBooked(DateOnly date)
    {
        var day = date.ToDateTime(TimeOnly.MinValue);
        var exisitingReservations = reservationsContext.Reservations.Where(x => x.Day == day);
        return exisitingReservations.Count() >= NumberOfTables;
    }
}
