using InsuranceApi.DTO;

namespace InsuranceApi.Repositories
{
    public interface IDashboardRepository
    {

        //Admin
        Task<int> ActiveUsersCount();
        Task<int> InactiveUsersCount();

        Task<int> GetAdminPendingClaimsCount();
        Task<decimal> GetAdminPendingClaimsAmount(); 
        Task<IEnumerable<DashboardClaimDTO>> GetAdminTopClaims(); //top 3 with highest claim amount 

        //officer 

        Task<int> GetOfficerPendingClaimsCount(); 
        Task<decimal> GetOfficerPendingClaimsAmount();
        Task<IEnumerable<DashboardClaimDTO>> GetOfficerTopClaims(); //top 3 with highest claim amount with status pending. 

        //customer 
        Task<int> GetCustomerTotalClaimsCount(int userId);
        Task<int> GetCustomerPendingClaimsCount(int userId);
        Task<decimal> GetCustomerPendingClaimsAmount(int userId);
        Task<IEnumerable<DashboardClaimHistoryDTO>> GetCustomerClaimHistory(int userId); //returns 5 most recent claims with their status and amount for a specific user.
    }
}
