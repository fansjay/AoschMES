using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aosch.MES.Common
{
    public enum LogType
    {
        [Description("普通日志")]
        Info=0x000,
        [Description("警告日志")]
        Warning =0x001,
        [Description("错误日志")]
        Error =0x002,
        [Description("致使错误日志")]
        Fatal =0x003
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
