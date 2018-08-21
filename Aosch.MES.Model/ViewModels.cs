using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aosch.MES.Model
{
    public class ActionURLViewModel
    {
        public int ID { get; set; }
        public string MenuName { get; set; }
        public string PageLogo { set; get; }
        public int ParentID { set; get; }
        public string URL { set; get; }
        public string Icon { get; set; }
        public bool IsRoot { get; set; }
        public string Description { set; get; }

        public List<ActionURLViewModel> Childrens { set; get; }

    }
}
