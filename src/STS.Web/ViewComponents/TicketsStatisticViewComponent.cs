using Microsoft.AspNetCore.Mvc;

using STS.Services.Contracts;

namespace STS.Web.ViewComponents
{
    public class TicketsStatisticViewComponent : ViewComponent
    {
        private readonly ICommonService commonService;

        public TicketsStatisticViewComponent(ICommonService commonService)
        {
            this.commonService = commonService;
        }

        public IViewComponentResult Invoke()
        {
            var departmentsDto = commonService.GetTicketsStatistic();

            return View(departmentsDto);
        }
    }
}
