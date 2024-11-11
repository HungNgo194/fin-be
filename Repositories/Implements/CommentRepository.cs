using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repositories.Helpers;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implements
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync(CommentQueryObject query)
        {
            var comments = _context.Comment.Include(c => c.AppUser).AsQueryable();

            if (!string.IsNullOrEmpty(query.Symbol))
            { 
                comments = comments.Where(c => c.Stock.Symbol == query.Symbol);
            }

            if (query.IsDecsending)
            {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }
            return await comments.ToListAsync();
        }

        public async Task<Comment> PostAsync(Comment commentModel)
        {
            await _context.Comment.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comment.Include(c => c.AppUser).FirstOrDefaultAsync(x => x.Id == id);

            if (commentModel == null)
            {
                return null;
            }

            _context.Comment.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }


        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comment.Include(c => c.AppUser).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await _context.Comment.FindAsync(id);

            if (existingComment == null)
            {
                return null;
            }

            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;

            await _context.SaveChangesAsync();

            return existingComment;
        }

        public async Task<bool> CommentExists(int id)
        {
            return await _context.Comment.Include(c => c.AppUser).AnyAsync(x => x.Id == id);
        }
    }
}
