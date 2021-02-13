using AutoMapper;
using GardenControlCore.Models;
using GardenControlRepositories.Entities;
using GardenControlRepositories.Interfaces;
using GardenControlServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenControlServices
{
    /// <summary>
    /// Service responsible for managing the Control Devices that are stored in the system.
    /// Does not perform the interaction with the physical devices.
    /// </summary>
    public class ControlDeviceService : IControlDeviceService
    {
        private IControlDeviceRepository _controlDeviceRepository { get; init; }
        private IMapper _mapper { get; init; }

        public ControlDeviceService(IControlDeviceRepository controlDeviceRepository, IMapper mapper)
        {
            _controlDeviceRepository = controlDeviceRepository;
            _mapper = mapper;
        }

        public async Task DeleteDeviceAsync(int id)
        {
            await _controlDeviceRepository.DeleteDeviceAsync(id);
        }

        public async Task<IEnumerable<ControlDevice>> GetAllDevicesAsync()
        {
            return _mapper.Map<IEnumerable<ControlDevice>>(await _controlDeviceRepository.GetAllDevicesAsync());
        }

        public async Task<ControlDevice> GetDeviceAsync(int id)
        {
            return _mapper.Map<ControlDevice>(await _controlDeviceRepository.GetDeviceAsync(id));
        }

        public async Task<ControlDevice> InsertDeviceAsync(ControlDevice device)
        {
            var newDevice = await _controlDeviceRepository.InsertDeviceAsync(_mapper.Map<ControlDeviceEntity>(device));

            return _mapper.Map<ControlDevice>(newDevice);
        }

        public async Task<ControlDevice> UpdateDeviceAsync(ControlDevice device)
        {
            ControlDeviceEntity updatedDevice = null;
            try
            {
                updatedDevice = await _controlDeviceRepository.UpdateDeviceAsync(_mapper.Map<ControlDeviceEntity>(device));
            }
            catch (Exception)
            {
                // TODO: What to do on expception?
                throw;
            }

            return _mapper.Map<ControlDevice>(updatedDevice);
        }
    }
}
