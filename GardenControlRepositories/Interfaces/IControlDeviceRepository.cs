using GardenControlRepositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlRepositories.Interfaces
{
    public interface IControlDeviceRepository
    {
        public Task InsertDevice(ControlDeviceEntity device);
        public Task<ControlDeviceEntity> GetDevice(int id);
        public Task<IEnumerable<ControlDeviceEntity>> GetAllDevices();
        public Task UpdateDevice(ControlDeviceEntity device);
        public Task DeleteDevice(int id);
    }
}
