using sample02.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace sample02.Dtos.StudentDtos
{
    public class StudentDto
    {
        public int Id { get; set; }

        public string StuNum { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDay { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public DateTime EnmDate { get; set; }
    }
}
