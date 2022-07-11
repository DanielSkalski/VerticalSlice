using VerticalSlice.Data;

namespace VerticalSlice.Features.CreateReservation;

public class CreateReservationValidator
{
    public ValidationResult Validate(CreateReservationCommand command, ReservationsContext dbContext, int numberOfTables)
    {
        if (IsDateInPast(command.Day))
        {
            return ValidationResult.Failed("Reservation cannot be in the past");
        }

        if (IsRestaurantClosed(command.Day))
        {
            return ValidationResult.Failed("Restaurant is closed on that day");
        }

        if (IsRestaurantAlreadyFullyBooked(command.Day, dbContext, numberOfTables))
        {
            return ValidationResult.Failed("Restaurant is fully booked");
        }

        return ValidationResult.Passed;
    }

    private static bool IsDateInPast(DateOnly day) => day < DateOnly.FromDateTime(DateTime.Now);

    private static bool IsRestaurantClosed(DateOnly date)
    {
        return date.DayOfWeek == DayOfWeek.Sunday
            || date.DayOfWeek == DayOfWeek.Monday;
    }

    private bool IsRestaurantAlreadyFullyBooked(DateOnly date, ReservationsContext dbContext, int numberOfTables)
    {
        var day = date.ToDateTime(TimeOnly.MinValue);
        var exisitingReservations = dbContext.Reservations.Where(x => x.Day == day);
        return exisitingReservations.Count() >= numberOfTables;
    }
}

public record ValidationResult(string? Error)
{
    public bool HasFailed => Error is not null;

    public static ValidationResult Failed(string error) => new(error);

    public static ValidationResult Passed => new((string?)null);
}
