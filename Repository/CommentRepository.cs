using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {

        private readonly ApplicationDBContext _context; // session 

        public CommentRepository(ApplicationDBContext context)
        {
            _context= context;
        }

    

        public async  Task<Comment?> DeleteCommentAsync(int id)
        {
           var comment= await _context.Comments.FirstOrDefaultAsync(c => c.Id==id);
           if( comment==null)
           {
            return null;
           }
           _context.Remove(comment);
           await _context.SaveChangesAsync();
           return comment;
        }

     

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public Task<Comment?> GetCommentByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Comment> SaveChangesAsync(Comment comment)
        {
            await  _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> UpdateCommentAsync(int id, Comment updateComment)
        {
           var existingComment= await _context.Comments.FindAsync(id);
           if (existingComment==null)
           {
            return null;
           }

           existingComment.Title=updateComment.Title;
           existingComment.Content=updateComment.Content;

           await _context.SaveChangesAsync();

           return existingComment;
        }

    }
}