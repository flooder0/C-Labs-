using University.Models;
using University.Repositories;

namespace University.Services;

public class EnrollmentService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;

    public EnrollmentService(
        ICourseRepository courseRepository,
        IStudentRepository studentRepository,
        IEnrollmentRepository enrollmentRepository)
    {
        _courseRepository = courseRepository;
        _studentRepository = studentRepository;
        _enrollmentRepository = enrollmentRepository;
    }

    public void EnrollStudent(Guid courseId, Guid studentId)
    {
        var course = _courseRepository.GetById(courseId);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        if (!_studentRepository.Exists(studentId))
            throw new InvalidOperationException("Student not found");

        if (_enrollmentRepository.IsStudentEnrolled(courseId, studentId))
            throw new InvalidOperationException("Student already enrolled");

        var offlineCourse = course as OfflineCourse;
        if (offlineCourse != null && offlineCourse.Capacity.HasValue)
        {
            var enrolled = _enrollmentRepository.GetByCourseId(courseId);
            var count = 0;
            foreach (var e in enrolled)
            {
                count++;
            }

            if (count >= offlineCourse.Capacity.Value)
                throw new InvalidOperationException("Course is full");
        }

        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            CourseId = courseId,
            StudentId = studentId,
            EnrollmentDate = DateTime.UtcNow
        };

        _enrollmentRepository.Add(enrollment);
    }

    public void UnenrollStudent(Guid courseId, Guid studentId)
    {
        Enrollment? enrollment = null;
        foreach (var e in _enrollmentRepository.GetByCourseId(courseId))
        {
            if (e.StudentId == studentId)
            {
                enrollment = e;
                break;
            }
        }

        if (enrollment == null)
            throw new InvalidOperationException("Student is not enrolled in this course");

        _enrollmentRepository.Remove(enrollment.Id);
    }

    public IReadOnlyCollection<Student> GetStudentsForCourse(Guid courseId)
    {
        var course = _courseRepository.GetById(courseId);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        var enrollments = _enrollmentRepository.GetByCourseId(courseId);
        var result = new List<Student>();

        foreach (var enrollment in enrollments)
        {
            var student = _studentRepository.GetById(enrollment.StudentId);
            if (student != null)
            {
                result.Add(student);
            }
        }

        return result;
    }
}

