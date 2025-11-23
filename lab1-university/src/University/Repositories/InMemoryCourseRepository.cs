using University.Models;

namespace University.Repositories;

public class InMemoryCourseRepository : ICourseRepository
{
    private readonly Dictionary<Guid, Course> _courses = new();

    public void Add(Course course)
    {
        _courses[course.Id] = course;
    }

    public void Remove(Guid courseId)
    {
        _courses.Remove(courseId);
    }

    public Course? GetById(Guid courseId)
    {
        return _courses.TryGetValue(courseId, out var course) ? course : null;
    }

    public IReadOnlyCollection<Course> GetAll()
    {
        return _courses.Values.ToList();
    }

    public IReadOnlyCollection<Course> GetByTeacherId(Guid teacherId)
    {
        return _courses.Values
            .Where(c => c.TeacherId == teacherId)
            .ToList()
            ;
    }
}

