using GardenControlRepositories.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GardenControlRepositories
{
    public class GardenControlContext : DbContext
    {
        public GardenControlContext(DbContextOptions<GardenControlContext> options) : base(options)
        { }

        public DbSet<AppSettingEntity> AppSettingEntities { get; set; }
        public DbSet<ControlDeviceEntity> ControlDeviceEntities { get; set; }
        public DbSet<MeasurementEntity> MeasurementEntities { get; set; }
        
        public DbSet<ScheduleEntity> TaskScheduleEntities { get; set; }
        //public DbSet<TaskActionEntity> TaskActionEntities { get; set; }
        //public DbSet<TimeIntervalEntity> TimeIntervalEntities { get; set; }
        //public DbSet<TriggerTypeEntity> TriggerTypeEntities { get; set; }
    }
}
