using Aosch.MES.Model;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;

namespace Aosch.MES.DAL
{
    //需要引用EF
    public  class DBContextFactory
    {
        public static DbContext CreateDBContext()
        {
            DbContext dbcontext = (DbContext)CallContext.GetData("AoschMesDBContext");
            if (dbcontext == null)
            {
                dbcontext = new AoschMESEntities();
                CallContext.SetData("AoschMesDBContext", dbcontext);
            }
            return dbcontext;
        }
    }
}
