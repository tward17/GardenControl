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

        #region Schedule
        public async Task DeleteScheduleAsync(int id)
        {
            var scheduleEntity = await _context.ScheduleEntities
                .Where(t => t.ScheduleId == id).FirstOrDefaultAsync();

            if (scheduleEntity == null)
            {
                _logger.LogWarning($"Tried deleting Schedule that does not exist, ScheduleId: {id}");
                return;
            }

            try
            {
                _context.ScheduleEntities.Remove(scheduleEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting Schedule ({id}): {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ScheduleEntity>> GetAllSchedulesAsync()
        {
            var taskScheduleEntities = await _context.ScheduleEntities
                .Include(s => s.ScheduleTasks)
                .ThenInclude(st => st.ControlDevice)
                .ToListAsync();

            return taskScheduleEntities;
        }

        public async Task<IEnumerable<ScheduleEntity>> GetDueSchedulesAsync()
        {
            var taskScheduleEntities = await _context.ScheduleEntities
                .Include(s => s.ScheduleTasks)
                .ThenInclude(st => st.ControlDevice)
                .Where(t => t.IsActive && t.NextRunDateTime <= DateTime.Now)
                .ToListAsync();

            return taskScheduleEntities;
        }

        public async Task<ScheduleEntity> GetScheduleByIdAsync(int id)
        {
            var scheduleEntity = await _context.ScheduleEntities
                .Include(s => s.ScheduleTasks)
                .ThenInclude(st => st.ControlDevice)
                .Where(t => t.ScheduleId == id)
                .FirstOrDefaultAsync();

            return scheduleEntity;
        }

        public async Task<ScheduleEntity> InsertScheduleAsync(ScheduleEntity schedule)
        {
            if (schedule == null) throw new ArgumentNullException(nameof(schedule));

            try
            {
                _context.ScheduleEntities.Add(schedule);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting Schedule: {ex.Message}");
                throw;
            }

            return schedule;
        }

        public async Task<ScheduleEntity> UpdateScheduleAsync(ScheduleEntity schedule)
        {
            try
            {
                _context.Entry(schedule).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Schedule: {schedule.ScheduleId}, {ex.Message}");
                throw;
            }

            return schedule;
        }

        public async Task UpdateScheduleNextRunTimeAsync(int id, DateTime nextRunDateTime)
        {
            var scheduleEntity = await _context.ScheduleEntities
                .Where(t => t.ScheduleId == id)
                .FirstOrDefaultAsync();

            if (scheduleEntity == null)
                throw new Exception();

            scheduleEntity.NextRunDateTime = nextRunDateTime;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating Schedule NextRunTime: {id}, {ex.Message}");
                throw;
            }
        }
        #endregion

        #region Schedule Task
        public async Task<ScheduleTaskEntity> InsertScheduleTaskAsync(ScheduleTaskEntity scheduleTask)
        {
            if (scheduleTask == null) throw new ArgumentNullException(nameof(scheduleTask));

            try
            {
                _context.ScheduleTaskEntities.Add(scheduleTask);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting ScheduleTask: {ex.Message}");
                throw;
            }

            return scheduleTask;
        }

        public async Task<IEnumerable<ScheduleTaskEntity>> GetAllScheduleTasksAsync()
        {
            return await _context.ScheduleTaskEntities
                .Include(st => st.Schedule)
                .Include(st => st.ControlDevice)
                .ToListAsync();
        }

        public async Task<IEnumerable<ScheduleTaskEntity>> GetScheduleTasksAsync(int scheduleId)
        {
            return await _context.ScheduleTaskEntities
                .Include(st => st.Schedule)
                .Include(st => st.ControlDevice)
                .Where(st => st.ScheduleId == scheduleId)
                .ToListAsync();
        }

        public async Task<ScheduleTaskEntity> GetScheduleTaskByIdAsync(int id)
        {
            return await _context.ScheduleTaskEntities
                .Include(st => st.Schedule)
                .Include(st => st.ControlDevice)
                .Where(st => st.ScheduleTaskId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ScheduleTaskEntity> UpdateScheduleTaskAsync(ScheduleTaskEntity scheduleTask)
        {
            try
            {
                _context.Entry(scheduleTask).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating ScheduleTask: {scheduleTask.ScheduleTaskId}, {ex.Message}");
                throw;
            }

            return scheduleTask;
        }

        public async Task DeleteScheduleTaskAsync(int id)
        {
            var scheduleTaskEntity = await _context.ScheduleTaskEntities
                .Where(st => st.ScheduleTaskId == id).FirstOrDefaultAsync();

            if (scheduleTaskEntity == null)
            {
                _logger.LogWarning($"Tried deleting Schedule Task that does not exist, ScheduleTaskId: {id}");
                return;
            }

            try
            {
                _context.ScheduleTaskEntities.Remove(scheduleTaskEntity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting Schedule Task ({id}): {ex.Message}");
                throw;
            }
        }
        #endregion

    }
}
