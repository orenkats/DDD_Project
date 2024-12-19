using Domain.Entities;
using Domain.Interfaces;
using Application.Interfaces;
using Application.Validations;
using FluentValidation;

namespace Application.Services
{
    public class TraderService : ITraderManagement
    {
        private readonly ITraderRepository _traderRepository;
        
        public TraderService(ITraderRepository traderRepository)
        {
            _traderRepository = traderRepository;
        }

        public async Task AddTraderAsync(Trader trader)
        {
            var validator = new TraderValidator();
            var validationResult = validator.Validate(trader);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _traderRepository.AddAsync(trader);
        }
        public async Task DeleteTraderAsync(Guid traderId)
        {
            var trader = await _traderRepository.GetByIdAsync(traderId);
            if (trader == null)
                throw new KeyNotFoundException("Trader not found.");

            await _traderRepository.DeleteAsync(trader);
        }
        public async Task<List<Trader>> GetAllTradersAsync()
        {
            return await _traderRepository.GetAllAsync();
        }

        public async Task<Trader?> GetTraderByIdAsync(Guid traderId)
        {
            return await _traderRepository.GetByIdAsync(traderId);
        }

        public async Task<List<StockOrder>> GetOrdersByTraderIdAsync(Guid traderId)
        {
            return await _traderRepository.GetOrdersByTraderIdAsync(traderId);
        }

        
    }
}
