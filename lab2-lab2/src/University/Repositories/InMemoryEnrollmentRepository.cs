using University.Models;

namespace University.Repositories;

public class InMemoryEnrollmentRepository : IEnrollmentRepository
{
    private readonly Dictionary<Guid, Enrollment> _enrollments = new();

    public void Add(Enrollment enrollment)
    {
        _enrollments[enrollment.Id] = enrollment;
    }

    public void Remove(Guid enrollmentId)
    {
        _enrollments.Remove(enrollmentId);
    }

    public Enrollment? GetById(Guid enrollmentId)
    {
        return _enrollments.TryGetValue(enrollmentId, out var enrollment) ? enrollment : null;
    }

    public IEnumerable<Enrollment> GetByCourseId(Guid courseId)
    {
        var result = new List<Enrollment>();
        foreach (var enrollment in _enrollments.Values)
        {
            if (enrollment.CourseId == courseId)
            {
                result.Add(enrollment);
            }
        }

        return result;
    }

    public IEnumerable<Enrollment> GetByStudentId(Guid studentId)
    {
        var result = new List<Enrollment>();
        foreach (var enrollment in _enrollments.Values)
        {
            if (enrollment.StudentId == studentId)
            {
                result.Add(enrollment);
            }
        }

        return result;
    }

    public bool IsStudentEnrolled(Guid courseId, Guid studentId)
    {
        foreach (var enrollment in _enrollments.Values)
        {
            if (enrollment.CourseId == courseId && enrollment.StudentId == studentId)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveAllByCourse(Guid courseId)
    {
        var ids = new List<Guid>();
        foreach (var enrollment in _enrollments.Values)
        {
            if (enrollment.CourseId == courseId)
            {
                ids.Add(enrollment.Id);
            }
        }

        foreach (var enrollmentId in ids)
        {
            _enrollments.Remove(enrollmentId);
        }
    }
}

