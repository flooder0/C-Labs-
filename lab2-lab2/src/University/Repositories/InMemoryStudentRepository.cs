using University.Models;

namespace University.Repositories;

public class InMemoryStudentRepository : IStudentRepository
{
    private readonly Dictionary<Guid, Student> _students = new();

    public void Add(Student student)
    {
        _students[student.Id] = student;
    }

    public Student? GetById(Guid studentId)
    {
        return _students.TryGetValue(studentId, out var student) ? student : null;
    }

    public IEnumerable<Student> GetAll()
    {
        return _students.Values;
    }

    public bool Exists(Guid studentId)
    {
        return _students.ContainsKey(studentId);
    }
}

