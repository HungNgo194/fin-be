using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Helpers;
using Repositories.Models;
using Services.ApiResponse;
using Services.DTOs.Comment;

namespace Services.Interfaces
{
    public interface ICommentService
    {
        Task<ApiResponseModel<IEnumerable<CommentDTO>>> GetAllComments(CommentQueryObject query);
        Task<ApiResponseModel<CommentDTO?>> GetCommentById(int id);
        Task<ApiResponseModel<CommentDTO>> PostComment(CreateCommentDTO stockDTO, int id, AppUser user);
        Task<ApiResponseModel<CommentDTO?>> DeleteComment(int id);

    }
}
