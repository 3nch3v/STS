using AutoMapper;

using STS.Data.Models;
using STS.Web.ViewModels.Tasks;
using STS.Web.ViewModels.Tickets;

namespace STS.Web.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Status, StatusViewModel>();
            CreateMap<Priority, PriorityViewModel>(); 
            CreateMap<Department, DepartmentViewModel>(); 
            CreateMap<TicketInputModel, Ticket>();
            CreateMap<Ticket, TicketListViewModel>();
            CreateMap<Ticket, TicketViewModel>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<CommentInputModel, Comment>();
            CreateMap<EmployeeTask, BaseTaskViewModel>();
        }
    }
}
