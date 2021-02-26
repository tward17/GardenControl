using GardenControlRepositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Interfaces
{
    public interface IScheduleRepository
    {
        //#region TaskAction
        //public Task<TaskActionEntity> InsertTaskActionAsync(TaskActionEntity taskAction);
        //public Task<IEnumerable<TaskActionEntity>> GetAllTaskActionsAsync();
        //public Task<TaskActionEntity> GetTaskActionAsync(int id);
        //public Task<TaskActionEntity> UpdateTaskActionAsync(TaskActionEntity taskAction);
        //public Task DeleteTaskActionAsync(int id);
        //#endregion

        #region Schedule
        public Task<ScheduleEntity> InsertScheduleAsync(ScheduleEntity schedule);
        public Task<IEnumerable<ScheduleEntity>> GetAllSchedulesAsync();
        public Task<IEnumerable<ScheduleEntity>> GetDueSchedulesAsync();
        public Task<ScheduleEntity> GetScheduleByIdAsync(int id);
        public Task<ScheduleEntity> UpdateScheduleAsync(ScheduleEntity schedule);
        public Task UpdateScheduleNextRunTimeAsync(int id, DateTime nextRunDateTime);
        public Task DeleteScheduleAsync(int id);
        #endregion

        #region Schedule Task
        public Task<ScheduleTaskEntity> InsertScheduleTaskAsync(ScheduleTaskEntity scheduleTask);
        public Task<IEnumerable<ScheduleTaskEntity>> GetAllScheduleTasksAsync();
        public Task<IEnumerable<ScheduleTaskEntity>> GetScheduleTasksAsync(int scheduleTaskId);
        public Task<ScheduleTaskEntity> GetScheduleTaskByIdAsync(int id);
        public Task<ScheduleTaskEntity> UpdateScheduleTaskAsync(ScheduleTaskEntity scheduleTask);
        public Task DeleteScheduleTaskAsync(int id);
        #endregion

        //#region TimeInterval
        //public Task<TimeIntervalEntity> InsertTimeIntervalAsync(TimeIntervalEntity timeInterval);
        //public Task<IEnumerable<TimeIntervalEntity>> GetAllTimeIntervalsAsync();
        //public Task<TimeIntervalEntity> GetTimeIntervalAsync(int id);
        //public Task<TimeIntervalEntity> UpdateTimeIntervalAsync(TimeIntervalEntity timeInterval);
        //public Task DeleteTimeIntervalAsync(int id);
        //#endregion

        //#region TriggerType
        //public Task<TriggerTypeEntity> InsertTriggerTypeAsync(TriggerTypeEntity taskSchedule);
        //public Task<IEnumerable<TriggerTypeEntity>> GetAllTriggerTypeAsync();
        //public Task<TriggerTypeEntity> GetTriggerTypeAsync(int id);
        //public Task<TriggerTypeEntity> UpdateTriggerTypeAsync(TriggerTypeEntity taskSchedule);
        //public Task DeleteTriggerTypeAsync(int id);
        //#endregion
    }
}
