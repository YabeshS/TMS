using DTO;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ITaskService
    {
        Task<string> CreateTaskAsync(CreateTaskDto createtaskdto);
        Task<List<MTask>> GetallTasks();
    }
}
