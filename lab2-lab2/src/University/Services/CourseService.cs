using University.Models;
using University.Repositories;

namespace University.Services;

public class CourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ITeacherRepository _teacherRepository;

    public CourseService(ICourseRepository courseRepository, ITeacherRepository teacherRepository)
    {
        _courseRepository = courseRepository;
        _teacherRepository = teacherRepository;
    }

    public Course CreateCourse(Course course)
    {
        if (course == null)
            throw new ArgumentNullException(nameof(course));

        _courseRepository.Add(course);
        return course;
    }

    public void DeleteCourse(Guid courseId)
    {
        var course = _courseRepository.GetById(courseId);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        _courseRepository.Remove(courseId);
    }

    public void AssignTeacherToCourse(Guid courseId, Guid teacherId)
    {
        var course = _courseRepository.GetById(courseId);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        if (!_teacherRepository.Exists(teacherId))
            throw new InvalidOperationException("Teacher not found");

        course.TeacherId = teacherId;
    }

    public IEnumerable<Course> GetCoursesByTeacher(Guid teacherId)
    {
        if (!_teacherRepository.Exists(teacherId))
            throw new InvalidOperationException("Teacher not found");

        return _courseRepository.GetByTeacherId(teacherId);
    }
}

