namespace University.Models;

public class Enrollment
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime EnrollmentDate { get; set; }
}

