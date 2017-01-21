using DotNetWrapperGen.View;
using System;
using System.Windows.Forms;

namespace DotNetWrapperGen
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ProjectView());
        }
    }
}
