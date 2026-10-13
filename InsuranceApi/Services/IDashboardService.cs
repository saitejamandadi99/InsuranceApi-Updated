using InsuranceApi.DTO;

namespace InsuranceApi.Services
{
    public interface IDashboardService
    {
        Task<AdminDashboardResponseDTO> GetAdminDashboard();
        Task<OfficerDashboardResponseDTO> GetOfficerDashboard();
        Task<CustomerDashboardResponseDTO> GetCustomerDashboard(int userId); 


    }
}
