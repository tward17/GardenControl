using GardenControlCore.Models;
using GardenControlCore.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices.Interfaces
{
    public interface ITaskScheduleService
    {
        #region TaskSchedule
        public Task<TaskSchedule> InsertTaskScheduleAsync(TaskSchedule taskSchedule);
        public Task<IEnumerable<TaskSchedule>> GetAllTaskSchedulesAsync();
        public Task<IEnumerable<TaskSchedule>> GetDueTaskSchedulesAsync();
        public Task<TaskSchedule> GetTaskScheduleAsync(int id);
        public Task<TaskSchedule> UpdateTaskScheduleAsync(TaskSchedule taskSchedule);
        public Task UpdateTaskScheduleNextRunTimeAsync(int id, DateTime nextRunDateTime);
        public Task DeleteTaskScheduleAsync(int id);
        #endregion

        public List<TaskAction> GetTaskActions();
        public Task PerformScheduleTaskAction(int id);
    }
}
