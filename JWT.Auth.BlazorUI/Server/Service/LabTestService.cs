using JWT.Auth.BlazorUI.Shared.Models;
using System.Collections.Generic;
using System.Linq;


namespace JWT.Auth.BlazorUI.Server.Service
{
    public class LabTestService
    {
        private List<LabTest> labTestList = new List<LabTest>
        {
            new LabTest { Id = 1, TestName = "Blood Test", Description = "General blood analysis" },
            new LabTest { Id = 2, TestName = "X-Ray", Description = "Chest X-ray" }
        };

        public List<LabTest> GetAllLabTests() => labTestList;

        public void AddLabTest(LabTest labTest)
        {
            labTest.Id = labTestList.Max(lt => lt.Id) + 1;
            labTestList.Add(labTest);
        }

        public void RemoveLabTest(int id) => labTestList.RemoveAll(lt => lt.Id == id);
    }
}
