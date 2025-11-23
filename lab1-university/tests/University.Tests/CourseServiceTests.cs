using University.Models;
using University.Repositories;
using University.Services;
using Xunit;

namespace University.Tests;

public class CourseServiceTests
{
    private CourseService CreateService()
    {
        var courseRepository = new InMemoryCourseRepository();
        var teacherRepository = new InMemoryTeacherRepository();
        var studentRepository = new InMemoryStudentRepository();
        var enrollmentRepository = new InMemoryEnrollmentRepository(studentRepository);
        return new CourseService(courseRepository, teacherRepository, studentRepository, enrollmentRepository);
    }

    [Fact]
    public void AddOnlineCourse_Works()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);
        var course = new OnlineCourse
        {
            Id = Guid.NewGuid(),
            Title = "C# Programming",
            MeetingLink = "https://zoom.us/meeting/123",
            Platform = "Zoom"
        };

        service.AddCourse(course);
        var saved = courseRepo.GetById(course.Id);
        Assert.NotNull(saved);
        Assert.Equal(course.Title, saved!.Title);
    }

    [Fact]
    public void AddOfflineCourse_Works()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);
        var course = new OfflineCourse
        {
            Id = Guid.NewGuid(),
            Title = "Database Design",
            Classroom = "Room 101",
            Capacity = 30
        };

        service.AddCourse(course);
        var saved = courseRepo.GetById(course.Id);
        Assert.NotNull(saved);
        Assert.Equal(course.Title, saved!.Title);
    }

    [Fact]
    public void SetTeacher_Works()
    {
        var service = CreateService();
        var teacher = new Teacher { Id = Guid.NewGuid(), FullName = "John Doe" };
        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "Math" };

        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var testService = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        teacherRepo.Add(teacher);
        courseRepo.Add(course);
        testService.SetTeacher(course.Id, teacher.Id);

        var updatedCourse = courseRepo.GetById(course.Id);
        Assert.Equal(teacher.Id, updatedCourse?.TeacherId);
    }

    [Fact]
    public void SetTeacher_Throws_When_TeacherMissing()
    {
        var service = CreateService();
        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "Math" };

        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var testService = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        courseRepo.Add(course);
        var nonExistentTeacherId = Guid.NewGuid();

        Assert.Throws<InvalidOperationException>(() => testService.SetTeacher(course.Id, nonExistentTeacherId));
    }

    [Fact]
    public void EnrollStudent_Should_EnrollStudent_When_Valid()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "Math" };
        var student = new Student { Id = Guid.NewGuid(), FullName = "Alice" };

        courseRepo.Add(course);
        studentRepo.Add(student);
        service.EnrollStudent(course.Id, student.Id);

        var students = service.GetStudentsForCourse(course.Id);
        Assert.Single(students);
        Assert.Equal(student.Id, students.First().Id);
    }

    [Fact]
    public void EnrollStudent_Should_ThrowException_When_StudentDoesNotExist()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "Math" };
        courseRepo.Add(course);
        var nonExistentStudentId = Guid.NewGuid();

        Assert.Throws<InvalidOperationException>(() => service.EnrollStudent(course.Id, nonExistentStudentId));
    }

    [Fact]
    public void EnrollStudent_Should_ThrowException_When_CourseDoesNotExist()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        var student = new Student { Id = Guid.NewGuid(), FullName = "Alice" };
        studentRepo.Add(student);
        var nonExistentCourseId = Guid.NewGuid();

        Assert.Throws<ArgumentException>(() => service.EnrollStudent(nonExistentCourseId, student.Id));
    }

    [Fact]
    public void EnrollStudent_Should_ThrowException_When_StudentAlreadyEnrolled()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "Math" };
        var student = new Student { Id = Guid.NewGuid(), FullName = "Alice" };

        courseRepo.Add(course);
        studentRepo.Add(student);
        service.EnrollStudent(course.Id, student.Id);

        Assert.Throws<InvalidOperationException>(() => service.EnrollStudent(course.Id, student.Id));
    }

    [Fact]
    public void EnrollStudent_Should_ThrowException_When_OfflineCourseCapacityExceeded()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        var course = new OfflineCourse
        {
            Id = Guid.NewGuid(),
            Title = "Physics",
            Classroom = "Room 201",
            Capacity = 2
        };

        var student1 = new Student { Id = Guid.NewGuid(), FullName = "Alice" };
        var student2 = new Student { Id = Guid.NewGuid(), FullName = "Bob" };
        var student3 = new Student { Id = Guid.NewGuid(), FullName = "Charlie" };

        courseRepo.Add(course);
        studentRepo.Add(student1);
        studentRepo.Add(student2);
        studentRepo.Add(student3);

        service.EnrollStudent(course.Id, student1.Id);
        service.EnrollStudent(course.Id, student2.Id);

        Assert.Throws<InvalidOperationException>(() => service.EnrollStudent(course.Id, student3.Id));
    }

    [Fact]
    public void UnenrollStudent_Should_RemoveStudent_When_Enrolled()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "Math" };
        var student = new Student { Id = Guid.NewGuid(), FullName = "Alice" };

        courseRepo.Add(course);
        studentRepo.Add(student);
        service.EnrollStudent(course.Id, student.Id);
        service.UnenrollStudent(course.Id, student.Id);

        var students = service.GetStudentsForCourse(course.Id);
        Assert.Empty(students);
    }

    [Fact]
    public void GetStudentsForCourse_Should_ReturnAllStudents_When_MultipleEnrolled()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "Math" };
        var student1 = new Student { Id = Guid.NewGuid(), FullName = "Alice" };
        var student2 = new Student { Id = Guid.NewGuid(), FullName = "Bob" };

        courseRepo.Add(course);
        studentRepo.Add(student1);
        studentRepo.Add(student2);
        service.EnrollStudent(course.Id, student1.Id);
        service.EnrollStudent(course.Id, student2.Id);

        var students = service.GetStudentsForCourse(course.Id);
        Assert.Equal(2, students.Count);
    }

    [Fact]
    public void GetTeacherCourses_Returns_All()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        var teacher = new Teacher { Id = Guid.NewGuid(), FullName = "John Doe" };
        var course1 = new OnlineCourse { Id = Guid.NewGuid(), Title = "Math" };
        var course2 = new OfflineCourse { Id = Guid.NewGuid(), Title = "Physics", Classroom = "Room 101" };

        teacherRepo.Add(teacher);
        courseRepo.Add(course1);
        courseRepo.Add(course2);
        service.SetTeacher(course1.Id, teacher.Id);
        service.SetTeacher(course2.Id, teacher.Id);

        var courses = service.GetTeacherCourses(teacher.Id);
        Assert.Equal(2, courses.Count);
    }

    [Fact]
    public void GetTeacherCourses_Throws_When_TeacherMissing()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        var nonExistentTeacherId = Guid.NewGuid();

        Assert.Throws<ArgumentException>(() => service.GetTeacherCourses(nonExistentTeacherId));
    }

    [Fact]
    public void RemoveCourse_Removes_Enrollments()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var studentRepo = new InMemoryStudentRepository();
        var enrollmentRepo = new InMemoryEnrollmentRepository(studentRepo);
        var service = new CourseService(courseRepo, teacherRepo, studentRepo, enrollmentRepo);

        var course = new OnlineCourse { Id = Guid.NewGuid(), Title = "Math" };
        var student = new Student { Id = Guid.NewGuid(), FullName = "Alice" };

        courseRepo.Add(course);
        studentRepo.Add(student);
        service.EnrollStudent(course.Id, student.Id);
        service.RemoveCourse(course.Id);

        Assert.Null(courseRepo.GetById(course.Id));
        var students = enrollmentRepo.GetStudentsByCourse(course.Id);
        Assert.Empty(students);
    }
}

