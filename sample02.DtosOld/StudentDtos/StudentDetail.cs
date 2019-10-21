using sample02.Dtos.AchievementDtos;
using sample02.Dtos.StuClassDtos;
using sample02.Dtos.StudentCourseDtos;
using sample02.Model;
using System;
using System.Collections.Generic;

namespace sample02.Dtos.StudentDtos
{
    public  class StudentDetail
    {
        public StudentDetail()
        {
            Courses = new List<StudentCourseDto>();
        }
        public int Id { get; set; }

        public string StuNum { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDay { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public DateTime EnmDate { get; set; }

        //学生有自己得班级
        public StuClassDto StuClass { get; set; }

        //学生有自己得成绩表
        public List<AchievementDto> Achievements { get; set; }

        //学生有自己的课程表
        public List<StudentCourseDto> Courses { get; set; }
    }
}
