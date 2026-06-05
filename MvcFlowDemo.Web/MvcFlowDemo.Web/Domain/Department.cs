using System;
using System.ComponentModel.DataAnnotations;

namespace MvcFlowDemo.Web.Domain
{
    public class Department
    {
        
        public int Id { get; set; }

   
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}