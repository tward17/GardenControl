using GardenControlCore.Enums;
using GardenControlServices;
using GardenControlServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GardenControlDaemon
{
    public class ScheduleWorker : IHostedService, IDisposable
    {
        private IConfiguration _configuration { get; init; }
        private HttpClient _httpClient { get; init; }
        private Timer _timer;

        public ScheduleWorker(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _configuration = configuration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(RunPendingSchedules, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        private async void RunPendingSchedules(object state)
        {
            var baseUrl = string.Empty;

            baseUrl = _configuration.GetValue<string>("ApiEndPointBaseUrl");

            if (string.IsNullOrWhiteSpace(baseUrl))
                baseUrl = "http://jonpi.lan:5000";
            try
            {
                var result = await _httpClient.GetAsync($"{baseUrl}/api/Schedule/RunPendingSchedules");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
