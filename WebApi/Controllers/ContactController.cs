using Infrastructure.Context;
using Infrastructure.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;

    #region CREATE
    [HttpPost]
    [UseApiKey]
    public async Task<IActionResult> Create(ContactDto contactDto)
    {
        if (ModelState.IsValid)
        {
            var contactForm = new ContactEntity
            {
                FullName = contactDto.FullName,
                Email = contactDto.Email,
                Service = contactDto.Service,
                Message = contactDto.Message
            };

            await _context.Contacts.AddAsync(contactForm);
            await _context.SaveChangesAsync();

            return Created("Contact form was created and sent", contactForm);
        }

        return BadRequest("Invalid fields in the form, please try again.");
    }
    #endregion

    #region GET
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var contactForm = await _context.Contacts.FindAsync(id);

        if (contactForm == null)
        {
            return NotFound("Contact form not found.");
        }

        return Ok(contactForm);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var contactForms = await _context.Contacts.ToListAsync();

        if (contactForms.Count == 0)
        {
            return NotFound("No contacts form found.");
        }

        return Ok(contactForms);
    }

    #endregion

    #region UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ContactDto dto)
    {
        var contactForm = await _context.Contacts.FindAsync(id);

        if (contactForm == null)
        {
            return NotFound("Contact form not found");
        }

        contactForm.FullName = dto.FullName;
        contactForm.Email = dto.Email;
        contactForm.Service = dto.Service;
        contactForm.Message = dto.Message;

        await _context.SaveChangesAsync();

        return Ok("Contact form was updated");
    }
    #endregion

    #region DELETE
    [HttpDelete("{id}")]

    public async Task<IActionResult> Delete(int id)
    {
        var contactForm = await _context.Contacts.FindAsync(id);

        if (contactForm == null)
        {
            return NotFound("Contact form not found");
        }

        _context.Contacts.Remove(contactForm);
        await _context.SaveChangesAsync();

        return Ok("Contact form was deleted");
    }

    #endregion
}
