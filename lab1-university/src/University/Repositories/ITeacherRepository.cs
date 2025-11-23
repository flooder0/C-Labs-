using University.Models;

namespace University.Repositories;

public interface ITeacherRepository
{
    void Add(Teacher teacher);
    Teacher? GetById(Guid teacherId);
    bool Exists(Guid teacherId);
}


