using Application.DTOs.Article;
using Application.Features.Articles.Commands.Create;
using Application.Features.Articles.Commands.Update;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Article, ArticleDto>();
            CreateMap<CreateArticleCommand, Article>();
            CreateMap<UpdateArticleCommand, Article>();
        }
    }
}