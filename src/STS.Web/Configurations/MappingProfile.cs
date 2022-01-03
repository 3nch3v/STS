using AutoMapper;

using STS.Data.Dtos;
using STS.Data.Dtos.Department;
using STS.Data.Dtos.Role;
using STS.Data.Dtos.Task;
using STS.Data.Dtos.Ticket;
using STS.Data.Dtos.User;

using STS.Data.Models;
using STS.Web.ViewModels.Admin;
using STS.Web.ViewModels.Comment;
using STS.Web.ViewModels.Common;
using STS.Web.ViewModels.Tasks;
using STS.Web.ViewModels.Tickets;
using STS.Web.ViewModels.User;

namespace STS.Web.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Status, StatusViewModel>();
            CreateMap<Priority, PriorityViewModel>(); 
            CreateMap<DepartmentBaseDto, DepartmentViewModel>();
            CreateMap<RoleDto, RoleViewModel>();
            CreateMap<DepartmentStatisticDto, DepartmentStatisticViewModel>();

            CreateMap<TicketInputModel, Ticket>();
            CreateMap<TicketListingDto, BaseTicketViewModel>();
            CreateMap<Ticket, TicketViewModel>();
            CreateMap<TicketEditModel, TicketDto>();

            CreateMap<Comment, CommentViewModel>();
            CreateMap<CommentInputModel, Comment>();

            CreateMap<BaseUserDto, BaseUserViewModel>();
            CreateMap<UserInputModel, UserInputDto>();
            CreateMap<UserEditModel, UserEditDto>();
            CreateMap<UserDto, UserViewModel>();
            CreateMap<ApplicationUser, UserEditModel>();

            CreateMap<TaskListingDto, TaskLinstingViewModel>();
            CreateMap<TaskInputModel, EmployeeTask>();
            CreateMap<BaseTaskDto, BaseTaskViewModel>();
            CreateMap<TaskListingDto, TaskViewModel>();
            CreateMap<TaskDto, TaskViewModel>();

            CreateMap<ReplyTask, ReplayTaskDto>();
            CreateMap<ReplayTaskDto, ReplayTaskViewModel>();

        }
    }
}
