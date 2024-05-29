using JWT.Auth.BlazorUI.Shared.Models;

namespace JWT.Auth.BlazorUI.Client
{
    public class StaffService
    {
        private List<Staff> staffList = new List<Staff>
        {
            new Staff { Id = 1, Name = "John Doe", Position = "Doctor" },
            new Staff { Id = 2, Name = "Jane Smith", Position = "Nurse" }
        };

        public List<Staff> GetAllStaff() => staffList;

        public void AddStaff(Staff staff)
        {
            staff.Id = staffList.Max(s => s.Id) + 1;
            staffList.Add(staff);
        }

        public void RemoveStaff(int id) => staffList.RemoveAll(s => s.Id == id);
    }
}
