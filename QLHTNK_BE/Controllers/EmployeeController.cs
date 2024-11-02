﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHTNK_BE.Models;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly DentalCentreManagementContext _context;

    public EmployeeController(DentalCentreManagementContext context)
    {
        _context = context;
    }

    // Create a new employee
    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] NhanVien employee)
    {
        if (employee == null)
            return BadRequest(new { Message = "Invalid employee data." });

        try
        {
            _context.NhanViens.Add(employee);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.MaNv }, employee);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while creating the employee.", Error = ex.Message });
        }
    }

    // Read all employees
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        try
        {
            var employees = await _context.NhanViens.ToListAsync();
            return Ok(employees);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching employees.", Error = ex.Message });
        }
    }

    // Read a specific employee by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid employee ID." });

        try
        {
            var employee = await _context.NhanViens.FindAsync(id);
            if (employee == null)
                return NotFound(new { Message = "Employee not found." });

            return Ok(employee);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while fetching the employee.", Error = ex.Message });
        }
    }

    // Update an employee
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] NhanVien employee)
    {
        if (id <= 0 || employee == null || id != employee.MaNv)
            return BadRequest(new { Message = "Invalid data provided." });

        try
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound(new { Message = "Employee not found for update." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while updating the employee.", Error = ex.Message });
        }
    }

    // Delete an employee
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        if (id <= 0)
            return BadRequest(new { Message = "Invalid employee ID." });

        try
        {
            var employee = await _context.NhanViens.FindAsync(id);
            if (employee == null)
                return NotFound(new { Message = "Employee not found." });

            _context.NhanViens.Remove(employee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while deleting the employee.", Error = ex.Message });
        }
    }
}
