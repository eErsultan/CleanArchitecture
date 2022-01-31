using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Constants;
using Application.DTOs.Common;
using Application.DTOs.Article;
using Application.Features.Articles.Commands.Create;
using Application.Features.Articles.Commands.Delete;
using Application.Features.Articles.Commands.Update;
using Application.Features.Articles.Queries.GetAll;
using Application.Features.Articles.Queries.GetById;
using WebAPI.Attributes;
using WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using System;

namespace WebAPI.Controllers
{
    public class ArticlesController : ApiControllerBase
    {
        /// <summary>
        /// Получить все статьи
        /// </summary>
        /// <response code="200">Успешная операция</response>
        [ProducesResponseType(typeof(ICollection<ArticleDto>), StatusCodes.Status200OK)]
        [HttpGet(ApiRoutes.Article.GetAll)]
        //[Cached("articles")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllArticlesQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        /// <summary>
        /// Получить по id статьи
        /// </summary>
        /// <response code="200">Успешная операция</response>
        /// <response code="404">Статья не найдена</response>
        [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [HttpGet(ApiRoutes.Article.GetById)]
        //[Cached("articleById")]
        public async Task<IActionResult> GetById([FromRoute] int articleId)
        {
            return Ok(await Mediator.Send(new GetByIdArticleQuery(){ArticleId = articleId}));
        }

        /// <summary>
        /// Создать статью
        /// </summary>
        /// <response code="200">Успешная операция</response>
        /// <response code="400">Name является обязательным</response>
        [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.Article.Create)]
        //[RemoveCache("[articles]")]
        public async Task<IActionResult> Create(CreateArticleCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Удалить статью
        /// </summary>
        /// <response code="200">Успешная операция</response>
        /// <response code="404">Статья не найдена</response>
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [HttpDelete(ApiRoutes.Article.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int articleId)
        {
            return Ok(await Mediator.Send(new DeleteArticleCommand(){ArticleId = articleId}));
        }

        /// <summary>
        /// Редактировать статью
        /// </summary>
        /// <response code="200">Успешная операция</response>
        /// <response code="404">Статья не найдена</response>
        [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status404NotFound)]
        [HttpPut(ApiRoutes.Article.Update)]
        public async Task<IActionResult> Update([FromRoute] int articleId, UpdateArticleCommand command)
        {
            command.ArticleId = articleId;
            return Ok(await Mediator.Send(command));
        }
    }
}