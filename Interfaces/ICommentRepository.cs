using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<Comment> SaveChangesAsync(Comment comment);
        Task<Comment?> UpdateCommentAsync(int id, Comment updateComment);
        Task<Comment?> DeleteCommentAsync( int id);
    
    }
}