using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Helpers;
using Repositories.Interfaces;
using Repositories.Models;
using Services.ApiResponse;
using Services.DTOs.Comment;
using Services.DTOs.Stock;
using Services.Interfaces;
using Services.Mappers;

namespace Services.Implements
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentService(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        public async Task<ApiResponseModel<CommentDTO?>> DeleteComment(int id)
        {
            var response = new ApiResponseModel<CommentDTO?>()
            {
                Success = false,
                Data = null
            };
            var comment = await _commentRepo.DeleteAsync(id);
            if (comment is null)
            {
                response.Message = "Comment does not exist!";
                return response;
            }
            response.Data = comment.ToCommentDTO();
            response.Success = true;
            return response;
        }

        public async Task<ApiResponseModel<IEnumerable<CommentDTO>>> GetAllComments(CommentQueryObject query)
        {
            var response = new ApiResponseModel<IEnumerable<CommentDTO>>()
            {
                Success = false,
                Data = null
            };

            var comments = await _commentRepo.GetAllAsync(query);
            if (comments is null)
            {
                response.Message = "List of comments is empty!";
                return response;
            }

            response.Data = comments.Select(c => c.ToCommentDTO());
            response.Success = true;
            return response;
        }

        public async Task<ApiResponseModel<CommentDTO?>> GetCommentById(int id)
        {
            var response = new ApiResponseModel<CommentDTO?>()
            {
                Success = false,
                Data = null
            };
            var comment = await _commentRepo.GetByIdAsync(id);
            if(comment is null)
            {
                response.Message = "Comment does not exist!";
                return response;
            }

            response.Data = comment.ToCommentDTO();
            response.Success = true;
            return response;
        }

        public async Task<ApiResponseModel<CommentDTO>> PostComment(CreateCommentDTO commentDTO, int stockId, AppUser user)
        {
            var response = new ApiResponseModel<CommentDTO>()
            {
                Success = false,
                Data = null
            };

            var comment = await _commentRepo.PostAsync(commentDTO.ToCommentFromCreate(stockId, user));
            response.Data = comment.ToCommentDTO();
            response.Success= true;
            return response;
        }
    }
}
