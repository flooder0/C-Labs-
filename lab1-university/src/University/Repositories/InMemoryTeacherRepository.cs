using University.Models;

namespace University.Repositories;

public class InMemoryTeacherRepository : ITeacherRepository
{
    private readonly Dictionary<Guid, Teacher> _teachers = new();

    public void Add(Teacher teacher)
    {
        _teachers[teacher.Id] = teacher;
    }

    public Teacher? GetById(Guid teacherId)
    {
        return _teachers.TryGetValue(teacherId, out var teacher) ? teacher : null;
    }

    public bool Exists(Guid teacherId)
    {
        return _teachers.ContainsKey(teacherId);
    }
}


