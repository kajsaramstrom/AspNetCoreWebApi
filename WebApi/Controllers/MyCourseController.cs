using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MyCourseController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    #region CREATE
    [HttpPost]
    public async Task<IActionResult> Create(MyCourseDto myCourses)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == myCourses.UserEmail);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == myCourses.CourseId);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            var myCourse = new MyCourseEntity
            {
                User = user,
                Course = course
            };

            await _context.MyCourse.AddAsync(myCourse);
            await _context.SaveChangesAsync();

            return Created("Course added to user", myCourse);
        }
        return BadRequest();
    }
    #endregion

    #region GET
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var myCourse = await _context.MyCourse.FindAsync(id);

        if (myCourse == null)
        {
            return NotFound("Course not found");
        }

        return Ok(myCourse);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var myCourses = await _context.MyCourse.ToListAsync();

        return Ok(myCourses);
    }
    #endregion

    #region DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var myCourse = await _context.MyCourse.FindAsync(id);

        if (myCourse == null)
        {
            return NotFound("Course not found");
        }

        _context.MyCourse.Remove(myCourse);
        await _context.SaveChangesAsync();

        return Ok("Course removed from user");
    }
    #endregion
}
