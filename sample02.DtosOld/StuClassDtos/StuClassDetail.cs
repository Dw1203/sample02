using sample02.Dtos.StudentDtos;
using System.Collections.Generic;


namespace sample02.Dtos.StuClassDtos
{
    public  class StuClassDetail
    {
        public StuClassDetail()
        {
            Students = new List<StudentDto>();
        }
        public string ClassNum { get; set; }
        public string ClassName { get; set; }

        //班级有自己的学生
        public List<StudentDto> Students { get; set; }

        public int StuCount => Students.Count;
    }
}
