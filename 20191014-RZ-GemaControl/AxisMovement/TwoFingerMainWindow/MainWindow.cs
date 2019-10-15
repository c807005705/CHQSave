using Interface.Interface;
using RZ_AutoGemaControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoFingerMainWindow
{
    public partial class MainWindow : IWindow
    {
        public void Dispose()
        {
            
        }

        public void ShowWindow()
        {

            GameConsole gameConsole = new GameConsole();
            gameConsole.ShowDialog();
        }
    }
}
