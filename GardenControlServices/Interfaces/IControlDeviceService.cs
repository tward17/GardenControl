using GardenControlCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices.Interfaces
{
    public interface IControlDeviceService
    {
        public Task<ControlDevice> InsertDeviceAsync(ControlDevice device);
        public Task<ControlDevice> GetDeviceAsync(int id);
        public Task<IEnumerable<ControlDevice>> GetAllDevicesAsync();
        public Task<ControlDevice> UpdateDeviceAsync(ControlDevice device);
        public Task DeleteDeviceAsync(int id);
    }
}
