using Dapper;
using VerticalSlice.Data;

namespace VerticalSlice.Features.ListReservations;



public record GetReservationDetailsQuery(int ReservationId) : IRequest<GetReservationDetailsDto?>;

public record GetReservationDetailsDto(int ReservationId, string Name, string ContactEmail, DateTime Day);




public class GetReservationDetailsQueryHandler : IRequestHandler<GetReservationDetailsQuery, GetReservationDetailsDto?>
{
    private readonly ReservationsDapperContext dbContext;

    public GetReservationDetailsQueryHandler(ReservationsDapperContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<GetReservationDetailsDto?> Handle(GetReservationDetailsQuery request, CancellationToken cancellationToken)
    {
        using var connection = dbContext.CreateConnection();

        var result = await connection.QuerySingleOrDefaultAsync<GetReservationDetailsDto>(
            "SELECT Id AS ReservationId, Name, ContactEmail, Day FROM Reservations WHERE Id = @Id",
            new { Id = request.ReservationId });

        return result;
    }
}
