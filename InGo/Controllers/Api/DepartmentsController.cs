using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InGo.Models;
using Microsoft.AspNetCore.Authorization;
using InGo.Models.Configuration;

namespace InGo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IngoContext _context;

        private IQueryable<Department> FullDepartments
        {
            get => _context.Departments
              .Include(d => d.Users);
        }

        public DepartmentsController(IngoContext context)
        {
            _context = context;
        }
        //get all departments
        // GET: api/Departments
        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await FullDepartments.ToArrayAsync();
            return Ok(departments.Select(d => d.ViewModel));
        }

        //get deparment by id
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var department = await FullDepartments.ToArrayAsync();
            var result = department.Where(d => d.Id == id).Select(d => d.ViewModel).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }


            return Ok(result);
        }



        //get all users from department
        // GET: api/Departments/3/users
        [HttpGet("{id}/users")]
        [Authorize(Roles = Roles.ModeratorOrAdmin)]
        public async Task<IActionResult> GetUsersFromDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await FullDepartments.ToArrayAsync();
            var result = department.Where(d => d.Id == id).Select(d => d.UserViewModel).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        // add department
        // POST: api/Departments
        [HttpPost]
        [Authorize(Roles = Roles.ModeratorOrAdmin)]
        public async Task<IActionResult> AddDepartment([FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await DepartmentExists(department.Id))
                return BadRequest($"Department {department.Id} already exists!");

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        //alter department
        [HttpPost("{id}/edit")]
        [Authorize(Roles = Roles.ModeratorOrAdmin)]
        public async Task<IActionResult> EditDepartment([FromRoute] int id, [FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await DepartmentExists(id))
                return NotFound($"Department {id} does not exist.");

            department.Id = id;
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        //delete department
        // DELETE: api/Deparments/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.ModeratorOrAdmin)]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            department.IsDeleted = true;
            await _context.SaveChangesAsync();

            return Ok(department);
        }

        private Task<bool> DepartmentExists(int id)
        {
            return _context.Departments.AnyAsync(e => e.Id == id);
        }
    }
}