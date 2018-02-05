using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using Spektrometer.Logic;

namespace Spektrometer.GUI
{
    public abstract class MenuComponent : Page
    {
        protected MainWindow MainWindow { get; private set; }

        public MenuComponent(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            SetReferences();
        }
        protected abstract void SetReferences();
    }
}
