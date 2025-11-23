using University.Models;
using University.Repositories;
using Xunit;

namespace University.Tests;

public class RepositoryTests
{
    [Fact]
    public void InMemoryCourseRepository_Should_AddAndRetrieveCourse()
    {
        var repository = new InMemoryCourseRepository();
        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "Test Course" };

        repository.Add(course);
        var retrieved = repository.GetById(course.Id);

        Assert.NotNull(retrieved);
        Assert.Equal(course.Id, retrieved.Id);
        Assert.Equal(course.Title, retrieved.Title);
    }

    [Fact]
    public void InMemoryCourseRepository_Should_RemoveCourse()
    {
        var repository = new InMemoryCourseRepository();
        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "Test Course" };

        repository.Add(course);
        repository.Remove(course.Id);
        var retrieved = repository.GetById(course.Id);

        Assert.Null(retrieved);
    }

    [Fact]
    public void InMemoryCourseRepository_Should_GetCoursesByTeacherId()
    {
        var repository = new InMemoryCourseRepository();
        var teacherId = Guid.NewGuid();
        var course1 = new OnlineCourse { Id = Guid.NewGuid(), Title = "Course 1", TeacherId = teacherId };
        var course2 = new OnlineCourse { Id = Guid.NewGuid(), Title = "Course 2", TeacherId = teacherId };
        var course3 = new OnlineCourse { Id = Guid.NewGuid(), Title = "Course 3", TeacherId = Guid.NewGuid() };

        repository.Add(course1);
        repository.Add(course2);
        repository.Add(course3);

        var courses = repository.GetByTeacherId(teacherId);

        Assert.Equal(2, courses.Count);
        Assert.All(courses, c => Assert.Equal(teacherId, c.TeacherId));
    }

    [Fact]
    public void InMemoryTeacherRepository_Should_AddAndRetrieveTeacher()
    {
        var repository = new InMemoryTeacherRepository();
        var teacher = new Teacher { Id = Guid.NewGuid(), FullName = "John Doe" };

        repository.Add(teacher);
        var retrieved = repository.GetById(teacher.Id);

        Assert.NotNull(retrieved);
        Assert.Equal(teacher.Id, retrieved.Id);
        Assert.Equal(teacher.FullName, retrieved.FullName);
    }

    [Fact]
    public void InMemoryTeacherRepository_Should_CheckExistence()
    {
        var repository = new InMemoryTeacherRepository();
        var teacher = new Teacher { Id = Guid.NewGuid(), FullName = "John Doe" };

        Assert.False(repository.Exists(teacher.Id));
        repository.Add(teacher);
        Assert.True(repository.Exists(teacher.Id));
    }

    [Fact]
    public void InMemoryStudentRepository_Should_AddAndRetrieveStudent()
    {
        var repository = new InMemoryStudentRepository();
        var student = new Student { Id = Guid.NewGuid(), FullName = "Alice" };

        repository.Add(student);
        var retrieved = repository.GetById(student.Id);

        Assert.NotNull(retrieved);
        Assert.Equal(student.Id, retrieved.Id);
        Assert.Equal(student.FullName, retrieved.FullName);
    }

    [Fact]
    public void InMemoryStudentRepository_Should_CheckExistence()
    {
        var repository = new InMemoryStudentRepository();
        var student = new Student { Id = Guid.NewGuid(), FullName = "Alice" };

        Assert.False(repository.Exists(student.Id));
        repository.Add(student);
        Assert.True(repository.Exists(student.Id));
    }

    [Fact]
    public void InMemoryEnrollmentRepository_Should_EnrollAndRetrieveStudents()
    {
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);

        var courseId = Guid.NewGuid();
        var student1 = new Student { Id = Guid.NewGuid(), FullName = "Alice" };
        var student2 = new Student { Id = Guid.NewGuid(), FullName = "Bob" };

        studentRepo.Add(student1);
        studentRepo.Add(student2);

        enrollmentRepo.Enroll(courseId, student1.Id);
        enrollmentRepo.Enroll(courseId, student2.Id);

        var students = enrollmentRepo.GetStudentsByCourse(courseId);

        Assert.Equal(2, students.Count);
        Assert.Contains(students, s => s.Id == student1.Id);
        Assert.Contains(students, s => s.Id == student2.Id);
    }

    [Fact]
    public void InMemoryEnrollmentRepository_Should_CheckEnrollment()
    {
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);

        var courseId = Guid.NewGuid();
        var studentId = Guid.NewGuid();

        Assert.False(enrollmentRepo.IsStudentEnrolled(courseId, studentId));

        enrollmentRepo.Enroll(courseId, studentId);

        Assert.True(enrollmentRepo.IsStudentEnrolled(courseId, studentId));
    }

    [Fact]
    public void InMemoryEnrollmentRepository_Should_UnenrollStudent()
    {
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);

        var courseId = Guid.NewGuid();
        var studentId = Guid.NewGuid();

        enrollmentRepo.Enroll(courseId, studentId);
        Assert.True(enrollmentRepo.IsStudentEnrolled(courseId, studentId));

        enrollmentRepo.Unenroll(courseId, studentId);
        Assert.False(enrollmentRepo.IsStudentEnrolled(courseId, studentId));
    }

    [Fact]
    public void InMemoryEnrollmentRepository_Should_RemoveAllByCourse()
    {
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);

        var courseId = Guid.NewGuid();
        var student1 = new Student { Id = Guid.NewGuid(), FullName = "Alice" };
        var student2 = new Student { Id = Guid.NewGuid(), FullName = "Bob" };

        studentRepo.Add(student1);
        studentRepo.Add(student2);

        enrollmentRepo.Enroll(courseId, student1.Id);
        enrollmentRepo.Enroll(courseId, student2.Id);

        enrollmentRepo.RemoveAllByCourse(courseId);

        var students = enrollmentRepo.GetStudentsByCourse(courseId);
        Assert.Empty(students);
    }
}


