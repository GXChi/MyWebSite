using MyWebSite.Application.UserApp.Dtos;
using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Application.UserApp
{
    public interface IUserAppService
    {
        User CheckUser(string userName, string password);

        UserDto Insert(UserDto dto);

        List<UserDto> GetAll();

        
    }
}
