using InsertUpdateDeleteDataJQueryAjax.DAO;
using InsertUpdateDeleteDataJQueryAjax.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace InsertUpdateDeleteDataJQueryAjax.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult Index(string id)
        {
            ViewBag.KeyID = id;
            return View();
        }
        public JsonResult GetJsonData(int page, int pageSize = 3)
        {
            var listData = new EmDAO().GetTbl_Employees();
            var listEmJson = new List<tbl_Employee>();
            foreach (var item in listData)
            {
                listEmJson.Add(new tbl_Employee()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Address = item.Address,
                    Age = item.Age,
                    Status = item.Status
                });
            }

            var model = listEmJson.Skip((page - 1) * pageSize).Take(pageSize);
            int totalRow = listEmJson.Count;
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true
            },JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(string model)
        {
            //tao 1 bien Serialize de chuyen doi tu Json sang object
            JavaScriptSerializer javaScript = new JavaScriptSerializer();
            //Chuyen sang kieu Employee
            tbl_Employee employee = javaScript.Deserialize<tbl_Employee>(model);
            EmployeeDbContext em = new EmployeeDbContext();
            var obj = em.tbl_Employee.Single(x => x.ID == employee.ID);
            obj.Name = employee.Name;
            em.SaveChanges();
            return Json(new
            {
                status = true
            });
        }
        [HttpPost]
        public JsonResult SaveData(string strEmployee)
        {
            EmployeeDbContext em = new EmployeeDbContext();
            //tao 1 bien Serialize de chuyen doi tu Json sang object
            JavaScriptSerializer javaScript = new JavaScriptSerializer();
            bool status = false;
            string message = string.Empty;
            //Chuyen sang kieu Employee
            tbl_Employee employee = javaScript.Deserialize<tbl_Employee>(strEmployee);
            // add new Employee if ID = 0 
            if (employee.ID == 0)
            {
                em.tbl_Employee.Add(employee);
                em.SaveChanges();
                status = true;
            }
            else
            {
                var obj = em.tbl_Employee.Single(x => x.ID == employee.ID);
                obj.Name = employee.Name;
                obj.Address = employee.Address;
                obj.Status = employee.Status;
                try
                {
                    em.SaveChanges();
                    status = true;
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                    throw;
                }
            }
            return Json(new
            {
                status = status,
                Message = message
            });
        }


    }
}