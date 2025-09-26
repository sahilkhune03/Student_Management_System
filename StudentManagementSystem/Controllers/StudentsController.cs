using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.Include(s => s.Courses).ToListAsync();
            return View(students);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.CourseList = new MultiSelectList(await _context.Courses.ToListAsync(), "CourseId", "CourseName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student, int[] selectedCourses)
        {
            foreach (var courseId in selectedCourses)
            {
                var course = await _context.Courses.FindAsync(courseId);
                if (course != null)
                    student.Courses.Add(course);
            }

            _context.Add(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _context.Students.Include(s => s.Courses).FirstOrDefaultAsync(s => s.StudentId == id);
            ViewBag.CourseList = new MultiSelectList(await _context.Courses.ToListAsync(), "CourseId", "CourseName", student.Courses.Select(c => c.CourseId));
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student, int[] selectedCourses)
        {
            var studentToUpdate = await _context.Students.Include(s => s.Courses).FirstOrDefaultAsync(s => s.StudentId == id);
            studentToUpdate.Name = student.Name;
            studentToUpdate.Email = student.Email;
            studentToUpdate.Courses.Clear();

            foreach (var courseId in selectedCourses)
            {
                var course = await _context.Courses.FindAsync(courseId);
                if (course != null)
                    studentToUpdate.Courses.Add(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }


}

