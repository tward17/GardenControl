using AutoMapper;
using GardenControlCore.Enums;
using GardenControlCore.Models;
using GardenControlCore.Scheduler;
using GardenControlRepositories.Entities;
using GardenControlRepositories.Interfaces;
using GardenControlServices.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices
{
    public class ScheduleService : IScheduleService
    {
        private IControlDeviceRepository _controlDeviceRepository { get; init; }
        private ILogger<ScheduleService> _logger { get; init; }
        private IScheduleRepository _scheduleRepository { get; init; }
        private IMapper _mapper { get; init; }
        private RelayService _relayService { get; init; }
        private DS18B20Service _ds18b20Service { get; init; }
        private FloatSensorService _floatSensorService{ get; init; }
        private IMeasurementService _measurementService { get; init; }

        private List<TaskAction> _TaskActionsList { get; init; }

        public ScheduleService(IControlDeviceRepository controlDeviceRepository,
            ILogger<ScheduleService> logger, 
            IScheduleRepository scheduleRepository, 
            IMapper mapper,
            RelayService relayService,
            DS18B20Service dS18B20Service,
            FloatSensorService floatSensorService,
            IMeasurementService measurementService)
        {
            _controlDeviceRepository = controlDeviceRepository;
            _logger = logger;
            _scheduleRepository = scheduleRepository;
            _mapper = mapper;
            _relayService = relayService;
            _ds18b20Service = dS18B20Service;
            _floatSensorService = floatSensorService;
            _measurementService = measurementService;

            _TaskActionsList = new List<TaskAction>
            {
                new TaskAction
                {
                    TaskActionId = TaskActionId.RelayOn,
                    Name = "Relay On",
                    DeviceType = DeviceType.Relay
                },
                new TaskAction
                {
                    TaskActionId = TaskActionId.RelayOff,
                    Name = "Relay Off",
                    DeviceType = DeviceType.Relay
                },
                new TaskAction
                {
                    TaskActionId = TaskActionId.RelayToggle,
                    Name = "Relay Toggle",
                    DeviceType = DeviceType.Relay
                },
                new TaskAction
                {
                    TaskActionId = TaskActionId.DS18B20Reading,
                    Name = "DS18B20 Measurement",
                    DeviceType = DeviceType.DS18B20
                },
                new TaskAction
                {
                    TaskActionId = TaskActionId.FloatSensorStateReading,
                    Name = "Float Sensor State",
                    DeviceType = DeviceType.FloatSensor
                }
            };
        }

        #region Schedule

        public async Task DeleteScheduleAsync(int id)
        {
            await _scheduleRepository.DeleteScheduleAsync(id);
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedulesAsync()
        {
            var scheduleEntities = await _scheduleRepository.GetAllSchedulesAsync();
            return _mapper.Map<IEnumerable<Schedule>>(scheduleEntities);
        }

        public async Task<IEnumerable<Schedule>> GetDueSchedulesAsync()
        {
            return _mapper.Map<IEnumerable<Schedule>>(await _scheduleRepository.GetDueSchedulesAsync());
        }

        public async Task<Schedule> GetScheduleAsync(int id)
        {
            return _mapper.Map<Schedule>(await _scheduleRepository.GetScheduleByIdAsync(id));
        }

        public async Task<Schedule> InsertScheduleAsync(Schedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            // TODO: Return a useful validation message
            if (!ScheduleIsValid(schedule))
                throw new Exception();

            var newScheduleEntity = new ScheduleEntity
            {
                Name = schedule.Name,
                IsActive = schedule.IsActive,
                TriggerTypeId = schedule.TriggerType,
                TriggerTimeOfDay = schedule.TriggerTimeOfDay,
                TriggerOffsetAmount = schedule.TriggerOffsetAmount,
                TriggerOffsetAmountTimeIntervalUnitId = schedule.TriggerOffsetAmountTimeIntervalUnit,
                IntervalAmount = schedule.IntervalAmount,
                IntervalAmountTimeIntervalUnitId = schedule.IntervalAmountTimeIntervalUnit,
                ScheduleTasks = new List<ScheduleTaskEntity>()
            };

            foreach (var task in schedule.ScheduleTasks)
            {
                var controlDevice = await _controlDeviceRepository.GetDeviceAsync(task.ControlDevice.ControlDeviceId);

                newScheduleEntity.ScheduleTasks.Add(new ScheduleTaskEntity
                {
                    Schedule = newScheduleEntity,
                    ControlDevice = controlDevice,
                    IsActive = task.IsActive,
                    TaskActionId = task.TaskAction.TaskActionId
                });
            }

            newScheduleEntity.NextRunDateTime = CalculateNextRunTime(schedule);

            Schedule insertedSchedule;

            try
            {
                newScheduleEntity = await _scheduleRepository.InsertScheduleAsync(newScheduleEntity);
                insertedSchedule = _mapper.Map<Schedule>(newScheduleEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting Schedule: {ex.Message}");
                throw;
            }
            

            return insertedSchedule;
        }

        public async Task<Schedule> UpdateScheduleAsync(Schedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            // TODO: Return a useful validation message
            if (!ScheduleIsValid(schedule))
                throw new Exception();

            // get the existing schedule entity from the database, including tasks
            var scheduleEntity = await _scheduleRepository.GetScheduleByIdAsync(schedule.ScheduleId);

            //TODO: Better exception
            if (scheduleEntity == null)
                throw new Exception();

            scheduleEntity.Name = schedule.Name;
            scheduleEntity.IsActive = schedule.IsActive;
            scheduleEntity.TriggerTypeId = schedule.TriggerType;
            scheduleEntity.TriggerTimeOfDay = schedule.TriggerTimeOfDay;
            scheduleEntity.TriggerOffsetAmount = schedule.TriggerOffsetAmount;
            scheduleEntity.TriggerOffsetAmountTimeIntervalUnitId = schedule.TriggerOffsetAmountTimeIntervalUnit;
            scheduleEntity.IntervalAmount = schedule.IntervalAmount;
            scheduleEntity.IntervalAmountTimeIntervalUnitId = schedule.IntervalAmountTimeIntervalUnit;
            scheduleEntity.NextRunDateTime = CalculateNextRunTime(schedule);

            // Update the schedule tasks

            // remove any tasks from the entity that are not present in the schedule object as they've been deleted
            foreach(var deletedTask in scheduleEntity.ScheduleTasks.Where(x => !schedule.ScheduleTasks.Select(y => y.ScheduleTaskId).ToList().Contains(x.ScheduleTaskId)).ToList())
            {
                scheduleEntity.ScheduleTasks.Remove(deletedTask);
            }

            // add any new tasks that are in the schedule but not in the scheduleEntity
            foreach(var newTask in schedule.ScheduleTasks.Where(x => !scheduleEntity.ScheduleTasks.Select(y => y.ScheduleTaskId).ToList().Contains(x.ScheduleTaskId)).ToList())
            {
                scheduleEntity.ScheduleTasks.Add(new ScheduleTaskEntity
                {
                    ControlDevice = await _controlDeviceRepository.GetDeviceAsync(newTask.ControlDevice.ControlDeviceId),
                    TaskActionId = newTask.TaskAction.TaskActionId,
                    IsActive = newTask.IsActive,
                    Schedule = scheduleEntity
                });
            }

            // check each task which has an id match between object and entity, and update the values
            foreach(var taskEntity in scheduleEntity.ScheduleTasks.Where(x => schedule.ScheduleTasks.Where(y => x.ScheduleTaskId == y.ScheduleTaskId).Any()))
            {
                var task = schedule.ScheduleTasks.Where(x => x.ScheduleTaskId == taskEntity.ScheduleTaskId).FirstOrDefault();
                taskEntity.ControlDevice = await _controlDeviceRepository.GetDeviceAsync(task.ControlDevice.ControlDeviceId);
                taskEntity.TaskActionId = task.TaskAction.TaskActionId;
                taskEntity.IsActive = task.IsActive;
            }
            
            try
            {
                await _scheduleRepository.UpdateScheduleAsync(scheduleEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Schedule: {schedule.ScheduleId}, {ex.Message}");
                throw;
            }

            return _mapper.Map<Schedule>(scheduleEntity);
        }

        public async Task UpdateScheduleNextRunTimeAsync(int id, DateTime nextRunDateTime)
        {
            await _scheduleRepository.UpdateScheduleNextRunTimeAsync(id, nextRunDateTime);
        }

        public async Task RunPendingSchedules()
        {
            var pendingSchedules = await GetDueSchedulesAsync();

            if (!pendingSchedules.Any())
                return;

            foreach (var schedule in pendingSchedules)
            {
                await RunSchedule(schedule);
            }
        }

        public async Task RunSchedule(Schedule schedule)
        {
            if (!schedule.ScheduleTasks.Where(x => x.IsActive).Any())
                return;

            foreach (var task in schedule.ScheduleTasks.Where(x => x.IsActive))
            {
                await PerformScheduleTaskAction(task.ScheduleTaskId);
            }

            // once tasks are completed, update the next run time value for the schedule
            await UpdateScheduleNextRunTimeAsync(schedule.ScheduleId, CalculateNextRunTime(schedule));
        }

        #endregion

        #region Schedule Tasks
        public async Task<ScheduleTask> InsertScheduleTaskAsync(ScheduleTask scheduleTask)
        {
            if (scheduleTask == null)
                throw new ArgumentNullException(nameof(scheduleTask));

            var scheduleEnity = await _scheduleRepository.GetScheduleByIdAsync(scheduleTask.ScheduleId);

            if (scheduleEnity == null)
                throw new InvalidOperationException();

            var controlDeviceEntity = await _controlDeviceRepository.GetDeviceAsync(scheduleTask.ControlDevice.ControlDeviceId);

            if (controlDeviceEntity == null)
                throw new InvalidOperationException();

            if (!_TaskActionsList.Select(ta => ta.TaskActionId).ToList().Contains(scheduleTask.TaskAction.TaskActionId))
                throw new InvalidOperationException();

            var scheduleTaskEntity = new ScheduleTaskEntity {
                Schedule = scheduleEnity,
                ControlDevice = controlDeviceEntity,
                TaskActionId = scheduleTask.TaskAction.TaskActionId,
                IsActive = scheduleTask.IsActive
            };

            try
            {
                await _scheduleRepository.InsertScheduleTaskAsync(scheduleTaskEntity);
            }
            catch (Exception)
            {

                throw;
            }

            return _mapper.Map<ScheduleTask>(scheduleTaskEntity);
        }

        public async Task<IEnumerable<ScheduleTask>> GetAllScheduleTasksAsync()
        {
            return _mapper.Map<IEnumerable<ScheduleTask>>(await _scheduleRepository.GetAllScheduleTasksAsync());
        }

        public async Task<IEnumerable<ScheduleTask>> GetScheduleTasksAsync(int scheduleId)
        {
            return _mapper.Map<IEnumerable<ScheduleTask>>(await _scheduleRepository.GetScheduleTasksAsync(scheduleId));
        }

        public async Task<ScheduleTask> GetScheduleTaskAsync(int id)
        {
            return _mapper.Map<ScheduleTask>(await _scheduleRepository.GetScheduleTaskByIdAsync(id));
        }

        public async Task<ScheduleTask> UpdateScheduleTaskAsync(ScheduleTask scheduleTask)
        {

            if (scheduleTask == null)
                throw new ArgumentNullException(nameof(scheduleTask));

            // get the existing schedule task entity from the database
            var scheduleTaskEntity = await _scheduleRepository.GetScheduleTaskByIdAsync(scheduleTask.ScheduleTaskId);

            if (scheduleTaskEntity == null)
                throw new Exception();

            scheduleTaskEntity.TaskActionId = scheduleTask.TaskAction.TaskActionId;
            scheduleTaskEntity.IsActive = scheduleTask.IsActive;

            return _mapper.Map<ScheduleTask>(scheduleTaskEntity);
        }

        public async Task DeleteScheduleTaskAsync(int id)
        {
            await _scheduleRepository.DeleteScheduleTaskAsync(id);
        }
        #endregion

        #region Validate Schedule
        private bool ScheduleIsValid(Schedule schedule)
        {
            var isValid = true;

            switch (schedule.TriggerType)
            {
                case GardenControlCore.Enums.TriggerType.TimeOfDay:
                    if (!schedule.TriggerTimeOfDay.HasValue)
                        isValid = false;
                    break;
                case GardenControlCore.Enums.TriggerType.Interval:
                    if (!schedule.IntervalAmount.HasValue)
                        isValid = false;

                    if (!schedule.IntervalAmountTimeIntervalUnit.HasValue)
                        isValid = false;

                    // Do not allow offsets for interval triggers
                    if (schedule.TriggerOffsetAmount.HasValue)
                        isValid = false;

                    break;
                case GardenControlCore.Enums.TriggerType.Sunrise:
                case GardenControlCore.Enums.TriggerType.Sunset:
                    // TODO: Check that a lat/long is set
                    break;
            }

            // trigger offset must have a specified interval unit
            if (schedule.TriggerOffsetAmount.HasValue && !schedule.TriggerOffsetAmountTimeIntervalUnit.HasValue)
                isValid = false;

            // interval amount must have a specified interval unit
            if (schedule.IntervalAmount.HasValue && !schedule.IntervalAmountTimeIntervalUnit.HasValue)
                isValid = false;

            return isValid;
        }
        #endregion

        #region ScheduleActions
        public async Task PerformScheduleTaskAction(int id)
        {
            var scheduleTask = await GetScheduleTaskAsync(id);

            if (scheduleTask == null)
                throw new Exception();

            switch (scheduleTask.TaskAction.TaskActionId)
            {
                case TaskActionId.RelayOn:
                    await RelayOnTask(scheduleTask.ControlDevice.ControlDeviceId);
                    break;
                case TaskActionId.RelayOff:
                    await RelayOffTask(scheduleTask.ControlDevice.ControlDeviceId);
                    break;
                case TaskActionId.RelayToggle:
                    await RelayToggleTask(scheduleTask.ControlDevice.ControlDeviceId);
                    break;
                case TaskActionId.DS18B20Reading:
                    await DS18B20ReadingTask(scheduleTask.ControlDevice.ControlDeviceId);
                    break;
                case TaskActionId.FloatSensorStateReading:
                    await FloatSensorReadingTask(scheduleTask.ControlDevice.ControlDeviceId);
                    break;
            }
        }

        /// <summary>
        /// Caclulates the next runtime for a task. Should not be used to get the next planned task run time.
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public DateTime CalculateNextRunTime(Schedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException();

            var nextRunTime = new DateTime();
            var currentDateTime = DateTime.Now;
            switch (schedule.TriggerType)
            {
                case GardenControlCore.Enums.TriggerType.TimeOfDay:
                    nextRunTime = currentDateTime.Date
                            .AddHours(schedule.TriggerTimeOfDay.Value.Hour)
                            .AddMinutes(schedule.TriggerTimeOfDay.Value.Minute);
                    
                    if (nextRunTime < currentDateTime)
                    {
                        // Already passed trigger time for today, so set for tomorrow
                        nextRunTime = nextRunTime.AddDays(1);
                    }
                    
                    break;
                case GardenControlCore.Enums.TriggerType.Interval:

                    if(schedule.NextRunDateTime != DateTime.MinValue && schedule.NextRunDateTime <= currentDateTime)
                    {
                        nextRunTime = GetAdjustedTime(schedule.NextRunDateTime, schedule.IntervalAmount.Value, schedule.IntervalAmountTimeIntervalUnit.Value);
                    }
                    else if(schedule.NextRunDateTime > currentDateTime)
                    {
                        nextRunTime = schedule.NextRunDateTime;
                    }
                    else
                    {
                        nextRunTime = GetAdjustedTime(currentDateTime, schedule.IntervalAmount.Value, schedule.IntervalAmountTimeIntervalUnit.Value);
                    }

                    break;
                case GardenControlCore.Enums.TriggerType.Sunrise:
                    // TODO: implement sunrise check
                    // Get Sunrise for today
                    // If Sunrise today is in the past
                        // Get Sunrise for tomorrow and set that value
                    // else
                        // Set sunrise for today as the value
                    break;
                case GardenControlCore.Enums.TriggerType.Sunset:
                    // TODO: implement sunset check
                    // Get Sunset for today
                    // If Sunset today is in the past
                        // Get Sunset for tomorrow and set that value
                    // else
                        // Set sunset for today as the value
                    break;
            }

            // Adjust next run time for any offsets
            if (schedule.TriggerOffsetAmount.HasValue)
            {
                nextRunTime = GetAdjustedTime(nextRunTime, schedule.TriggerOffsetAmount.Value, schedule.TriggerOffsetAmountTimeIntervalUnit.Value);
            }

            return nextRunTime;
        }

        private DateTime GetAdjustedTime(DateTime inTime, int interval, TimeIntervalUnit timeInterval)
        {
            var outTime = inTime;

            switch (timeInterval)
            {
                case TimeIntervalUnit.Seconds:
                    outTime = outTime.AddSeconds(interval);
                    break;
                case TimeIntervalUnit.Minutes:
                    outTime = outTime.AddMinutes(interval);
                    break;
                case TimeIntervalUnit.Hours:
                    outTime = outTime.AddHours(interval);
                    break;
                case TimeIntervalUnit.Days:
                    outTime = outTime.AddDays(interval);
                    break;
            }

            return outTime;
        }

        #endregion

        #region Task Actions

        private async Task RelayOnTask(int controlDeviceId)
        {
            await _relayService.SetRelayState(controlDeviceId, RelayState.On);
        }

        private async Task RelayOffTask(int controlDeviceId)
        {
            await _relayService.SetRelayState(controlDeviceId, RelayState.Off);
        }

        private async Task RelayToggleTask(int controlDeviceId)
        {
            await _relayService.ToggleRelayState(controlDeviceId);
        }

        private async Task DS18B20ReadingTask(int controlDeviceId)
        {
            var temperatureReading = await _ds18b20Service.GetTemperatureReading(controlDeviceId);

            var measurement = new Measurement
            {
                ControlDeviceId = controlDeviceId,
                MeasurementValue = temperatureReading.TemperatureC,
                MeasurementDateTime = temperatureReading.ReadingDateTime,
                MeasurementUnit = GardenControlCore.Enums.MeasurementUnit.Celcius
            };

            await _measurementService.InsertMeasurementAsync(measurement);
        }

        private async Task FloatSensorReadingTask(int controlDeviceId)
        {
            var floatStateReading = await _floatSensorService.GetFloatSensorState(controlDeviceId);

            var measurement = new Measurement
            {
                ControlDeviceId = controlDeviceId,
                MeasurementValue = (floatStateReading == FloatSensorState.High ? 1 : 0),
                MeasurementDateTime = DateTime.Now,
                MeasurementUnit = GardenControlCore.Enums.MeasurementUnit.Boolean
            };

            await _measurementService.InsertMeasurementAsync(measurement);
        }

        public List<TaskAction> GetTaskActions()
        {
            return _TaskActionsList;
        }

        
        #endregion

    }
}
