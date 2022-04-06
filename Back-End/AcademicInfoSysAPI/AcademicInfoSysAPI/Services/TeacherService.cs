﻿using AcademicInfoSysAPI.DTOs;
using AcademicInfoSysAPI.Repository;
using System;
using System.Threading.Tasks;

namespace AcademicInfoSysAPI.Services
{
    public interface ITeacherService
    {
        Task<TeacherDTO> GetTeacherInfoForID(string id);
        Task<bool> UpdateTeacherInfoForID(TeacherDTO data);
    }
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherService(ITeacherRepository some_repo)
        {
            _teacherRepository = some_repo;
        }
        public async Task<TeacherDTO> GetTeacherInfoForID(string id)
        {
            var userInfo = await _teacherRepository.GetInfo(Int32.Parse(id));
            return new TeacherDTO
            {
                Cnp = userInfo.Cnp,
                TeacherId = userInfo.TeacherId,

            };
        }
        public async Task<bool> UpdateTeacherInfoForID(TeacherDTO data)
        {
            if (await _teacherRepository.UpdateTeacherInfoForID(data))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}