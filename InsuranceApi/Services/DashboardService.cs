using InsuranceApi.DTO;
using InsuranceApi.Repositories;
using System.Diagnostics;

namespace InsuranceApi.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;
        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }
        public async Task<AdminDashboardResponseDTO> GetAdminDashboard()
        {
            var activeUsers = await _repo.ActiveUsersCount();
            var inactiveUsers = await _repo.InactiveUsersCount();
            var pendingClaimsCount =await _repo.GetAdminPendingClaimsCount();
            var pendingClaimsAmount =await _repo.GetAdminPendingClaimsAmount();
            var topClaims = await _repo.GetAdminTopClaims();

            return new AdminDashboardResponseDTO
            {
                ActiveUsers = activeUsers,
                InactiveUsers = inactiveUsers,
                PendingClaimsCount = pendingClaimsCount,
                PendingClaimsAmount = pendingClaimsAmount,
                TopClaims = topClaims
            };

        }

        public async Task<CustomerDashboardResponseDTO> GetCustomerDashboard(int userId)
        {
            var totalClaimsCount = await _repo.GetCustomerTotalClaimsCount(userId); 
            var pendingClaimsCount = await _repo.GetCustomerPendingClaimsCount(userId);
            var pendingClaimsAmount = await _repo.GetCustomerPendingClaimsAmount(userId);
            var history = await _repo.GetCustomerClaimHistory(userId);

            return new CustomerDashboardResponseDTO
            {
                TotalClaimsCount = totalClaimsCount,
                PendingClaimsCount = pendingClaimsCount,
                PendingClaimsAmount = pendingClaimsAmount,
                ClaimHistory = history
            };
        }

        public async Task<OfficerDashboardResponseDTO> GetOfficerDashboard()
        {
            var pendingClaimsCount = await _repo.GetOfficerPendingClaimsCount();
            var pendingClaimsAmount =await _repo.GetOfficerPendingClaimsAmount();
            var topClaims =await _repo.GetOfficerTopClaims();


            return new OfficerDashboardResponseDTO
            {
                PendingClaimsCount = pendingClaimsCount,
                PendingClaimsAmount = pendingClaimsAmount,
                TopClaims = topClaims
            };
        }
    }
}
