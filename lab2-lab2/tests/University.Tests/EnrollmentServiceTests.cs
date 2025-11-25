using University.Models;
using University.Repositories;
using University.Services;
using Xunit;

namespace University.Tests;

public class EnrollmentServiceTests
{
    [Fact]
    public void EnrollStudent_Should_AddEnrollment_When_ValidData()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository();
        var service = new EnrollmentService(courseRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse
        {
            Id = Guid.NewGuid(),
            Title = "C# Basics"
        };
        courseRepo.Add(course);

        var student = new Student
        {
            Id = Guid.NewGuid(),
            Name = "Alice",
            Email = "alice@example.com"
        };
        studentRepo.Add(student);

        service.EnrollStudent(course.Id, student.Id);

        Assert.True(enrollmentRepo.IsStudentEnrolled(course.Id, student.Id));
    }

    [Fact]
    public void EnrollStudent_Should_ThrowException_When_CourseNotFound()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository();
        var service = new EnrollmentService(courseRepo, studentRepo, enrollmentRepo);

        var student = new Student { Id = Guid.NewGuid(), Name = "Alice" };
        studentRepo.Add(student);

        var nonExistentCourseId = Guid.NewGuid();

        Assert.Throws<InvalidOperationException>(() =>
            service.EnrollStudent(nonExistentCourseId, student.Id));
    }

    [Fact]
    public void EnrollStudent_Should_ThrowException_When_StudentNotFound()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository();
        var service = new EnrollmentService(courseRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "C# Basics" };
        courseRepo.Add(course);

        var nonExistentStudentId = Guid.NewGuid();

        Assert.Throws<InvalidOperationException>(() =>
            service.EnrollStudent(course.Id, nonExistentStudentId));
    }

    [Fact]
    public void EnrollStudent_Should_ThrowException_When_StudentAlreadyEnrolled()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository();
        var service = new EnrollmentService(courseRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "C# Basics" };
        courseRepo.Add(course);

        var student = new Student { Id = Guid.NewGuid(), Name = "Alice" };
        studentRepo.Add(student);

        service.EnrollStudent(course.Id, student.Id);

        Assert.Throws<InvalidOperationException>(() =>
            service.EnrollStudent(course.Id, student.Id));
    }

    [Fact]
    public void EnrollStudent_Should_ThrowException_When_OfflineCourseCapacityExceeded()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository();
        var service = new EnrollmentService(courseRepo, studentRepo, enrollmentRepo);

        var course = new OfflineCourse
        {
            Id = Guid.NewGuid(),
            Title = "Database Design",
            Capacity = 2
        };
        courseRepo.Add(course);

        var student1 = new Student { Id = Guid.NewGuid(), Name = "Student 1" };
        var student2 = new Student { Id = Guid.NewGuid(), Name = "Student 2" };
        var student3 = new Student { Id = Guid.NewGuid(), Name = "Student 3" };
        studentRepo.Add(student1);
        studentRepo.Add(student2);
        studentRepo.Add(student3);

        service.EnrollStudent(course.Id, student1.Id);
        service.EnrollStudent(course.Id, student2.Id);

        Assert.Throws<InvalidOperationException>(() =>
            service.EnrollStudent(course.Id, student3.Id));
    }

    [Fact]
    public void EnrollStudent_Should_AllowEnrollment_When_OfflineCourseCapacityNotSet()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository();
        var service = new EnrollmentService(courseRepo, studentRepo, enrollmentRepo);

        var course = new OfflineCourse
        {
            Id = Guid.NewGuid(),
            Title = "Database Design",
            Capacity = null
        };
        courseRepo.Add(course);

        var student = new Student { Id = Guid.NewGuid(), Name = "Alice" };
        studentRepo.Add(student);

        service.EnrollStudent(course.Id, student.Id);

        Assert.True(enrollmentRepo.IsStudentEnrolled(course.Id, student.Id));
    }

    [Fact]
    public void UnenrollStudent_Should_RemoveEnrollment_When_StudentEnrolled()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository();
        var service = new EnrollmentService(courseRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "C# Basics" };
        courseRepo.Add(course);

        var student = new Student { Id = Guid.NewGuid(), Name = "Alice" };
        studentRepo.Add(student);

        service.EnrollStudent(course.Id, student.Id);
        Assert.True(enrollmentRepo.IsStudentEnrolled(course.Id, student.Id));

        service.UnenrollStudent(course.Id, student.Id);
        Assert.False(enrollmentRepo.IsStudentEnrolled(course.Id, student.Id));
    }

    [Fact]
    public void UnenrollStudent_Should_ThrowException_When_StudentNotEnrolled()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository();
        var service = new EnrollmentService(courseRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "C# Basics" };
        courseRepo.Add(course);

        var student = new Student { Id = Guid.NewGuid(), Name = "Alice" };
        studentRepo.Add(student);

        Assert.Throws<InvalidOperationException>(() =>
            service.UnenrollStudent(course.Id, student.Id));
    }

    [Fact]
    public void GetStudentsForCourse_Should_ReturnAllEnrolledStudents_When_CourseHasStudents()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository();
        var service = new EnrollmentService(courseRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "C# Basics" };
        courseRepo.Add(course);

        var student1 = new Student { Id = Guid.NewGuid(), Name = "Alice" };
        var student2 = new Student { Id = Guid.NewGuid(), Name = "Bob" };
        studentRepo.Add(student1);
        studentRepo.Add(student2);

        service.EnrollStudent(course.Id, student1.Id);
        service.EnrollStudent(course.Id, student2.Id);

        var students = service.GetStudentsForCourse(course.Id);

        Assert.Equal(2, students.Count);
        Assert.Contains(students, s => s.Id == student1.Id);
        Assert.Contains(students, s => s.Id == student2.Id);
    }

    [Fact]
    public void GetStudentsForCourse_Should_ReturnEmptyList_When_CourseHasNoStudents()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository();
        var service = new EnrollmentService(courseRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "C# Basics" };
        courseRepo.Add(course);

        var students = service.GetStudentsForCourse(course.Id);

        Assert.Empty(students);
    }

    [Fact]
    public void GetStudentsForCourse_Should_ThrowException_When_CourseNotFound()
    {
        var courseRepo = new InMemoryCourseRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository();
        var service = new EnrollmentService(courseRepo, studentRepo, enrollmentRepo);

        var nonExistentCourseId = Guid.NewGuid();

        Assert.Throws<InvalidOperationException>(() =>
            service.GetStudentsForCourse(nonExistentCourseId));
    }
}

