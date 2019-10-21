using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using sample02.Dtos.StuClassDtos;
using sample02.IService;
using sample02.Model;

namespace sample02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StuClassController : ControllerBase
    {

        private readonly IStuClassService _stuClassService;
        private readonly IMapper _mapper;        
        public StuClassController(IStuClassService stuClassService,IMapper mapper,IStudentService studentService)
        {
            _stuClassService = stuClassService;
            _mapper = mapper;         
        }

        /// <summary>
        /// 获取所有班级信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllStuClass()
        {
            var stuclasses = await _stuClassService.GetAll();
            if(stuclasses==null)
            {
                return NotFound();
            }
            var result = _mapper.Map<List<StuClassDto>>(stuclasses);
            return Ok(result);
        }

        /// <summary>
        /// 获取所有班级的详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllStuClassDetail()
        {
            var stuclasses = await _stuClassService.GetAllStuclassDetail();
            if(stuclasses==null)
            {
                return NotFound();
            }
            var result = _mapper.Map<List<StuClassDetail>>(stuclasses);
            return Ok(result);
        }

        /// <summary>
        /// 添加班级信息
        /// </summary>
        /// <param name="createStuClass"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddStuClass([FromBody] CreateStuClassDto createStuClass)
        {
            if(createStuClass==null)
            {
                return BadRequest("班级信息不能为空");
            }
            var cls = await _stuClassService.GetStuClassByClassNumDetail(createStuClass.ClassNum);
            var cls1 = await _stuClassService.GetStuClassByClassNameDetail(createStuClass.ClassName);
            if(cls!=null)
            {
                ModelState.AddModelError("ClassNum", "班级编号已存在");
            }
            if(cls1!=null)
            {
                ModelState.AddModelError("ClassName", "班级名称已存在");
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var res = _mapper.Map<StuClass>(createStuClass);
            _stuClassService.AddT(res);
            if(!await _stuClassService.Save())
            {
                return StatusCode(500, "添加班级信息失败");
            }
            return Created("", createStuClass);
        }

        /// <summary>
        /// 删除班级信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeteleStuClass(int id)
        {
            var stuclass =await _stuClassService.GetTById(id);
            if(stuclass==null)
            {
                return NotFound();
            }
            _stuClassService.DeleteT(stuclass);
            if(!await _stuClassService.Save())
            {
                return StatusCode(500, "删除班级信息失败");
            }
            return NoContent();
        }

        /// <summary>
        /// 修改班级信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifyStuClassDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateStuClass(int id,[FromBody] ModifyStuClassDto modifyStuClassDto)
        {
            if(modifyStuClassDto==null)
            {
                return BadRequest();
            }
            var cls = await _stuClassService.GetStuClassByClassNumDetail(modifyStuClassDto.ClassNum);
            var cls1 = await _stuClassService.GetStuClassByClassNameDetail(modifyStuClassDto.ClassName);
            if (cls != null && cls.ClassNum != modifyStuClassDto.ClassNum)
            {
                ModelState.AddModelError("ClassNum", "班级编号已存在");
            }
            if (cls1 != null && cls1.ClassName != modifyStuClassDto.ClassName)
            {
                ModelState.AddModelError("ClassName", "班级名称已存在");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var stuClass =await _stuClassService.GetTById(id);
            if(stuClass ==null)
            {
                return NotFound();
            }
            _mapper.Map(modifyStuClassDto, stuClass);
            if(! await _stuClassService.Save())
            {
                return StatusCode(500, "更新班级信息失败");
            }
            return Created("", modifyStuClassDto);
        }

        
    } 
}
