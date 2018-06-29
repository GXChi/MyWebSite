using AutoMapper;
using MyWebSite.Application.ArticleApp.Dtos;
using MyWebSite.Application.UserApp.Dto;
using MyWebSite.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebSite.Application
{
    public class MyMapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ArticleDto, Article>();
                cfg.CreateMap<Article, ArticleDto>();
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<User, UserDto>();
            });
        }
    }
}
