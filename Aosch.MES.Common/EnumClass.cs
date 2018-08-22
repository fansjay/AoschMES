using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aosch.MES.Common
{
    public enum LogType
    {
        Info=0x000,
        Warning=0x001,
        Error=0x002,
        Fatal=0x003
    }


    public enum RoleLevel
    {
        超级管理员=10,
        系统管理员=100,
        公司管理=200,
        普通员工=300,
        其它=400
    }


}
