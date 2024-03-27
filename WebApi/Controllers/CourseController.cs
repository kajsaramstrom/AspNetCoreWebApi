using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CourseController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    #region Create

    [HttpPost]

    public async Task<IActionResult> Create(CourseDto course)
    {
        if (ModelState.IsValid)
        {
            if (!await _context.Courses.AnyAsync(x => x.Title == course.Title))
            {
                var newCourse = new CourseEntity
                {
                    Title = course.Title,
                    Price = course.Price,
                    DiscountPrice = course.DiscountPrice,
                    Hours = course.Hours,
                    IsBestseller = course.IsBestseller,
                    LikesInNumbers = course.LikesInNumbers,
                    LikesInPercent = course.LikesInPercent,
                    Author = course.Author,
                    CourseImageUrl = course.CourseImageUrl
                };
                await _context.Courses.AddAsync(newCourse);
                await _context.SaveChangesAsync();
                return Created("Course has been created", newCourse);

            }

            return Conflict("Course with the same title already exist");
        }

        return BadRequest();
    }


    #endregion


    #region GET

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _context.Courses.ToListAsync();

        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound("Subscriber not found");
        }

        return Ok(course);
    }

    #endregion

    #region UPDATE

    [HttpPut("{id}")]

    public async Task<IActionResult> Update(int id, CourseDto dto)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound("Course not found");
        }

        course.Title = dto.Title;
        course.Price = dto.Price;
        course.DiscountPrice = dto.DiscountPrice;
        course.Hours = dto.Hours;
        course.Author = dto.Author;
        course.IsBestseller = dto.IsBestseller;
        course.LikesInNumbers = dto.LikesInNumbers;
        course.LikesInPercent = dto.LikesInPercent;
        course.CourseImageUrl = dto.CourseImageUrl;

        await _context.SaveChangesAsync();

        return Ok("Subscriber was updated");
    }
    #endregion
}
