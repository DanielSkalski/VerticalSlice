using VerticalSlice.Data;

namespace VerticalSlice.Features.CancelReservation
{
    public record CancelReservationCommand(int ReservationId) : IRequest;

    public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand>
    {
        private readonly ReservationsContext reservationsContext;

        public CancelReservationCommandHandler(ReservationsContext reservationsContext)
        {
            this.reservationsContext = reservationsContext;
        }

        public async Task<Unit> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
        {
            var reservation = await reservationsContext.Reservations.FindAsync(request.ReservationId);
            if (reservation is not null)
            {
                reservationsContext.Remove(reservation);
            }

            return Unit.Value;
        }
    }
}
