//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Aosch.MES.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employee
    {
        public int ID { get; set; }
        public string EmployeeName { get; set; }
        public string TelphoneNumber { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public int DepartmentID { get; set; }
        public int GroupID { get; set; }
        public int Sex { get; set; }
        public string Address { get; set; }
        public System.DateTime Birthday { get; set; }
        public System.DateTime HireDate { get; set; }
        public Nullable<System.DateTime> FireDate { get; set; }
        public int PositionID { get; set; }
        public string IDCardNumber { get; set; }
        public string RecordAccount { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
    }
}
