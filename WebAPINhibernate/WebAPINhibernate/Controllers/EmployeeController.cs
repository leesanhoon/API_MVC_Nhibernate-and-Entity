using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPINhibernate.Models;
using NHibernate;
using NHibernate.Linq;

namespace WebAPINhibernate.Controllers
{
    public class EmployeeController : ApiController
    {
      
        //NHibernate Session  
        ISession session = NHibertnateSession.OpenSession();
        //Get All Employee  
        public List<Employee> GetListEmployee()
        {
            List<Employee> employee = session.Query<Employee>().ToList();
            return employee;
        }
        //Add New Employee  
        [HttpPost]
        public HttpResponseMessage AddNewEmployee(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(employee);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        //GetEmployeeData  
        [HttpGet]
        public Employee DetailsEmployee(int id)
        {
            var employee = session.Get<Employee>(id);
            return employee;
        }
        //UpdateEmployee  
        [HttpPut]
        public HttpResponseMessage UpdateEmployee(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var emp = session.Get<Employee>(employee.Id);
                    emp.FirstName = employee.FirstName;
                    emp.LastName = employee.LastName;
                    
                    emp.Designation = employee.Designation;
                   
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(emp);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error !");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        //Delete Employee  
        [HttpDelete]
        public HttpResponseMessage DeleteEmployee(int id)
        {
            try
            {
                var employee = session.Get<Employee>(id);
                if (employee != null)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(employee);
                        transaction.Commit();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "Success");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error !");
                }
            }
            catch (Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
