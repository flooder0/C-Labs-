using University.Models;

namespace University.Repositories;

public interface IStudentRepository
{
    void Add(Student student);
    Student? GetById(Guid studentId);
    IEnumerable<Student> GetAll();
    bool Exists(Guid studentId);
}

