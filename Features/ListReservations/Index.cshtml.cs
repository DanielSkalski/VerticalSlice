using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VerticalSlice.Features.ListReservations
{
    public class IndexModel : PageModel
    {
        private readonly IMediator mediator;

        public IList<ReservationsListItemDto> Reservations { get; set; }

        public IndexModel(IMediator mediator)
        {
            this.mediator = mediator;
            Reservations = new List<ReservationsListItemDto>();
        }

        public async void OnGet()
        {
            var result = await mediator.Send(new GetReservationsQuery(DateOnly.Parse("22-07-2022")));

            Reservations = result.Items;
        }
    }
}
