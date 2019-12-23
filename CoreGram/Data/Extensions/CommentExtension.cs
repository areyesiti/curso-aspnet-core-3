using CoreGram.Data.Dto;
using CoreGram.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreGram.Data.Extensions
{
    public static class CommentExtension
    {        
        /// <summary>
        /// Método de extensión para mapear un Comment a un CommentDto
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static CommentDto ToDto(this Comment comment, int postId)
        {
            return new CommentDto
            {
                Id = comment.Id,
                PostId = postId,
                UserId = comment.UserId,
                Remark = comment.Remark
            };
        }
    }
}
