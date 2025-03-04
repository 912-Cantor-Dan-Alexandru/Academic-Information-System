﻿using AcademicInfoSysAPI.Context;
using AcademicInfoSysAPI.Context.Models;
using AcademicInfoSysAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicInfoSysAPI.Repository
{
    public interface IDisciplineRepository
    {
        Task<List<OptionalDiscipline>> GetOptionalDisciplines(int year);
        Task<List<StandardDiscipline>> GetStandardDisciplines(int year);
        Task<List<OptionalDisciplineList>> GetAssignedOptionalDisciplinesList(int stud_id);
        Task<List<OptionalDiscipline>> GetAssignedOptionalDisciplines(List<OptionalDisciplineList> optionalDisciplineLists);
        Task<List<AssignedCourseDTO>> GetAssignedOptionalDisciplinesForDTO(List<OptionalDiscipline> CoursesIdList);
        Task<bool> InsertTemporaryOptional(OptionalDisciplineList odl);
        Task<List<OptionalDiscipline>> GetOptionalDisciplinesSortedByPriority(int studentId);
        Task<Student> GetStudentById(int studentId);
    
        Task<bool> MakeOptionalFinal(int studentId, int optionalId);
        Task<List<OptionalDiscipline>> GetProposedOptionalsByTeacher(int teacherId);
    }
    public class DisciplineRepository : IDisciplineRepository
    {
        private readonly AcademicInfoSysAPI_dbContext _dbContext;

        public DisciplineRepository(AcademicInfoSysAPI_dbContext someContext)
        {
            _dbContext = someContext;
        }

        public async Task<List<OptionalDiscipline>> GetAssignedOptionalDisciplines(List<OptionalDisciplineList> optionalDisciplineLists)
        {
            List<OptionalDiscipline> response = new();

            foreach (var discipline in optionalDisciplineLists)
            {
                response.Add(await _dbContext.OptionalDisciplines.Include(x => x.Teacher).FirstOrDefaultAsync(x => x.Id == discipline.OptionalDisciplineId));
            }
            return response;
        }

        public async Task<List<AssignedCourseDTO>> GetAssignedOptionalDisciplinesForDTO(List<OptionalDiscipline> CoursesList)
        {
            List<AssignedCourseDTO> response = new();

            foreach(var discipline in CoursesList)
            {
                var proffesor = await _dbContext.Teachers.FirstOrDefaultAsync(x => x.TeacherId == discipline.TeacherId);
                var proffesorName = string.Concat(proffesor.FirstName,' ', proffesor.LastName);
                response.Add(new AssignedCourseDTO { name = discipline.Name , proffesor = proffesorName});
            }

            return response.ToList();
        }

        public async Task<List<OptionalDisciplineList>> GetAssignedOptionalDisciplinesList(int stud_id)
        {
            return await _dbContext.OptionalDisciplineLists.Where(x => x.StudId == stud_id && x.Final == true)
                                                            .ToListAsync();
        }

        public async Task<List<OptionalDiscipline>> GetOptionalDisciplines(int year)
        {
            return await _dbContext.OptionalDisciplines.Where(x => x.CoresopondingYear == year).Include(x => x.Teacher).ToListAsync();
        }

        public async Task<List<StandardDiscipline>> GetStandardDisciplines(int year)
        {
            return await _dbContext.StandardDisciplines.Where(x => x.CorespondingYear == year).Include(x => x.Teacher).ToListAsync();
        }

        public async Task<List<OptionalDiscipline>> GetOptionalDisciplinesSortedByPriority(int studentId)
        {
            return await _dbContext.OptionalDisciplines.Join(
                _dbContext.OptionalDisciplineLists, od => od.Id, odl => odl.OptionalDisciplineId,
                (od, odl) => new { optionalDiscipline = od, optionalDisciplineList = odl }).Join(
                _dbContext.Students, odl => odl.optionalDisciplineList.StudId, s => s.StudId,
                (oddl, s) => new { discipline_list = oddl, stud = s}).Where(result => result.stud.StudId == studentId)
                .OrderBy(result => result.discipline_list.optionalDisciplineList.OrderPreference)
                .Select(result => new OptionalDiscipline
                {
                    Id = result.discipline_list.optionalDiscipline.Id,
                    Name = result.discipline_list.optionalDiscipline.Name,
                    Teacher = result.discipline_list.optionalDiscipline.Teacher,
                    NoCredits = result.discipline_list.optionalDiscipline.NoCredits,
                    NoStudents = result.discipline_list.optionalDiscipline.NoStudents
                }).ToListAsync();
                
        }

        public async Task<bool> InsertTemporaryOptional(OptionalDisciplineList odl)
        {
            var opt = await _dbContext.OptionalDisciplineLists.Where(optional => optional.StudId == odl.StudId && optional.OptionalDisciplineId == odl.OptionalDisciplineId).FirstOrDefaultAsync();
            if (opt != null) 
            {
                opt.OrderPreference = odl.OrderPreference;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            odl.Final = false;
            _dbContext.OptionalDisciplineLists.Add(odl);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Student> GetStudentById(int studentId)
        {
            return await _dbContext.Students.FirstOrDefaultAsync(x => x.StudId == studentId);
        }

        public async Task<bool> MakeOptionalFinal(int studentId, int optionalId)
        {
            var optionalToUpdate = await _dbContext.OptionalDisciplineLists.Where(opl => opl.StudId == studentId && opl.OptionalDisciplineId == optionalId).FirstAsync();
            if (optionalToUpdate != null)
            {
                optionalToUpdate.Final = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<OptionalDiscipline>> GetProposedOptionalsByTeacher(int teacherId)
        {
            return await _dbContext.OptionalDisciplines.Where(opl => opl.TeacherId == teacherId).ToListAsync();
        }
    }
}
