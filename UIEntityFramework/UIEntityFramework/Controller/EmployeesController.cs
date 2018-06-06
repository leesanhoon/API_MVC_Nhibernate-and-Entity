using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UIEntityFramework.Models;

namespace UIEntityFramework.Controllers
{
    public class EmployeesController : Controller
    {
        string url = "http://localhost:51448/api/employees";
        HttpClient client;

        public EmployeesController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private SinhVienDBContext db = new SinhVienDBContext();

        // GET: Employees
        public async Task<ActionResult> Index()
        {
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                List<Employee> Employees = JsonConvert.DeserializeObject<List<Employee>>(responseData, settings);
                return View(Employees);

            }
            return View("Error");
        }

        // GET: Employees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            Employee employee = null;
            HttpResponseMessage response = await client.GetAsync(url + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                employee = await response.Content.ReadAsAsync<Employee>();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Uri> Create([Bind(Include = "id,FirstName,LastName,Designation")] Employee employee)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(url, employee);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;

        }

        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            Employee employee = null;
            HttpResponseMessage response = await client.GetAsync(url + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                employee = await response.Content.ReadAsAsync<Employee>();
            }
            return View(employee);
           
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<HttpStatusCode> Edit([Bind(Include = "id,FirstName,LastName,Designation")] Employee employee)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(url + "/" + employee.id ,employee);
            response.EnsureSuccessStatusCode();
            employee = await response.Content.ReadAsAsync<Employee>();
            return response.StatusCode;
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            Employee employee = null;
            HttpResponseMessage response = await client.GetAsync(url + "/" + id);
            if (response.IsSuccessStatusCode)
            {
                employee = await response.Content.ReadAsAsync<Employee>();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<HttpStatusCode> DeleteConfirmed(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync(url + "/" + id);
            return response.StatusCode;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
