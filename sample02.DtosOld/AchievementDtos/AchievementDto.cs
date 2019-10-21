using sample02.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace sample02.Dtos.AchievementDtos
{
    public class AchievementDto
    {
        public AchievementDto(Student student,Course course)
        {
            this.StudentName = student.Name;
            this.CourseName = course.CourseName;
        }
        public int id { get; set; }
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public int Score { get; set; }
    }
}
