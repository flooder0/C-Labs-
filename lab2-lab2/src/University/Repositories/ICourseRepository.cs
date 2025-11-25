using University.Models;

namespace University.Repositories;

public interface ICourseRepository
{
    void Add(Course course);
    void Remove(Guid courseId);
    Course? GetById(Guid courseId);
    IEnumerable<Course> GetAll();
    IEnumerable<Course> GetByTeacherId(Guid teacherId);
}

