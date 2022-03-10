using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Process;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.WebApp.Data;
using System.Diagnostics;
using Sprout.Exam.WebApp.Validation;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;
using System.Data.SqlClient;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
           
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await Task.FromResult(_context.ResultList);
       
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await Task.FromResult(_context.ResultList.FirstOrDefault(m => m.Id == id));
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            var item = await Task.FromResult(_context.ResultList.FirstOrDefault(m => m.Id == input.Id));
            if (item == null) return NotFound();
            item.FullName = input.FullName;
            item.Tin = input.Tin;
            item.Birthdate = input.Birthdate.ToString("yyyy-MM-dd");
            item.TypeId = input.TypeId;
            item.Salary = input.Salary;
            _context.Update(item);
            _context.SaveChanges();
            return Ok(item);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
  
        [HttpPost]

        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {

           var id = await Task.FromResult(_context.ResultList.Max(m => m.Id) + 1);

            EmployeeDto model = new EmployeeDto();
            model.Id = id;
            model.Birthdate = input.Birthdate.ToString("yyyy-MM-dd");
            model.FullName = input.FullName;
            model.Tin = input.Tin;
            model.TypeId = input.TypeId;
            model.Salary = input.Salary;

            //_context.ResultList.Add(new EmployeeDto
            //{
            //    Birthdate = input.Birthdate.ToString("yyyy-MM-dd"),
            //    FullName = input.FullName,
            //    Tin = input.Tin,
            //    TypeId = input.TypeId,
            //    Salary = input.Salary
                
            //});

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ResultList.Add(model);
                    _context.SaveChanges();
                    return Created($"/api/employees/{id}", id);
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine(string.Format(@"Entity of type ""{0}"" in state ""{1}"" 
                   has the following validation errors:",
                            eve.Entry.Entity.GetType().Name,
                            eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine(string.Format(@"- Property: ""{0}"", Error: ""{1}""",
                                ve.PropertyName, ve.ErrorMessage));
                        }
                    }
                    return BadRequest();
                }
                catch (DbUpdateException e)
                {
                    UpdateException updateException = (UpdateException)e.InnerException;
                    SqlException sqlException = (SqlException)updateException.InnerException;

                    foreach (SqlError error in sqlException.Errors)
                    {

                        Console.WriteLine(string.Format(@"- Property: ""{0}"", Error: ""{1}""",
                            error.Source, error.Message));
                    }
                    return BadRequest();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return BadRequest();
                }
            }
            return BadRequest();
           
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Task.FromResult(_context.ResultList.FirstOrDefault(m => m.Id == id));
            if (result == null) return NotFound();

            try
            {
                _context.ResultList.Remove(result);
                _context.SaveChanges();
                return Ok(id);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine(string.Format(@"Entity of type ""{0}"" in state ""{1}"" 
                   has the following validation errors:",
                        eve.Entry.Entity.GetType().Name,
                        eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine(string.Format(@"- Property: ""{0}"", Error: ""{1}""",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }
                return BadRequest();
            }
            catch (DbUpdateException e)
            {
                UpdateException updateException = (UpdateException)e.InnerException;
                SqlException sqlException = (SqlException)updateException.InnerException;

                foreach (SqlError error in sqlException.Errors)
                {

                    Console.WriteLine(string.Format(@"- Property: ""{0}"", Error: ""{1}""",
                        error.Source, error.Message));
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
           
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(ComputeSalaryDto input)
        {
            var result = await Task.FromResult(_context.ResultList.FirstOrDefault(m => m.Id == input.Id));

            if (result == null) return NotFound();
            var type = (EmployeeType) result.TypeId;
      
            return type switch
            {
                EmployeeType.Regular =>
                    //create computation for regular.
                    Ok(new RegularEmployee(result.Salary,input.AbsentDays,input.WorkedDays,(decimal)0.12).ComputedSalary),
                EmployeeType.Contractual =>
                    //create computation for contractual.
                    Ok(new ContractualEmploye(result.Salary,input.WorkedDays).ComputedSalary),
                _ => NotFound("Employee Type not found")
            };

        }

    }
}
