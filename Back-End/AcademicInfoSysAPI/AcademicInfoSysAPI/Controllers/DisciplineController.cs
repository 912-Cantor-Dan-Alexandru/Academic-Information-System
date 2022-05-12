﻿using AcademicInfoSysAPI.Services;
using Microsoft.AspNetCore.Mvc;
using AcademicInfoSysAPI.DTOs;
using System.Threading.Tasks;
using System;

namespace AcademicInfoSysAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplineController : Controller
    {
        private IDisciplineService disciplineService;
        public DisciplineController(IDisciplineService service)
        {
            disciplineService = service;
        }

        [HttpGet("{year}")]
        public async Task<IActionResult> GetAllDisciplinesForYear(int year)
        {

            if (year != 1 && year != 2 && year != 3)
                return BadRequest();
            try
            {
                var disciplines = await disciplineService.GetAllDisciplinesForYear(year);
                if (disciplines == null)
                {
                    return NotFound();
                }
                return Ok(disciplines);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("assigned/{stud_id}")]
        public async Task<IActionResult> GetAssignedOptionalDisciplines([FromRoute] int stud_id)
        {
            if (stud_id < 1)
            {
                return BadRequest();
            }
            try
            {
                var optionalCourses = await disciplineService.GetAssignedOptionalDisciplines(stud_id);
                return Ok(optionalCourses);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllOptionalDisciplines()
        {
            try
            {
                var disciplines = await disciplineService.GetAllOptionalDisciplines();
                if (disciplines == null)
                {
                    return NotFound();
                }
                return Ok(disciplines);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("optional/{studentID}")]
        public async Task<IActionResult> GetOptionalDisciplinesSortedByPriority(int studentID)
        {
            try
            {
                var disciplines = await disciplineService.GetOptionalDisciplinesSortedByPriority(studentID);
                if (disciplines == null)
                {
                    return NotFound();
                }
                return Ok(disciplines);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost("temporary-optional")]
        public async Task<IActionResult> InsertTemporaryOptional([FromBody] OptionalTemporaryDTO data)
        {
            if (await disciplineService.InsertTemporaryOptional(data))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
