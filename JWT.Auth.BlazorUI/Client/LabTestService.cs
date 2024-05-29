using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JWT.Auth.BlazorUI.Shared.Models;
using Microsoft.AspNetCore.Cors;

namespace JWT.Auth.BlazorUI.Client
{
    public class LabTestService
    {
        private readonly HttpClient _httpClient;

        public LabTestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Patient>> GetAllPatients()
        {
            return await _httpClient.GetFromJsonAsync<List<Patient>>("api/patient");
        }

        public async Task<List<LabTest>> GetLabTestsByPatientId(int patientId)
        {
            return (await _httpClient.GetFromJsonAsync<List<LabTest>>($"api/labtest/{patientId}"))
                .Where(lt => lt.PatientId == patientId)
                .ToList();
        }
    }
}
