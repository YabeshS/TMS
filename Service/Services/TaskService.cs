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
            {  
                var assigneduser= await _task.GetAllAsync(x=>x.AssignedTo==createtaskdto.AssignedTo&&x.DueDate>DateTime.Now);
                if (assigneduser.Any())
                {
                   throw new Exception($"Already assigned to user");
                }
                var task = new MTask
                {
                    Title = createtaskdto.Title,
                    Description = createtaskdto.Description,
                    StatusId = createtaskdto.StatusId,
                    PriorityId = createtaskdto.PriorityId,
                    AssignedTo = createtaskdto.AssignedTo,
                    DueDate = createtaskdto.DueDate == null ? DateTime.Now.AddHours(1) : createtaskdto.DueDate,
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
        public async Task<string> UpdateTaskAsync(UpdateTaskDto updatetaskdto)
        {
            try
            {
                var task = await _task.GetByIdAsync(updatetaskdto.Taskid);
                if (task == null) throw new Exception("user not found");
                task.Title = updatetaskdto.Title;
                task.Description = updatetaskdto.Description;
                task.StatusId = updatetaskdto.StatusId;
                task.PriorityId = updatetaskdto.PriorityId;
                task.AssignedTo = updatetaskdto.AssignedTo;
                task.DueDate = updatetaskdto.DueDate;
                task.UpdatedDate = DateTime.Now;
                task.UPdatedbY = userid;
                await _task.UpdateAsync(task);
                return "Task Updated Success";
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}
