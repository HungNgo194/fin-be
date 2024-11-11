using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Models;
using Services.DTOs.Comment;

namespace Services.Mappers
{
    public static class CommentMappers
    {
        public static CommentDTO ToCommentDTO(this Comment commentModel)
        {
            return new CommentDTO
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId,
                CreateBy = commentModel.AppUser.UserName
            };
        }

        public static Comment ToCommentFromCreate(this CreateCommentDTO commentDto, int stockId, AppUser user)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId,
                AppUserId = user.Id,
                AppUser = user
            };
        }

        public static Comment ToCommentFromUpdate(this UpdateCommentRequestDTO commentDto, int stockId)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }
    }
}
