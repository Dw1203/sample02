using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sample02.Dtos.AchievementDtos;
using sample02.IService;
using sample02.Model;

namespace sample02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly IAchievementService _achievementService;
        private readonly IMapper _mapper;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        public AchievementController(IAchievementService achievementService,IMapper mapper,IStudentService studentService,ICourseService courseService)
        {
            _achievementService = achievementService;
            _mapper = mapper;
            _studentService = studentService;
            _courseService = courseService;
        }

        /// <summary>
        /// 获取所有成绩信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAchievement()
        {
            var avs = await _achievementService.GetAllAchievementDetail();
            if(avs==null)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<AchievementDto>>(avs);
            return Ok(res);
        }

        /// <summary>
        /// 根据学生姓名获取学生成绩
        /// </summary>
        /// <param name="Name">学生姓名</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAchievementsByStuName(string Name)
        {
            var avs = await _achievementService.GetAchievementsByStuName(Name);
            if(avs.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<AchievementDto>>(avs);
            return Ok(res);
        }

        /// <summary>
        /// 根据学生编号获取学生成绩
        /// </summary>
        /// <param name="Num">学生学号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAchievementByStuNum(string Num)
        {
            var avs = await _achievementService.GetAchievementsByStuNum(Num);
            if(avs.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<AchievementDto>>(avs);
            return Ok(res);
        }
    
        /// <summary>
        /// 根据课程名称获取学生成绩
        /// </summary>
        /// <param name="Name">课程名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAchievementByCourseName(string Name)
        {
            var avs =await _achievementService.GetAchievementsByCourseName(Name);
            if(avs.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<AchievementDto>>(avs);
            return Ok(res);
        }

        /// <summary>
        /// 根据课程编号获取学生成绩
        /// </summary>
        /// <param name="Num">课程编号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAchievementByCourseNum(string Num)
        {
            var avs = await _achievementService.GetAchievementsByCourseNum(Num);
            if (avs.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<AchievementDto>>(avs);
            return Ok(res);
        }

        /// <summary>
        /// 添加成绩信息
        /// </summary>
        /// <param name="createAchievementDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddAchievement([FromBody] CreateAchievementDto createAchievementDto)
        {
            var stu = await _studentService.GetStudentDetailByStuName(createAchievementDto.StuName);
            var cs = await _courseService.GetCourseByCourseNameDetail(createAchievementDto.CourseName);
            if(stu.Count==0||cs.Count==0)
            {
                return BadRequest("学生或者课程不存在");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Achievement achievement = new Achievement();
            achievement.Student = stu.FirstOrDefault();
            achievement.Course = cs.FirstOrDefault();
            achievement.Score = createAchievementDto.Score;
            _achievementService.AddT(achievement);
            if(!await _achievementService.Save())
            {
                return StatusCode(500, "添加信息时出错");
            }
            return Created("", "");
        }

        /// <summary>
        /// 更新成绩信息
        /// </summary>
        /// <param name="modify"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateAchievement([FromBody] ModifyAchievement modify)
        {
           
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var acs= await _achievementService.GetAchievementsByStuNameAndCsName(modify.StuName, modify.CourseName);
            if(acs==null)
            {
                return NotFound();
            }           
            acs.Score = modify.Score;          
            _mapper.Map(acs,acs);
            if (!await _achievementService.Save())
            {
                return StatusCode(500, "更新数据时出错");
            }
            return Created("", null);

        }

        /// <summary>
        /// 删除一条成绩记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAchievement(int id)
        {
            var acs =await _achievementService.GetTById(id);
            if(acs==null)
            {
                return NotFound();
            }
            _achievementService.DeleteT(acs);
            if(! await _achievementService.Save())
            {
                return StatusCode(500, "删除数据时出错");
            }
            return NoContent();
        }
    }

}