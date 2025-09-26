using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email {  get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();

    }
}
