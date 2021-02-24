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
    public class scheduleService : IScheduleService
    {
        private IControlDeviceRepository _controlDeviceRepository { get; init; }
        private ILogger<scheduleService> _logger { get; init; }
        private IScheduleRepository _scheduleRepository { get; init; }
        private IMapper _mapper { get; init; }
        private RelayService _relayService { get; init; }
        private DS18B20Service _ds18b20Service { get; init; }
        private FloatSensorService _floatSensorService{ get; init; }
        private IMeasurementService _measurementService { get; init; }

        private List<TaskAction> _TaskActionsList { get; init; }

        public scheduleService(IControlDeviceRepository controlDeviceRepository,
            ILogger<scheduleService> logger, 
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
            await _scheduleRepository.DeleteTaskScheduleAsync(id);
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedulesAsync()
        {
            return _mapper.Map<IEnumerable<Schedule>>(await _scheduleRepository.GetAllTaskSchedulesAsync());
        }

        public async Task<IEnumerable<Schedule>> GetDueSchedulesAsync()
        {
            return _mapper.Map<IEnumerable<Schedule>>(await _scheduleRepository.GetDueTaskSchedulesAsync());
        }

        public async Task<Schedule> GetScheduleAsync(int id)
        {
            return _mapper.Map<Schedule>(await _scheduleRepository.GetTaskScheduleAsync(id));
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

            Schedule insertedTaskSchedule;

            try
            {
                newScheduleEntity = await _scheduleRepository.InsertTaskScheduleAsync(newScheduleEntity);
                insertedTaskSchedule = _mapper.Map<Schedule>(newScheduleEntity);
            }
            catch (Exception)
            {

                throw;
            }
            

            return insertedTaskSchedule;
        }

        public async Task<Schedule> UpdateScheduleAsync(Schedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            // TODO: Return a useful validation message
            if (!ScheduleIsValid(schedule))
                throw new Exception();

            throw new NotImplementedException();
        }

        public async Task UpdateScheduleNextRunTimeAsync(int id, DateTime nextRunDateTime)
        {
            await _scheduleRepository.UpdateTaskScheduleNextRunTimeAsync(id, nextRunDateTime);
        }

        #endregion

        #region Schedule Tasks
        public Task<ScheduleTask> InsertScheduleTaskAsync(ScheduleTask scheduleTask)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Schedule>> GetSchedulesTasksAsync(int scheduleId)
        {
            throw new NotImplementedException();
        }

        public Task<ScheduleTask> GetScheduleTaskAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ScheduleTask> UpdateScheduleTaskAsync(ScheduleTask scheduleTask)
        {
            throw new NotImplementedException();
        }

        public Task DeleteScheduleTaskAsync(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Validate TaskSchedule
        private bool ScheduleIsValid(Schedule schedule)
        {
            var isValid = true;

            if (schedule.ScheduleTasks == null || !schedule.ScheduleTasks.Any())
                isValid = false;

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

            throw new NotImplementedException();
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
                MeasurementDateTime = temperatureReading.ReadingDateTime
            };

            await _measurementService.InsertMeasurementAsync(measurement);
        }

        private async Task FloatSensorReadingTask(int controlDeviceId)
        {
            throw new NotImplementedException();
        }

        public List<TaskAction> GetTaskActions()
        {
            return _TaskActionsList;
        }

        #endregion

    }
}
