using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using sample02.Dtos.CourseDtos;
using sample02.IService;
using sample02.Model;

namespace sample02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseController : ControllerBase
    {

        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public CourseController(ICourseService courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取所有课程信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCourse()
        {
            var courses = await _courseService.GetAll();
            if (courses == null)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<CourseDto>>(courses);
            return Ok(res);
        }

        /// <summary>
        /// 获取所有课程的详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCourseDetail()
        {
            var courses = await _courseService.GetCourseAllDetail();
            if (courses == null)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<CourseDetailDto>>(courses);
            return Ok(res);
        }

        /// <summary>
        /// 根据课程名称获取课程详细信息
        /// </summary>
        /// <param name="Name">课程名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCourseDetailByName(string Name)
        {
            var courses = await _courseService.GetCourseByCourseNameDetail(Name);
            if (courses == null)
            {
                return null;
            }
            var res = _mapper.Map<List<CourseDetailDto>>(courses);
            return Ok(res);
        }

        /// <summary>
        /// 根据课程编号获取课程详细信息
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCourseDetailByNumber(string Num)
        {
            var courses = await _courseService.GetCourseByCourseNumDetail(Num);
            if (courses == null)
            {
                return null;
            }
            var res = _mapper.Map<List<CourseDetailDto>>(courses);
            return Ok(res);
        }

        /// <summary>
        /// 添加课程信息
        /// </summary>
        /// <param name="createCourseDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddCourse([FromBody] CreateCourseDto createCourseDto)
        {
            if (createCourseDto == null)
            {
                return BadRequest();
            }

            if (await _courseService.CourseIsExist(createCourseDto.CourseName, createCourseDto.CourseNum))
            {
                return BadRequest("课程名称或者编号已存在");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _mapper.Map<Course>(createCourseDto);
            _courseService.AddT(res);
            if (!await _courseService.Save())
            {
                return StatusCode(500, "添加课程信息失败");
            }
            return Created("", createCourseDto);
        }

        /// <summary>
        /// 修改课程信息
        /// </summary>
        /// <param name="id">课程id</param>
        /// <param name="modifyCourseDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] ModifyCourseDto modifyCourseDto)
        {

            var course = await _courseService.GetTById(id);
            if (course == null)
            {
                return NotFound();
            }
            if (modifyCourseDto == null)
            {
                return BadRequest();
            }
            var cs = await _courseService.GetCourseByCourseNameDetail(modifyCourseDto.CourseName);
            var cs1 = await _courseService.GetCourseByCourseNumDetail(modifyCourseDto.CourseNum);
            if (cs.Count > 1 || cs1.Count > 1)
            {
                return BadRequest("课程名称或者课程编号已存在");
            }
            var res = _mapper.Map(modifyCourseDto, course);
            if (!await _courseService.Save())
            {
                return StatusCode(500, "更新课程信息失败");
            }
            return Created("", modifyCourseDto);
        }

        /// <summary>
        /// 删除课程信息
        /// </summary>
        /// <param name="id">课程id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseService.GetTById(id);
            if (course == null)
            {
                return NotFound();
            }
            _courseService.DeleteT(course);
            if(!await _courseService.Save())
            {
                return StatusCode(500, "删除课程信息失败");
            }
            return NoContent();
        }
    }
}
