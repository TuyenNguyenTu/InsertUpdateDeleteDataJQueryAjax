using InsertUpdateDeleteDataJQueryAjax.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsertUpdateDeleteDataJQueryAjax.DAO
{
    public class EmDAO
    {
        EmployeeDbContext db = new EmployeeDbContext();
        public List<tbl_Employee> GetTbl_Employees()
        {
            return db.tbl_Employee.ToList();
        }
    }
}