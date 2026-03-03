using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CreateTaskDto
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public int StatusId { get; set; }

        public int PriorityId { get; set; }

        public int AssignedTo { get; set; }

        public DateTime? DueDate { get; set; } 

    }
    public class UpdateTaskDto
    {
        public int Taskid { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int StatusId { get; set; }

        public int PriorityId { get; set; }

        public int AssignedTo { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UPdatedbY { get; set; }

    }
}
