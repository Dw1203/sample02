using sample02.Dtos.CourseClassroomDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace sample02.Dtos.ClassroomDtos
{
    public class ClassroomDetailDto
    {

        public ClassroomDetailDto()
        {
            CourseClassrooms = new List<CourseClassroomDto>();
        }
        public int Id { get; set; }

        public string ClassRoomNum { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public List<CourseClassroomDto>  CourseClassrooms { get; set; }

        public int Count => CourseClassrooms.Count;
    }
}
