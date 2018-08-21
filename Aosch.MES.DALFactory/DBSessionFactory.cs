using Aosch.MES.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Aosch.MES.DALFactory
{
     public  class DBSessionFactory
    {
        public static IDBSession CreateDBSession()
        {
            IDBSession dbsession = CallContext.GetData("AoschMesDBSession") as IDBSession;
            if (dbsession == null)
            {
                dbsession = new DBSession();
                CallContext.SetData("AoschMesDBSession", dbsession);
            }
            return dbsession;
        }
    }
}
