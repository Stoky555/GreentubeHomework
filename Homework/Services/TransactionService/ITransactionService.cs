using Homework.Model.ServiceResponse;

namespace Homework.Services
{
    public interface ITransactionService
    {
        Task<ServiceResponse<string>> WinAsync(decimal amount, Guid playerGuid);
        Task<ServiceResponse<string>> DepositAsync(decimal amount, Guid playerGuid);
        Task<ServiceResponse<string>> StakeAsync(decimal amount, Guid playerGuid);
    }
}


