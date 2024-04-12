namespace AppointmentSystemApi.Models
{
    public class AppointmentCreationDto
    {
        public DateTime AppointmentDate { get; set; }
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int AppointmentStatusId { get; set; }
    }

}
