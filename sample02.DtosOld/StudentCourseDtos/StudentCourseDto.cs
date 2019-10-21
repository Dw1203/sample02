using sample02.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace sample02.Dtos.StudentCourseDtos
{
    public class StudentCourseDto
    {
        public StudentCourseDto(Student student,Course course)
        {
            StudentName = student.Name;
            CourseName = course.CourseName;
        }
       // public int id { get; set; }
        public string  StudentName { get; set; }
        public string CourseName { get; set; }
    }
}
