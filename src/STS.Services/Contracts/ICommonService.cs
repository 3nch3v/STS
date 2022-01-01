using System.Collections.Generic;
using STS.Data.Dtos.Department;
using STS.Data.Dtos.Role;
using STS.Data.Dtos.User;
using STS.Data.Models;

namespace STS.Services.Contracts
{
    public interface ICommonService
    {
        IEnumerable<Priority> GetPriorities();

        IEnumerable<Status> GetStatuses();

        IEnumerable<RoleDto> GetRoles();

        IEnumerable<DepartmentBaseDto> GetDepartmentsBase();

        IEnumerable<DepartmentStatisticDto> GetDepartmentsWithStatistic();

        IEnumerable<BaseUserDto> GetEmployeesBase(int departmentId);

        Dictionary<string, Dictionary<string, int>> GetTicketsStatistic();

        int GetStatusId(string status);

        int GetDepartmentId(string userId);
    }
}
