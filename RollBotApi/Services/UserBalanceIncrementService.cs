using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RollBotApi.Repositories;

namespace RollBotApi.Services
{
    public class UserBalanceIncrementService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IUserRepository _userRepository;
        private readonly ILoggingService _loggingService;

        public UserBalanceIncrementService(IUserRepository userRepository, ILoggingService loggingService)
        {
            _userRepository = userRepository;
            _loggingService = loggingService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _loggingService.LogInformation("User Balance Increment Service: Starting user balance increment service");
            _timer = new Timer(IncrementUserBalances, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private async void IncrementUserBalances(object? state)
        {
            _loggingService.LogInformation("User Balance Increment Service: Incrementing user balances");
            var users = await _userRepository.GetUsers();
            foreach (var user in users)
            {
                if (user.Balance < 100)
                {
                    user.Balance += 1;
                    await _userRepository.UpdateUser(user.DiscordId, user);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _loggingService.LogInformation("User Balance Increment Service: Stopping user balance increment service");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _loggingService.LogInformation("User Balance Increment Service: Disposing user balance increment service");
            _timer?.Dispose();
        }
    }
}