using University.Models;
using University.Repositories;
using University.Services;
using Xunit;

namespace University.Tests;

public class CourseServiceTests
{
    [Fact]
    public void CreateCourse_Should_AddOnlineCourse_When_ValidData()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var service = new CourseService(courseRepo, teacherRepo);

        var course = new OnlineCourse
        {
            Id = Guid.NewGuid(),
            Title = "C# Basics",
            Description = "Introduction to C#",
            Platform = "Zoom",
            MeetingLink = "https://zoom.us/meeting123"
        };

        service.CreateCourse(course);

        var retrieved = courseRepo.GetById(course.Id);
        Assert.NotNull(retrieved);
        Assert.IsType<OnlineCourse>(retrieved);
        Assert.Equal("C# Basics", retrieved.Title);
    }

    [Fact]
    public void CreateCourse_Should_AddOfflineCourse_When_ValidData()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var service = new CourseService(courseRepo, teacherRepo);

        var course = new OfflineCourse
        {
            Id = Guid.NewGuid(),
            Title = "Database Design",
            Description = "Learn database design",
            Classroom = "Room 101",
            Schedule = "Mon, Wed 10:00-12:00",
            Capacity = 30
        };

        service.CreateCourse(course);

        var retrieved = courseRepo.GetById(course.Id);
        Assert.NotNull(retrieved);
        Assert.IsType<OfflineCourse>(retrieved);
        var offlineCourse = (OfflineCourse)retrieved;
        Assert.Equal("Room 101", offlineCourse.Classroom);
        Assert.Equal(30, offlineCourse.Capacity);
    }

    [Fact]
    public void CreateCourse_Should_ThrowException_When_CourseIsNull()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var service = new CourseService(courseRepo, teacherRepo);

        Assert.Throws<ArgumentNullException>(() => service.CreateCourse(null!));
    }

    [Fact]
    public void AssignTeacherToCourse_Should_SetTeacher_When_TeacherExists()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var service = new CourseService(courseRepo, teacherRepo);

        var teacher = new Teacher
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Department = "Computer Science"
        };
        teacherRepo.Add(teacher);

        var course = new OnlineCourse
        {
            Id = Guid.NewGuid(),
            Title = "C# Basics",
            Description = "Introduction to C#"
        };
        courseRepo.Add(course);

        service.AssignTeacherToCourse(course.Id, teacher.Id);

        var updatedCourse = courseRepo.GetById(course.Id);
        Assert.NotNull(updatedCourse);
        Assert.Equal(teacher.Id, updatedCourse.TeacherId);
    }

    [Fact]
    public void AssignTeacherToCourse_Should_ThrowException_When_CourseNotFound()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var service = new CourseService(courseRepo, teacherRepo);

        var teacher = new Teacher { Id = Guid.NewGuid(), Name = "John Doe" };
        teacherRepo.Add(teacher);

        var nonExistentCourseId = Guid.NewGuid();

        Assert.Throws<InvalidOperationException>(() =>
            service.AssignTeacherToCourse(nonExistentCourseId, teacher.Id));
    }

    [Fact]
    public void AssignTeacherToCourse_Should_ThrowException_When_TeacherNotFound()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var service = new CourseService(courseRepo, teacherRepo);

        var course = new OnlineCourse
        {
            Id = Guid.NewGuid(),
            Title = "C# Basics"
        };
        courseRepo.Add(course);

        var nonExistentTeacherId = Guid.NewGuid();

        Assert.Throws<InvalidOperationException>(() =>
            service.AssignTeacherToCourse(course.Id, nonExistentTeacherId));
    }

    [Fact]
    public void GetCoursesByTeacher_Should_ReturnOnlyTeacherCourses_When_TeacherHasCourses()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var service = new CourseService(courseRepo, teacherRepo);

        var teacher1 = new Teacher { Id = Guid.NewGuid(), Name = "Teacher 1" };
        var teacher2 = new Teacher { Id = Guid.NewGuid(), Name = "Teacher 2" };
        teacherRepo.Add(teacher1);
        teacherRepo.Add(teacher2);

        var course1 = new OnlineCourse { Id = Guid.NewGuid(), Title = "Course 1", TeacherId = teacher1.Id };
        var course2 = new OnlineCourse { Id = Guid.NewGuid(), Title = "Course 2", TeacherId = teacher1.Id };
        var course3 = new OnlineCourse { Id = Guid.NewGuid(), Title = "Course 3", TeacherId = teacher2.Id };
        courseRepo.Add(course1);
        courseRepo.Add(course2);
        courseRepo.Add(course3);

        var teacher1Courses = service.GetCoursesByTeacher(teacher1.Id).ToList();

        Assert.Equal(2, teacher1Courses.Count);
        Assert.All(teacher1Courses, c => Assert.Equal(teacher1.Id, c.TeacherId));
    }

    [Fact]
    public void GetCoursesByTeacher_Should_ThrowException_When_TeacherNotFound()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var service = new CourseService(courseRepo, teacherRepo);

        var nonExistentTeacherId = Guid.NewGuid();

        Assert.Throws<InvalidOperationException>(() =>
            service.GetCoursesByTeacher(nonExistentTeacherId));
    }

    [Fact]
    public void DeleteCourse_Should_RemoveCourse_When_CourseExists()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var service = new CourseService(courseRepo, teacherRepo);

        var course = new OnlineCourse
        {
            Id = Guid.NewGuid(),
            Title = "C# Basics"
        };
        courseRepo.Add(course);

        service.DeleteCourse(course.Id);

        var retrieved = courseRepo.GetById(course.Id);
        Assert.Null(retrieved);
    }

    [Fact]
    public void DeleteCourse_Should_ThrowException_When_CourseNotFound()
    {
        var courseRepo = new InMemoryCourseRepository();
        var teacherRepo = new InMemoryTeacherRepository();
        var service = new CourseService(courseRepo, teacherRepo);

        var nonExistentCourseId = Guid.NewGuid();

        Assert.Throws<InvalidOperationException>(() => service.DeleteCourse(nonExistentCourseId));
    }
}

