using System;

namespace STS.Web.ViewModels.Tasks
{
    public class TaskEditModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? Deadline { get; set; }

        public int? PriorityId { get; set; }

        public int? StatusId { get; set; }

        public string EmployeeId { get; set; }
    }
}


//public int Id { get; set; }

//[Required]
//[StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
//public string Title { get; set; }

//[Required]
//[StringLength(TaskDescriptionMaxLength, MinimumLength = TaskDescriptionMinLength)]
//public string Description { get; set; }

//public DateTime Deadline { get; set; }

//[PriorityId]
//public int PriorityId { get; set; }

//[StatusId]
//public int StatusId { get; set; }

//[Required]
//public string EmployeeId { get; set; }