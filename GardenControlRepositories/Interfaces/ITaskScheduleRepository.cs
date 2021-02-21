using GardenControlRepositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Interfaces
{
    public interface ITaskScheduleRepository
    {
        //#region TaskAction
        //public Task<TaskActionEntity> InsertTaskActionAsync(TaskActionEntity taskAction);
        //public Task<IEnumerable<TaskActionEntity>> GetAllTaskActionsAsync();
        //public Task<TaskActionEntity> GetTaskActionAsync(int id);
        //public Task<TaskActionEntity> UpdateTaskActionAsync(TaskActionEntity taskAction);
        //public Task DeleteTaskActionAsync(int id);
        //#endregion

        #region TaskSchedule
        public Task<TaskScheduleEntity> InsertTaskScheduleAsync(TaskScheduleEntity taskSchedule);
        public Task<IEnumerable<TaskScheduleEntity>> GetAllTaskSchedulesAsync();
        public Task<IEnumerable<TaskScheduleEntity>> GetDueTaskSchedulesAsync();
        public Task<TaskScheduleEntity> GetTaskScheduleAsync(int id);
        public Task<TaskScheduleEntity> UpdateTaskScheduleAsync(TaskScheduleEntity taskSchedule);
        public Task UpdateTaskScheduleNextRunTimeAsync(int id, DateTime nextRunDateTime);
        public Task DeleteTaskScheduleAsync(int id);
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
