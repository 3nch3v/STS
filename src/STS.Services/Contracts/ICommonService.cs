﻿using System.Collections.Generic;

using STS.Data.Models;

namespace STS.Services.Contracts
{
    public interface ICommonService
    {
        IEnumerable<Priority> GetPriorities();

        IEnumerable<Status> GetStatuses();

        IEnumerable<Department> GetDepartments();

        IEnumerable<ApplicationUser> GetEmployees(int departmentId);

        int GetStatusId(string status);
    }
}
