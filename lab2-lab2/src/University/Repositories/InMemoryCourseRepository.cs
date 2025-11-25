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
        if (_courses.TryGetValue(courseId, out var course))
        {
            return course;
        }

        return null;
    }

    public IEnumerable<Course> GetAll()
    {
        var result = new List<Course>();
        foreach (var course in _courses.Values)
        {
            result.Add(course);
        }

        return result;
    }

    public IEnumerable<Course> GetByTeacherId(Guid teacherId)
    {
        var result = new List<Course>();
        foreach (var course in _courses.Values)
        {
            if (course.TeacherId == teacherId)
            {
                result.Add(course);
            }
        }

        return result;
    }
}

