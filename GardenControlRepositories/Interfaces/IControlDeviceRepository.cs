using GardenControlRepositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Interfaces
{
    public interface IControlDeviceRepository
    {
        public Task<ControlDeviceEntity> InsertDeviceAsync(ControlDeviceEntity device);
        public Task<ControlDeviceEntity> GetDeviceAsync(int id);
        public Task<IEnumerable<ControlDeviceEntity>> GetAllDevicesAsync();
        public Task<ControlDeviceEntity> UpdateDeviceAsync(ControlDeviceEntity device);
        public Task DeleteDeviceAsync(int id);
    }
}
