using InsuranceApi.Data;
using InsuranceApi.DTO;
using InsuranceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InsuranceApi.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DatabaseContext _context;
        public DashboardRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<int> ActiveUsersCount()
        {
            return await _context.Users.CountAsync(u => u.ActiveStatus == ActiveStatus.Active);
        }

        public async Task<decimal> GetAdminPendingClaimsAmount()
        {
            var claims = _context.Claims.Where(c => c.ClaimStatus == ClaimStatus.RecommendedForApproval || c.ClaimStatus == ClaimStatus.RecommendedForRejection);
            return await claims.SumAsync(c => (decimal?)c.ClaimAmount) ?? 0m; //0m means decimal 
        }

        public async Task<int> GetAdminPendingClaimsCount()
        {
            var claims = _context.Claims.Where(c => c.ClaimStatus == ClaimStatus.RecommendedForApproval || c.ClaimStatus == ClaimStatus.RecommendedForRejection);
            return await claims.CountAsync();
        }

        public async Task<IEnumerable<DashboardClaimDTO>> GetAdminTopClaims()
        {
            var claims = _context.Claims.Where(c => c.ClaimStatus == ClaimStatus.RecommendedForApproval || c.ClaimStatus == ClaimStatus.RecommendedForRejection);
            return await claims.OrderByDescending(c => c.ClaimAmount)
                .Take(5)
                .Select(c => new DashboardClaimDTO
                {
                    ClaimId = c.ClaimId,
                    ClaimNumber = c.ClaimNumber,
                    ClaimAmount = c.ClaimAmount,
                    ClaimStatus = c.ClaimStatus
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<DashboardClaimHistoryDTO>> GetCustomerClaimHistory(int userId)
        {
            var claims = _context.Claims.Where(c => c.Policy.Customer.UserId == userId);
                
            return await  claims.OrderByDescending(c => c.CreatedDate).ThenByDescending(c => c.ClaimId)
                .Take(5)
                .Select(c => new DashboardClaimHistoryDTO
                {
                    ClaimId = c.ClaimId,
                    ClaimNumber = c.ClaimNumber,
                    ClaimAmount = c.ClaimAmount,
                    ClaimStatus = c.ClaimStatus,
                    CreatedDate = c.CreatedDate,
                    UpdatedDate = c.UpdatedDate
                }).ToListAsync();
        }

        public async Task<decimal> GetCustomerPendingClaimsAmount(int userId)
        {
            var claims = _context.Claims.Where(c => c.Policy.Customer.UserId == userId && c.ClaimStatus != ClaimStatus.Approved && c.ClaimStatus != ClaimStatus.Rejected);
            return await claims.SumAsync(c => (decimal?)c.ClaimAmount) ?? 0m;
        }

        public async Task<int> GetCustomerPendingClaimsCount(int userId)
        {
            var claims = _context.Claims.Where(c => c.Policy.Customer.UserId == userId && c.ClaimStatus != ClaimStatus.Approved && c.ClaimStatus != ClaimStatus.Rejected);
            return await claims.CountAsync();
        }

        public async Task<int> GetCustomerTotalClaimsCount(int userId)
        {
            var claims = _context.Claims.Where(c => c.Policy.Customer.UserId == userId);
            return await claims.CountAsync();
        }

        public async Task<decimal> GetOfficerPendingClaimsAmount()
        {
            var claims = _context.Claims.Where(c => c.ClaimStatus == ClaimStatus.Pending);
            return await claims.SumAsync(c => (decimal?)c.ClaimAmount) ?? 0m;
        }

        public async Task<int> GetOfficerPendingClaimsCount()
        {
            var claims = _context.Claims.Where(c => c.ClaimStatus == ClaimStatus.Pending);
            return await claims.CountAsync();
        }

        public async Task<IEnumerable<DashboardClaimDTO>> GetOfficerTopClaims()
        {
            var claims = _context.Claims.Where(c => c.ClaimStatus == ClaimStatus.Pending);
            return await claims.OrderByDescending(c => c.ClaimAmount)
                .Take(5)
                .Select(c => new DashboardClaimDTO
                {
                    ClaimId = c.ClaimId,
                    ClaimNumber = c.ClaimNumber,
                    ClaimAmount = c.ClaimAmount,
                    ClaimStatus = c.ClaimStatus
                })
                .ToListAsync();
        }

        public async Task<int> InactiveUsersCount()
        {
            return await _context.Users.CountAsync(u => u.ActiveStatus == ActiveStatus.Inactive);
        }
    }
}
