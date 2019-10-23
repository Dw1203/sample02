using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sample02.Dtos.PasternDtos;
using sample02.IService;
using sample02.Model;

namespace sample02.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PasternController : ControllerBase
    {
        private readonly IPasternService _pasternService;
        private readonly IMapper _mapper;
        public PasternController(IPasternService pasternService,IMapper mapper)
        {
            _pasternService = pasternService;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加系部信息
        /// </summary>
        /// <param name="createPastern"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddPastern([FromBody] CreatePasternDto  createPastern)
        {
            if(createPastern==null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(await _pasternService.IsExsit(createPastern.Name,createPastern.PasternNum))
            {
                return BadRequest("系部名称或编号已存在");
            }
            var res = _mapper.Map<Pastern>(createPastern);
            _pasternService.AddT(res);
            if(!await _pasternService.Save())
            {
                return StatusCode(500, "添加系部信息时出错");
            }
            return Created("", createPastern);
        }

        /// <summary>
        /// 获取所有系部信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPastern()
        {
            var psn = await _pasternService.GetAll();
            if(psn.Count==0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<PasternDto>>(psn);
            return Ok(res);            
        }

        /// <summary>
        /// 获取所有系部详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPasternDetail()
        {
            var psn = await _pasternService.GetPasternDetail();
            if (psn.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<PasternDetailDto>>(psn);
            return Ok(res);
        }

        /// <summary>
        /// 根据系部名称获取系部信息
        /// </summary>
        /// <param name="PasName">系部名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPasternByName(string PasName)
        {
            var psn = await _pasternService.GetPasternsByPasName(PasName);
            if (psn.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<PasternDetailDto>>(psn);
            return Ok(res);
        }

        /// <summary>
        /// 根据系部编号获取系部信息
        /// </summary>
        /// <param name="PasNum">系部编号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPasternByNum(string PasNum)
        {
            var psn = await _pasternService.GetPasternsByPasNum(PasNum);
            if (psn.Count == 0)
            {
                return NotFound();
            }
            var res = _mapper.Map<List<PasternDetailDto>>(psn);
            return Ok(res);
        }


        /// <summary>
        /// 修改系部信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifyPasternDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdatePastern(int id,[FromBody]ModifyPasternDto modifyPasternDto)
        {

            if(modifyPasternDto==null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var psn = await _pasternService.GetTById(id);
            if(psn==null)
            {
                return NotFound();
            }
            if(psn.Name!=modifyPasternDto.Name)
            {
                var psn1 = await _pasternService.GetPasternsByPasName(modifyPasternDto.Name);
                if(psn1.Count>0)
                {
                    return BadRequest("系部名称已存在");
                }
            }
            if(psn.PasternNum!=modifyPasternDto.PasternNum)
            {
                var psn2 = await _pasternService.GetPasternsByPasNum(modifyPasternDto.PasternNum);
                if(psn2.Count>0)
                {
                    return BadRequest("系部编号已存在");
                }
            }
            psn.Name = modifyPasternDto.Name;
            psn.PasternNum = modifyPasternDto.PasternNum;
            _mapper.Map(psn, psn);
            if(! await _pasternService.Save())
            {
                return StatusCode(500, "更新系部信息时出错");
            }
            return Created("", modifyPasternDto);
        }
        
        /// <summary>
        /// 删除系部信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeletePastern(int id)
        {
            var psn = await _pasternService.GetTById(id);
            if(psn==null)
            {
                return NotFound();
            }
            _pasternService.DeleteT(psn);
            if(!await _pasternService.Save())
            {
                return StatusCode(500, "删除系部信息出错");
            }
            return NoContent();
        }
    }  
}