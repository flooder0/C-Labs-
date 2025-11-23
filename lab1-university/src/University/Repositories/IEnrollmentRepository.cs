using University.Models;

namespace University.Repositories;

public interface IEnrollmentRepository
{
    void Enroll(Guid courseId, Guid studentId);
    void Unenroll(Guid courseId, Guid studentId);
    bool IsStudentEnrolled(Guid courseId, Guid studentId);
    IReadOnlyCollection<Student> GetStudentsByCourse(Guid courseId);
    void RemoveAllByCourse(Guid courseId);
}


