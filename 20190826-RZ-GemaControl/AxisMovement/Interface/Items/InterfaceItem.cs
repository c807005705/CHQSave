using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Items
{
   public class InterfaceItem
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        public string InterfaceName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public List<ParamterItem> Paramters { get; set; } = new List<ParamterItem>();

    }

    public class ParamterItem
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}

