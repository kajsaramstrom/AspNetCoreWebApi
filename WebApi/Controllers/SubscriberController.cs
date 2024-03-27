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
public class SubscriberController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    #region CREATE
    [HttpPost]
    [UseApiKey]
    public async Task<IActionResult> Create(SubsriberDto subscriberDto)
    {
        if (ModelState.IsValid)
        {
            if (! await _context.Subscribers.AnyAsync(x => x.Email == subscriberDto.Email))
            {
                var subscriber = new SubscribeEntity
                {
                    Email = subscriberDto.Email,
                    DailyNewsletter = subscriberDto.DailyNewsletter,
                    AdvertisingUpdates = subscriberDto.AdvertisingUpdates,
                    WeekinReview = subscriberDto.WeekinReview,
                    EventUpdates = subscriberDto.EventUpdates,
                    StartupsWeekly = subscriberDto.StartupsWeekly,
                    Podcasts = subscriberDto.Podcasts
                };

                await _context.Subscribers.AddAsync(subscriber);
                await _context.SaveChangesAsync();

                return Created("Subscriber was created.", subscriber);
            }
            else
            {
                return Conflict("Subscriber already exists.");
            }
        }

        return BadRequest("Invalid email.");
    }
    #endregion

    #region READ

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subscribers = await _context.Subscribers.ToListAsync();

        return Ok(subscribers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id) 
    {
        var subscriber = await _context.Subscribers.FindAsync(id);

        if (subscriber == null)
        {
            return NotFound("Subscriber not found.");
        }

        return Ok(subscriber);
    }

    [HttpGet("email/{email}")]

    public async Task<IActionResult> FindByEmail(string email)
    {
        var sub = await _context.Subscribers.FirstOrDefaultAsync(x => x.Email == email);

        if (sub != null)
        {
            return Ok(sub);
        }

        return NotFound();
    }

    #endregion

    #region UPDATE

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SubsriberDto subsriberDto)
    {
        var subscriber = await _context.Subscribers.FindAsync(id);

        if (subscriber == null)
        {
            return NotFound("Subscriber not found");
        }

        subscriber.Email = subsriberDto.Email;
        subscriber.DailyNewsletter = subsriberDto.DailyNewsletter;
        subscriber.AdvertisingUpdates = subsriberDto.AdvertisingUpdates;
        subscriber.WeekinReview = subsriberDto.WeekinReview;
        subscriber.EventUpdates = subsriberDto.EventUpdates;
        subscriber.StartupsWeekly = subsriberDto.StartupsWeekly;
        subscriber.Podcasts = subsriberDto.Podcasts;

        await _context.SaveChangesAsync();

        return Ok("Subscriber was updated");
    }
    #endregion

    #region DELETE

    [HttpDelete("{id}")]
    [UseApiKey]
    public async Task<IActionResult> Delete(int id)
    {
        var subscriber = await _context.Subscribers.FindAsync(id);

        if (subscriber == null)
        {
            return NotFound("Subscriber not found.");
        }

        _context.Subscribers.Remove(subscriber);
        await _context.SaveChangesAsync();

        return Ok("Subscriber was deleted.");
    }

    [HttpDelete("email")]
    public async Task<IActionResult> DeleteWithEmail(string email)
    {
        var result = await FindByEmail(email);

        if (result is OkObjectResult okResult && okResult.Value is SubscribeEntity sub)
        {
            _context.Subscribers.Remove(sub);
            await _context.SaveChangesAsync();
            return Ok("Subscriber was deleted");
        }

        return result;
    }

    #endregion
}
