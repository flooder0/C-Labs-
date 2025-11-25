using University.Models;

namespace University.Repositories;

public interface IEnrollmentRepository
{
    void Add(Enrollment enrollment);
    void Remove(Guid enrollmentId);
    Enrollment? GetById(Guid enrollmentId);
    IEnumerable<Enrollment> GetByCourseId(Guid courseId);
    IEnumerable<Enrollment> GetByStudentId(Guid studentId);
    bool IsStudentEnrolled(Guid courseId, Guid studentId);
    void RemoveAllByCourse(Guid courseId);
}

