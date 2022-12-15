using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestEmployees.Models
{
    public class EmployeeCLS
    {
        public int ID { get; set;  }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string RFC { get; set; }
        public DateTime BornDate { get; set; }
        public EmployeeStatus Status { get; set; }
        public string StatusDes { get; set; }
    }
    public enum EmployeeStatus
    {
        NotSet,
        Active,
        Inactive,
    }

}