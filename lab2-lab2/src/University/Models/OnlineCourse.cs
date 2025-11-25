namespace University.Models;

public class OnlineCourse : Course
{
    public string Platform { get; set; } = string.Empty;
    public string MeetingLink { get; set; } = string.Empty;
    public int? MaxStudents { get; set; }
}

