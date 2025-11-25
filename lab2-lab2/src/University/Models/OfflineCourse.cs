namespace University.Models;

public class OfflineCourse : Course
{
    public string Classroom { get; set; } = string.Empty;
    public string Schedule { get; set; } = string.Empty;
    public int? Capacity { get; set; }
}

