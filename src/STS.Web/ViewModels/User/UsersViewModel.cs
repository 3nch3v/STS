using System;
using System.Collections.Generic;

using STS.Web.ViewModels.Common;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewModels.User
{
    public class UsersViewModel
    {
        public string Keyword { get; set; }

        public int? DepartmentId { get; set; }

        public int UsersCount { get; set; }

        public int PagesCount => (int)Math.Ceiling(UsersCount * 1.0 / UsersPerPage);

        public int Page { get; set; }

        public int PreviousPage => Page - 1;

        public int NextPage => Page + 1;

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < PagesCount;

        public IEnumerable<UserViewModel> Users { get; set; }

        public IEnumerable<DepartmentViewModel> Departments { get; set; }
    }
}
