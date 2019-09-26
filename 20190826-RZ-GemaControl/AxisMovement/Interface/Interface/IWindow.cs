using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Interface
{
    /// <summary>
    /// 窗口接口
    /// </summary>
    public interface IWindow:IDisposable
    {
        /// <summary>
        /// 显示窗口
        /// </summary>
        void ShowWindow();
    }
}
