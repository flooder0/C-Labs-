using University.Models;

namespace University.Repositories;

public class InMemoryEnrollmentRepository : IEnrollmentRepository
{
    private readonly List<Enrollment> _enrollments = new();
    private readonly IStudentRepository _studentRepository;

    public InMemoryEnrollmentRepository(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public void Enroll(Guid courseId, Guid studentId)
    {
        _enrollments.Add(new Enrollment { CourseId = courseId, StudentId = studentId });
    }

    public void Unenroll(Guid courseId, Guid studentId)
    {
        var enrollment = _enrollments.FirstOrDefault(e => e.CourseId == courseId && e.StudentId == studentId);
        if (enrollment != null)
        {
            _enrollments.Remove(enrollment);
        }
    }

    public bool IsStudentEnrolled(Guid courseId, Guid studentId)
    {
        return _enrollments.Any(e => e.CourseId == courseId && e.StudentId == studentId);
    }

    public IReadOnlyCollection<Student> GetStudentsByCourse(Guid courseId)
    {
        var studentIds = _enrollments
            .Where(e => e.CourseId == courseId)
            .Select(e => e.StudentId)
            .ToList();

        return studentIds
            .Select(id => _studentRepository.GetById(id))
            .Where(s => s != null)
            .Cast<Student>()
            .ToList()
            ;
    }

    public void RemoveAllByCourse(Guid courseId)
    {
        _enrollments.RemoveAll(e => e.CourseId == courseId);
    }
}

