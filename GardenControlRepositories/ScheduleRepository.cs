using GardenControlRepositories.Entities;
using GardenControlRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private GardenControlContext _context { get; init; }
        private ILogger<ScheduleRepository> _logger { get; init; }

        public ScheduleRepository(GardenControlContext context, ILogger<ScheduleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task DeleteTaskScheduleAsync(int id)
        {
            var taskScheduleEntity = await _context.TaskScheduleEntities
                .Where(t => t.ScheduleId == id).FirstOrDefaultAsync();

            if (taskScheduleEntity == null)
            {
                _logger.LogWarning($"Tried deleting TaskSchedule that does not exist, TaskScheduleId: {id}");
                return;
            }

            try
            {
                _context.TaskScheduleEntities.Remove(taskScheduleEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting TaskSchedule ({id}): {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ScheduleEntity>> GetAllTaskSchedulesAsync()
        {
            var taskScheduleEntities = await _context.TaskScheduleEntities
                .ToListAsync();

            return taskScheduleEntities;
        }

        public async Task<IEnumerable<ScheduleEntity>> GetDueTaskSchedulesAsync()
        {
            var taskScheduleEntities = await _context.TaskScheduleEntities
                .Where(t => t.IsActive && t.NextRunDateTime <= DateTime.Now)
                .ToListAsync();

            return taskScheduleEntities;
        }

        public async Task<ScheduleEntity> GetTaskScheduleAsync(int id)
        {
            var taskScheduleEntity = await _context.TaskScheduleEntities
                .Where(t => t.ScheduleId == id)
                .FirstOrDefaultAsync();

            return taskScheduleEntity;
        }

        public async Task<ScheduleEntity> InsertTaskScheduleAsync(ScheduleEntity taskSchedule)
        {
            if (taskSchedule == null) throw new ArgumentNullException(nameof(taskSchedule));

            try
            {
                _context.TaskScheduleEntities.Add(taskSchedule);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting TaskSchedule: {ex.Message}");
                throw;
            }

            return taskSchedule;
        }

        public async Task<ScheduleEntity> UpdateTaskScheduleAsync(ScheduleEntity taskSchedule)
        {
            var taskScheduleEntity = await _context.TaskScheduleEntities
                .Where(t => t.ScheduleId == taskSchedule.ScheduleId)
                .FirstOrDefaultAsync();

            if (taskScheduleEntity == null)
                throw new Exception();

            taskScheduleEntity.Name = taskSchedule.Name;
            taskScheduleEntity.TriggerTypeId = taskSchedule.TriggerTypeId;
            taskScheduleEntity.TriggerTimeOfDay = taskSchedule.TriggerTimeOfDay;
            taskScheduleEntity.TriggerOffsetAmount = taskSchedule.TriggerOffsetAmount;
            taskScheduleEntity.TriggerOffsetAmountTimeIntervalUnitId = taskSchedule.TriggerOffsetAmountTimeIntervalUnitId;
            taskScheduleEntity.IntervalAmount = taskSchedule.IntervalAmount;
            taskScheduleEntity.IntervalAmountTimeIntervalUnitId = taskSchedule.IntervalAmountTimeIntervalUnitId;
            taskScheduleEntity.IsActive = taskSchedule.IsActive;
            taskScheduleEntity.NextRunDateTime = taskSchedule.NextRunDateTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating TaskSchedule: {taskSchedule.ScheduleId}, {ex.Message}");
                throw;
            }

            return taskScheduleEntity;
        }

        public async Task UpdateTaskScheduleNextRunTimeAsync(int id, DateTime nextRunDateTime)
        {
            var taskScheduleEntity = await _context.TaskScheduleEntities
                .Where(t => t.ScheduleId == id)
                .FirstOrDefaultAsync();

            if (taskScheduleEntity == null)
                throw new Exception();

            taskScheduleEntity.NextRunDateTime = nextRunDateTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating TaskSchedule NextRunTime: {id}, {ex.Message}");
                throw;
            }
        }
    }
}
