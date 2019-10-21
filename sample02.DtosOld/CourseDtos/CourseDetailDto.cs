using sample02.Dtos.AchievementDtos;
using sample02.Dtos.StudentCourseDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace sample02.Dtos.CourseDtos
{
    public class CourseDetailDto
    {
        public int Id { get; set; }

        public string CourseNum { get; set; }

        public string CourseName { get; set; }

        public int Score { get; set; }

        public double CourseTime { get; set; }

       

        //该课程下的学生
        public List<StudentCourseDto> Students { get; set; }

        //该课程的成绩
        public List<AchievementDto> Achievements { get; set; }
    }
}
