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
    public class TaskScheduleService : ITaskScheduleService
    {
        private IControlDeviceRepository _controlDeviceRepository { get; init; }
        private ILogger<TaskScheduleService> _logger { get; init; }
        private ITaskScheduleRepository _taskScheduleRepository { get; init; }
        private IMapper _mapper { get; init; }
        private RelayService _relayService { get; init; }
        private DS18B20Service _ds18b20Service { get; init; }
        private FloatSensorService _floatSensorService{ get; init; }
        private IMeasurementService _measurementService { get; init; }

        private List<TaskAction> _TaskActionsList { get; init; }

        public TaskScheduleService(IControlDeviceRepository controlDeviceRepository,
            ILogger<TaskScheduleService> logger, 
            ITaskScheduleRepository taskScheduleRepository, 
            IMapper mapper,
            RelayService relayService,
            DS18B20Service dS18B20Service,
            FloatSensorService floatSensorService,
            IMeasurementService measurementService)
        {
            _controlDeviceRepository = controlDeviceRepository;
            _logger = logger;
            _taskScheduleRepository = taskScheduleRepository;
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

        #region TaskSchedule

        public async Task DeleteTaskScheduleAsync(int id)
        {
            await _taskScheduleRepository.DeleteTaskScheduleAsync(id);
        }

        public async Task<IEnumerable<TaskSchedule>> GetAllTaskSchedulesAsync()
        {
            return _mapper.Map<IEnumerable<TaskSchedule>>(await _taskScheduleRepository.GetAllTaskSchedulesAsync());
        }

        public async Task<IEnumerable<TaskSchedule>> GetDueTaskSchedulesAsync()
        {
            return _mapper.Map<IEnumerable<TaskSchedule>>(await _taskScheduleRepository.GetDueTaskSchedulesAsync());
        }

        public async Task<TaskSchedule> GetTaskScheduleAsync(int id)
        {
            return _mapper.Map<TaskSchedule>(await _taskScheduleRepository.GetTaskScheduleAsync(id));
        }

        public async Task<TaskSchedule> InsertTaskScheduleAsync(TaskSchedule taskSchedule)
        {
            if (taskSchedule == null)
                throw new ArgumentNullException(nameof(taskSchedule));

            // TODO: Return a useful validation message
            if (!TaskScheduleIsValid(taskSchedule))
                throw new Exception();

            var newTaskScheduleEntity = _mapper.Map<TaskScheduleEntity>(taskSchedule);

            newTaskScheduleEntity.ControlDevice = await _controlDeviceRepository.GetDeviceAsync(newTaskScheduleEntity.ControlDeviceId);

            newTaskScheduleEntity.NextRunDateTime = CalculateNextRunTime(taskSchedule);

            TaskSchedule insertedTaskSchedule;

            try
            {
                newTaskScheduleEntity = await _taskScheduleRepository.InsertTaskScheduleAsync(newTaskScheduleEntity);
                insertedTaskSchedule = _mapper.Map<TaskSchedule>(newTaskScheduleEntity);
            }
            catch (Exception)
            {

                throw;
            }
            

            return insertedTaskSchedule;
        }

        public async Task<TaskSchedule> UpdateTaskScheduleAsync(TaskSchedule taskSchedule)
        {
            if (taskSchedule == null)
                throw new ArgumentNullException(nameof(taskSchedule));

            // TODO: Return a useful validation message
            if (!TaskScheduleIsValid(taskSchedule))
                throw new Exception();

            throw new NotImplementedException();
        }

        public async Task UpdateTaskScheduleNextRunTimeAsync(int id, DateTime nextRunDateTime)
        {
            await _taskScheduleRepository.UpdateTaskScheduleNextRunTimeAsync(id, nextRunDateTime);
        }

        #endregion

        #region Validate TaskSchedule
        private bool TaskScheduleIsValid(TaskSchedule taskSchedule)
        {
            var isValid = true;

            switch (taskSchedule.TriggerType)
            {
                case GardenControlCore.Enums.TriggerType.TimeOfDay:
                    if (!taskSchedule.TriggerTimeOfDay.HasValue)
                        isValid = false;
                    break;
                case GardenControlCore.Enums.TriggerType.Interval:
                    if (!taskSchedule.IntervalAmount.HasValue)
                        isValid = false;

                    if (!taskSchedule.IntervalAmountTimeIntervalUnit.HasValue)
                        isValid = false;

                    // Do not allow offsets for interval triggers
                    if (taskSchedule.TriggerOffsetAmount.HasValue)
                        isValid = false;

                    break;
                case GardenControlCore.Enums.TriggerType.Sunrise:
                case GardenControlCore.Enums.TriggerType.Sunset:
                    // TODO: Check that a lat/long is set
                    break;
            }

            // trigger offset must have a specified interval unit
            if (taskSchedule.TriggerOffsetAmount.HasValue && !taskSchedule.TriggerOffsetAmountTimeIntervalUnit.HasValue)
                isValid = false;

            // interval amount must have a specified interval unit
            if (taskSchedule.IntervalAmount.HasValue && !taskSchedule.IntervalAmountTimeIntervalUnit.HasValue)
                isValid = false;

            return isValid;
        }
        #endregion

        #region ScheduleActions
        public async Task PerformScheduleTaskAction(int id)
        {
            var taskSchedule = await GetTaskScheduleAsync(id);

            if (taskSchedule == null)
                throw new Exception();

            switch ((TaskActionId)taskSchedule.TaskAction.TaskActionId)
            {
                case TaskActionId.RelayOn:
                    await RelayOnTask(taskSchedule.ControlDevice.ControlDeviceId);
                    break;
                case TaskActionId.RelayOff:
                    await RelayOffTask(taskSchedule.ControlDevice.ControlDeviceId);
                    break;
                case TaskActionId.RelayToggle:
                    await RelayToggleTask(taskSchedule.ControlDevice.ControlDeviceId);
                    break;
                case TaskActionId.DS18B20Reading:
                    await DS18B20ReadingTask(taskSchedule.ControlDevice.ControlDeviceId);
                    break;
                case TaskActionId.FloatSensorStateReading:
                    await FloatSensorReadingTask(taskSchedule.ControlDevice.ControlDeviceId);
                    break;
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Caclulates the next runtime for a task. Should not be used to get the next planned task run time.
        /// </summary>
        /// <param name="taskSchedule"></param>
        /// <returns></returns>
        public DateTime CalculateNextRunTime(TaskSchedule taskSchedule)
        {
            if (taskSchedule == null)
                throw new ArgumentNullException();

            var nextRunTime = new DateTime();
            var currentDateTime = DateTime.Now;
            switch (taskSchedule.TriggerType)
            {
                case GardenControlCore.Enums.TriggerType.TimeOfDay:
                    nextRunTime = currentDateTime.Date
                            .AddHours(taskSchedule.TriggerTimeOfDay.Value.Hour)
                            .AddMinutes(taskSchedule.TriggerTimeOfDay.Value.Minute);
                    
                    if (nextRunTime < currentDateTime)
                    {
                        // Already passed trigger time for today, so set for tomorrow
                        nextRunTime = nextRunTime.AddDays(1);
                    }
                    
                    break;
                case GardenControlCore.Enums.TriggerType.Interval:

                    if(taskSchedule.NextRunDateTime != DateTime.MinValue && taskSchedule.NextRunDateTime <= currentDateTime)
                    {
                        nextRunTime = GetAdjustedTime(taskSchedule.NextRunDateTime, taskSchedule.IntervalAmount.Value, taskSchedule.IntervalAmountTimeIntervalUnit.Value);
                    }
                    else if(taskSchedule.NextRunDateTime > currentDateTime)
                    {
                        nextRunTime = taskSchedule.NextRunDateTime;
                    }
                    else
                    {
                        nextRunTime = GetAdjustedTime(currentDateTime, taskSchedule.IntervalAmount.Value, taskSchedule.IntervalAmountTimeIntervalUnit.Value);
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
            if (taskSchedule.TriggerOffsetAmount.HasValue)
            {
                nextRunTime = GetAdjustedTime(nextRunTime, taskSchedule.TriggerOffsetAmount.Value, taskSchedule.TriggerOffsetAmountTimeIntervalUnit.Value);
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
