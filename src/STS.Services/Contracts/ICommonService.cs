using System.Collections.Generic;
using System.Threading.Tasks;

using STS.Data.Dtos.Department;
using STS.Data.Dtos.Role;
using STS.Data.Dtos.User;
using STS.Data.Models;

namespace STS.Services.Contracts
{
    public interface ICommonService
    {
        int GetStatusId(string status);

        int GetDepartmentId(string userId);

        Task<bool> IsDepartmentExisting(int id);

        Dictionary<string, Dictionary<string, int>> GetTicketsStatistic();

        IEnumerable<Priority> GetPriorities();

        IEnumerable<Status> GetStatuses();

        IEnumerable<RoleDto> GetRoles();

        IEnumerable<DepartmentBaseDto> GetDepartmentsBase();

        IEnumerable<DepartmentStatisticDto> GetDepartmentsWithStatistic();

        IEnumerable<BaseUserDto> GetEmployeesBase(int departmentId);
    }
}
