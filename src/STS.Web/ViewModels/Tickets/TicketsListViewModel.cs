using System;
using System.Collections.Generic;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.Tickets
{
    public class TicketsListViewModel
    {
        public int TicketsCount { get; set; }

        public int PagesCount => (int)Math.Ceiling(TicketsCount * 1.0 / TicketsPerPage);

        public int Page { get; set; }

        public int PreviousPage => Page - 1;

        public int NextPage => Page + 1;

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < PagesCount;

        public string Keyword { get; set; }

        public IEnumerable<BaseTicketViewModel> Tickets { get; set; }
    }
}
