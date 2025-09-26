using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required, StringLength(50)]
        public string CourseName { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
