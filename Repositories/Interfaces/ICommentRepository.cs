using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repositories.Helpers;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync(CommentQueryObject query);
        Task<Comment> PostAsync(Comment commentModel);
        Task<Comment?> DeleteAsync(int id);
        Task<Comment?> GetByIdAsync(int id);
        Task<bool> CommentExists(int id);

    }
}
