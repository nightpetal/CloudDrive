using System.Data;
using CloudDrive.Application.DTOs.UserDTOs;
using CloudDrive.Application.Interfaces;
using CloudDrive.Application.Interfaces.Services;
using CloudDrive.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CloudDrive.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }


    }
}