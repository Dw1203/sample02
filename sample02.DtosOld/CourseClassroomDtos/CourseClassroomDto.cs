using sample02.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace sample02.Dtos.CourseClassroomDtos
{
   public  class CourseClassroomDto
    {
        public CourseClassroomDto(Course course,Classroom classroom)
        {
            CourseName = course.CourseName;
            ClassroomName = classroom.Name;
        }

        public int Id { get; set; }

        public string CourseName { get; set; }
        public string ClassroomName { get; set; }

    }
}
