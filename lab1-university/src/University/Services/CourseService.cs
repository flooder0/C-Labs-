using University.Models;
using University.Repositories;

namespace University.Services;

public class CourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;

    public CourseService(
        ICourseRepository courseRepository,
        ITeacherRepository teacherRepository,
        IStudentRepository studentRepository,
        IEnrollmentRepository enrollmentRepository)
    {
        _courseRepository = courseRepository;
        _teacherRepository = teacherRepository;
        _studentRepository = studentRepository;
        _enrollmentRepository = enrollmentRepository;
    }

    public void AddCourse(Course course)
    {
        _courseRepository.Add(course);
    }

    public void RemoveCourse(Guid courseId)
    {
        _enrollmentRepository.RemoveAllByCourse(courseId);
        _courseRepository.Remove(courseId);
    }

    public void SetTeacher(Guid courseId, Guid teacherId)
    {
        var course = _courseRepository.GetById(courseId);
        if (course == null)
        {
            throw new ArgumentException($"Курс не найден: {courseId}");
        }

        if (!_teacherRepository.Exists(teacherId))
        {
            throw new InvalidOperationException($"Преподаватель не найден: {teacherId}");
        }

        course.TeacherId = teacherId;
    }

    public void EnrollStudent(Guid courseId, Guid studentId)
    {
        var course = _courseRepository.GetById(courseId);
        if (course == null)
        {
            throw new ArgumentException($"Курс не найден: {courseId}");
        }

        if (!_studentRepository.Exists(studentId))
        {
            throw new InvalidOperationException($"Студент не найден: {studentId}");
        }

        if (_enrollmentRepository.IsStudentEnrolled(courseId, studentId))
        {
            throw new InvalidOperationException("Студент уже записан.");
        }

        if (course is OfflineCourse offlineCourse && offlineCourse.Capacity.HasValue)
        {
            var enrolledCount = _enrollmentRepository.GetStudentsByCourse(courseId).Count;
            if (enrolledCount >= offlineCourse.Capacity.Value)
            {
                throw new InvalidOperationException($"Лимит мест: {offlineCourse.Capacity.Value}");
            }
        }

        _enrollmentRepository.Enroll(courseId, studentId);
    }

    public void UnenrollStudent(Guid courseId, Guid studentId)
    {
        var course = _courseRepository.GetById(courseId);
        if (course == null)
        {
            throw new ArgumentException($"Курс не найден: {courseId}");
        }

        _enrollmentRepository.Unenroll(courseId, studentId);
    }

    public IReadOnlyCollection<Student> GetStudentsForCourse(Guid courseId)
    {
        var course = _courseRepository.GetById(courseId);
        if (course == null)
        {
            throw new ArgumentException($"Курс не найден: {courseId}");
        }

        return _enrollmentRepository.GetStudentsByCourse(courseId);
    }

    public IReadOnlyCollection<Course> GetTeacherCourses(Guid teacherId)
    {
        if (!_teacherRepository.Exists(teacherId))
        {
            throw new ArgumentException($"Преподаватель не найден: {teacherId}");
        }

        return _courseRepository.GetByTeacherId(teacherId);
    }
}

