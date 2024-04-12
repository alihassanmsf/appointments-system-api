namespace AppointmentSystemApi.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }
        public int AppointmentStatusId { get; set; }
        public virtual AppointmentStatus AppointmentStatus { get; set; }
        // Additional properties for details about the appointment can be added.
    }

}
