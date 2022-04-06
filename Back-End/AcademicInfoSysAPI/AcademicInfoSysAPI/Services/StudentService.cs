﻿using AcademicInfoSysAPI.DTOs;
using AcademicInfoSysAPI.Repository;
using System;
using System.Threading.Tasks;

namespace AcademicInfoSysAPI.Services
{
    public interface IStudentService
    {
        Task<StudentDTO> GetStudentInfoForID(string id);
    }
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository some_repo)
        {
            _studentRepository = some_repo;
        }
        public async Task<StudentDTO> GetStudentInfoForID(string id)
        {
            var userInfo = await _studentRepository.GetInfo(Int32.Parse(id));
            return new StudentDTO{
                Cnp = userInfo.Cnp,
                StudentId = userInfo.StudId,

            };
        }
    }
}