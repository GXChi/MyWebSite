using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MyWebSite.Application.UserApp.Dto;
using MyWebSite.Domain.Entities;
using MyWebSite.Domain.IRepositories;

namespace MyWebSite.Application.UserApp
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;

        public UserAppService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public User CheckUser(string userName, string password)
        {
            return _userRepository.CheckUser(userName, password);
            
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public UserDto Insert(UserDto dto)
        {
            var user = _userRepository.Insert(Mapper.Map<User>(dto));
            return Mapper.Map<UserDto>(user);
        }
    }
}
