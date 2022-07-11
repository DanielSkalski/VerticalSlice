using Dapper;
using VerticalSlice.Data;

namespace VerticalSlice.Features.ListReservations;

public record GetReservationsQuery(DateOnly Day) : IRequest<ReservationsListDto>;

public class GetReservationsQueryHandler : IRequestHandler<GetReservationsQuery, ReservationsListDto>
{
    private readonly ReservationsDapperContext dbContext;

    public GetReservationsQueryHandler(ReservationsDapperContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ReservationsListDto> Handle(GetReservationsQuery request, CancellationToken cancellationToken)
    {
        using var connection = dbContext.CreateConnection();
        
        var sql = "SELECT Id, Name FROM Reservations";
        var reservations = await connection.QueryAsync<ReservationsListItemDto>(sql, cancellationToken);

        var result = new ReservationsListDto(reservations.ToList());

        return result;
    }
}

public record ReservationsListItemDto(int Id, string Name);
public record ReservationsListDto(List<ReservationsListItemDto> Items);
