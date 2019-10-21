using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using sample02.Dtos.StudentCourseDtos;
using sample02.IService;
using sample02.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sample02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentCourseController : ControllerBase
    {
        private readonly IStudentCourseService _studentCourseService;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;
        public StudentCourseController(IStudentCourseService studentCourseService,IMapper mapper,
            IStudentService studentService,ICourseService courseService)
        {
            _studentCourseService = studentCourseService;
            _mapper = mapper;
            _studentService = studentService;
            _courseService = courseService;
        }

        /// <summary>
        /// 获取所有学生课程信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllStuCrs()
        {
            var StuCrs = await _studentCourseService.GetAllStuCrsDetail();
            if(StuCrs.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<StudentCourseDto>>(StuCrs);
            return Ok(res);
        }

        /// <summary>
        /// 根据学生姓名获取课程信息
        /// </summary>
        /// <param name="StuName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStuCrsByStuName(string StuName)
        {
            var StuCrs = await _studentCourseService.GetStudentCoursesByStuName(StuName);
            if(StuCrs.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<StudentCourseDto>>(StuCrs);
            return Ok(res);
        }

        /// <summary>
        /// 根据课程名称获取课程信息
        /// </summary>
        /// <param name="CourseName">课程名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStuCrsByCourseName(string CourseName)
        {
            var StuCrs = await _studentCourseService.GetStudentCoursesByCourseName(CourseName);
            if (StuCrs.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<StudentCourseDto>>(StuCrs);
            return Ok(res);
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="createStudentCourse"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddStudentCourse([FromBody] CreateStudentCourseDto createStudentCourse)
        {
            if(createStudentCourse==null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stu = await _studentService.GetStudentDetailByStuName(createStudentCourse.StudentName);
            var course = await _courseService.GetCourseByCourseNameDetail(createStudentCourse.CourseName);
            if(stu.Count==0 || course.Count==0)
            {
                return BadRequest("学生或者课程不存在");
            }
            if(stu.Count>1||course.Count>1)
            {
                return BadRequest("学生姓名或者课程名存在多条记录");
            }
            StudentCourse studentCourse = new StudentCourse()
            {
                Student = stu.FirstOrDefault(),
                Course = course.FirstOrDefault()
            };
             _studentCourseService.AddT(studentCourse);
            if(! await _studentCourseService.Save())
            {
                return StatusCode(500, "添加记录时出错");
            }
            return Created("", createStudentCourse);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="deleteStudent"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteStuCourse([FromBody] DeleteStudentCourseDto deleteStudent)
        {
            if(deleteStudent == null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stu =await _studentCourseService.GetStudentCourseByStuNameAndCourseName(deleteStudent.StudentName, deleteStudent.CourseName);
            if(stu==null)
            {
                return NotFound();
            }
            _studentCourseService.DeleteT(stu);
            if(! await _studentCourseService.Save())
            {
                return StatusCode(500, "删除记录时出错");
            }
            return NoContent();
        }
    }
}