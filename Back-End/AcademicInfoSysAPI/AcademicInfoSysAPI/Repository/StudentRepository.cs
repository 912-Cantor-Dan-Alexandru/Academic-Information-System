﻿using AcademicInfoSysAPI.dbContext;
using AcademicInfoSysAPI.DTOs;
using AcademicInfoSysAPI.TempDir;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicInfoSysAPI.Repository
{

    public interface IStudentRepository
    {
        Task<Student> GetInfo(int StudId);
    }
    public class StudentRepository : IStudentRepository
    {
        private readonly AcademicInformationSystemContext _dbContext;

        public StudentRepository(AcademicInformationSystemContext someContext)
        {
            _dbContext = someContext;
        }

        public async Task<Student> GetInfo(int StudId)
        {
            return await _dbContext.Students.Where(x => x.StudId == StudId).FirstOrDefaultAsync();
        }
    }
}
