using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sample02.Dtos.MajorDtos;
using sample02.IService;
using sample02.Model;

namespace sample02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MajorController : ControllerBase
    {
        private readonly IMajorService _majorService;
        private readonly IMapper _mapper;
        private readonly IPasternService _pasternService;

        public MajorController(IMajorService majorService, IMapper mapper,IPasternService pasternService)
        {
            _majorService = majorService;
            _mapper = mapper;
            _pasternService = pasternService;
        }

        /// <summary>
        /// 添加专业
        /// </summary>
        /// <param name="createMajorDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddMajor([FromBody]CreateMajorDto createMajorDto)
        {
            if(createMajorDto==null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(await _majorService.IsExist(createMajorDto.MajorName,createMajorDto.MajorNum))
            {
                return BadRequest("专业名称或编号已存在");
            }
            var res = _mapper.Map<Major>(createMajorDto);
            _majorService.AddT(res);
            if(! await _majorService.Save())
            {
                return StatusCode(500, "添加专业时出现错误");
            }
            return Created("", createMajorDto);
        }

        /// <summary>
        /// 获取所有专业信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMajor()
        {
            var mjs = await _majorService.GetAll();
            if(mjs.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<MajorDto>>(mjs);
            return Ok(res);
        }

        /// <summary>
        /// 获取所有专业详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMajorDetail()
        {
            var mjs = await _majorService.GetAllMajorsDetail();
            if (mjs.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<MajorDetailDto>>(mjs);
            return Ok(res);
        }

        /// <summary>
        /// 根据专业名称获取专业信息
        /// </summary>
        /// <param name="Name">专业名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMajorDetailByMjName(string Name)
        {
            var mjs = await _majorService.GetMajorsByMjName(Name);
            if (mjs.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<MajorDetailDto>>(mjs);
            return Ok(res);
        }

        /// <summary>
        /// 根据专业编号获取专业信息
        /// </summary>
        /// <param name="Num">专业编号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMajorDetailByMjNum(string Num)
        {
            var mjs = await _majorService.GetMajorsByMjNum(Num);
            if (mjs.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<MajorDetailDto>>(mjs);
            return Ok(res);
        }

        /// <summary>
        /// 更新专业信息
        /// </summary>
        /// <param name="id">专业id</param>
        /// <param name="modifyMajor"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateMajor(int id,[FromBody]ModifyMajorDto modifyMajor)
        {
            if(modifyMajor == null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mj =await _majorService.GetTById(id);
            if(mj==null)
            {
                return NotFound();
            }
            if(mj.MajorName!=modifyMajor.MajorName)
            {
               if(await _majorService.MjNameIsExist(modifyMajor.MajorName))                
                {
                    return BadRequest("专业名称已存在");
                }
            }
            if(mj.MajorNum != modifyMajor.MajorNum)
            {
               if( await _majorService.MjNumIsExist(modifyMajor.MajorNum))                
                {
                    return BadRequest("专业编号已存在");
                }
            }
            mj.MajorName = modifyMajor.MajorName;
            mj.MajorNum = modifyMajor.MajorNum;
            _mapper.Map(mj, mj);
            if(! await _majorService.Save())
            {
                return StatusCode(500, "更新专业信息时出错");
            }
            return Created("", modifyMajor);
        }

        /// <summary>
        /// 删除专业信息
        /// </summary>
        /// <param name="id">专业id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteMajor(int id)
        {
            var mj =await _majorService.GetTById(id);
            if(mj==null)
            {
                return NotFound();
            }
            _majorService.DeleteT(mj);
            if(!await _majorService.Save())
            {
                return StatusCode(500, "删除专业信息时出错");
            }
            return NoContent();
        }

        /// <summary>
        /// 将专业添加到系部
        /// </summary>
        /// <param name="MajorName">专业名称</param>
        /// <param name="PasternName">系部名称</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> AddMajorToPastern(string MajorName,string PasternName)
        {
            if(!await _majorService.MjNameIsExist(MajorName))
            {
                return BadRequest("专业名称不存在");
            }
            if(!await _pasternService.PasNameIsExist(PasternName))
            {
                return BadRequest("系部名称不存在");
            }
            var psns = await _pasternService.GetPasternsByPasName(PasternName);
            var mjs = await (_majorService.GetMajorsByMjName(MajorName));
            var mj = mjs.FirstOrDefault();
            mj.Pastern = psns.FirstOrDefault();
            _mapper.Map(mj, mj);
            if(!await _majorService.Save())
            {
                return StatusCode(500, "将专业添加到系部时出错");
            }
            return Created("", null);
        }
    }
}