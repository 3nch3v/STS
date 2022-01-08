using System.Collections.Generic;

using STS.Web.ViewModels.User;

namespace STS.Web.ViewModels.Tasks
{
    public class ManagerTaskSearchViewModel
    {
        public string Keyword { get; set; }

        public IEnumerable<BaseUserViewModel> Employees { get; set; }
    }
}
