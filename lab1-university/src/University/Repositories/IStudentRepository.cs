using University.Models;

namespace University.Repositories;

public interface IStudentRepository
{
    void Add(Student student);
    Student? GetById(Guid studentId);
    bool Exists(Guid studentId);
}


