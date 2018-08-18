using System;

namespace MyBlog.BLL
{
    /// <summary>
    /// Info used for pagination
    /// </summary>
    public class PageInfo
    {
        public int PageNumber { get; set; } 
        public int PageSize { get; set; } 
        public int TotalItems { get; set; } 
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
}
