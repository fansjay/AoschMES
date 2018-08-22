using Aosch.MES.IDAL;
using System.Configuration;
using System.Reflection;

namespace Aosch.MES.DALFactory
{
    public  class AbstractFactory
    {
        //通过反射创建类的实例
        private static readonly string AssemblePath = ConfigurationManager.AppSettings["AssemblePath"];
        private static readonly string NameSpace= ConfigurationManager.AppSettings["NameSpace"];

        /// <summary>
        /// 通过反射创建类的实例
        /// </summary>
        /// <param name="ClassName">类名</param>
        /// <returns></returns>
        private static object CreateInstance(string ClassName)
        {
            var assembly = Assembly.Load(AssemblePath);
            return assembly.CreateInstance(ClassName);
        }
        public static IAccountDAL CreateAccountDAL()
        {
            string fullClassName = NameSpace + ".AccountDAL";
            return CreateInstance(fullClassName) as IAccountDAL;
        }
        public static IRoleDAL CreateRoleDAL()
        {
            string fullClassName = NameSpace + ".RoleDAL";
            return CreateInstance(fullClassName) as IRoleDAL;
        }
        public static ILogDAL CreateLogDAL()
        {
            string fullClassName = NameSpace + ".LogDAL";
            return CreateInstance(fullClassName) as ILogDAL;
        }
        public static IEmployeeDAL CreateEmployeeDAL()
        {
            string fullClassName = NameSpace + ".EmployeeDAL";
            return CreateInstance(fullClassName) as IEmployeeDAL;
        }


        //public static IAccountRole_MappingDAL CreateAccountRole_MappingDAL()
        //{
        //    string fullClassName = NameSpace + ".AccountRole_MappingDAL";
        //    return CreateInstance(fullClassName) as IAccountRole_MappingDAL;
        //}
        //public static IDepartmentDAL CreateDepartmentDAL()
        //{
        //    string fullClassName = NameSpace + ".DepartmentDAL";
        //    return CreateInstance(fullClassName) as IDepartmentDAL;
        //}
    }
}
