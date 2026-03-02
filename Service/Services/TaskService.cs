using DTO;
using Repository.Models;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Repository;

namespace Service.Services
{
    public class TaskService:ITaskService
    {
        private readonly IGenericRepository<MUser> _user;
        private readonly IGenericRepository<MTask> _task;
        private readonly IUserContextService _usercontextservice;
        private int userid;
        public TaskService(IUserContextService usercontextservice, IGenericRepository<MUser> user, IGenericRepository<MTask> task)
        {
            _usercontextservice = usercontextservice;
            userid= _usercontextservice.GetUserId();
                _user = user;
            _task = task;
        }
        public async Task<string> CreateTaskAsync(CreateTaskDto createtaskdto)
        {
            try
            {  //var taskdup = await _task.AnyAsync(x => x.Title == createtaskdto.Title);
               //if (taskdup) return "Already Task Exist";
                var task = new MTask
                {
                    Title = createtaskdto.Title,
                    Description = createtaskdto.Description,
                    StatusId = createtaskdto.StatusId,
                    PriorityId = createtaskdto.PriorityId,
                    AssignedTo = createtaskdto.AssignedTo,
                    DueDate = createtaskdto.DueDate,
                    CreatedBy = userid,
                    CreatedDate = DateTime.Now
                };
                if (task != null)
                {
                    await _task.AddAsync(task);
                    return "Task Created SuccessFully";
                }
                return "Failed to Create";
            }
            catch(Exception ex) { return ex.Message; }
        }
        public async Task<List<MTask>> GetallTasks()
        {
            try
            {
                var tasks = await _task.GetAllAsync();
                return tasks.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
