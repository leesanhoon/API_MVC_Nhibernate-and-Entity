using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UIEntityFramework.Models
{
    public class Employee
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
    }
    public class SinhVienDBContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }
    }
}