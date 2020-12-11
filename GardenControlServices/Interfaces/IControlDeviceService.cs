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
        public Task InsertDevice(ControlDevice device);
        public Task<ControlDevice> GetDevice(int id);
        public Task<IEnumerable<ControlDevice>> GetAllDevices();
        public Task UpdateDevice(ControlDevice device);
        public Task DeleteDevice(int id);
    }
}
