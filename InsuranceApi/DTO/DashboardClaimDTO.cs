using InsuranceApi.Models;

namespace InsuranceApi.DTO
{
    public class DashboardClaimDTO
    {
        public int ClaimId { get; set; }
        public string ClaimNumber { get; set; } = string.Empty;
        public decimal ClaimAmount { get; set; }

        public ClaimStatus ClaimStatus { get; set; }
    }
}
