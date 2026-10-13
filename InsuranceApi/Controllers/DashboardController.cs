using InsuranceApi.Config;
using InsuranceApi.Models;
using InsuranceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceApi.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [Authorize(Roles = nameof(Role.Admin))]
        [HttpGet("api/adminDashboard")]
       public async Task<IActionResult> GetAdminDashboard()
        {
            var result =await _service.GetAdminDashboard();
            return Success(result, "Details fetched successfully");
        }

        [Authorize(Roles = nameof(Role.Officer))]
        [HttpGet("api/officerDashboard")]
        public async Task<IActionResult> GetOfficerDashboard()
        {
            var result =await _service.GetOfficerDashboard();
            return Success(result, "Details fetched successfully");
        }

        [Authorize(Roles = nameof(Role.Customer))]
        [HttpGet("api/customerDashboard")]
        public async Task<IActionResult> GetCustomerDashboard()
        {
            var result = await _service.GetCustomerDashboard(User.GetUserId());
            return Success(result, "Details fetched successfully");
        }
    }
}
