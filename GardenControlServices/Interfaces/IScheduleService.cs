using GardenControlCore.Models;
using GardenControlCore.Scheduler;
using GardenControlRepositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices.Interfaces
{
    public interface IScheduleService
    {
        #region Schedule
        public Task<Schedule> InsertScheduleAsync(Schedule schedule);
        public Task<IEnumerable<Schedule>> GetAllSchedulesAsync();
        public Task<IEnumerable<Schedule>> GetDueSchedulesAsync();
        public Task<Schedule> GetScheduleAsync(int id);
        public Task<Schedule> UpdateScheduleAsync(Schedule schedule);
        public Task UpdateScheduleNextRunTimeAsync(int id, DateTime nextRunDateTime);
        public Task DeleteScheduleAsync(int id);
        #endregion

        #region ScheduleTasks
        public Task<ScheduleTask> InsertScheduleTaskAsync(ScheduleTask scheduleTask);
        public Task<IEnumerable<ScheduleTask>> GetAllScheduleTasksAsync();
        public Task<IEnumerable<ScheduleTask>> GetScheduleTasksAsync(int scheduleId);
        public Task<ScheduleTask> GetScheduleTaskAsync(int id);
        public Task<ScheduleTask> UpdateScheduleTaskAsync(ScheduleTask scheduleTask);
        public Task DeleteScheduleTaskAsync(int id);
        #endregion

        public List<TaskAction> GetTaskActions();
        public Task PerformScheduleTaskAction(int id);
    }
}
