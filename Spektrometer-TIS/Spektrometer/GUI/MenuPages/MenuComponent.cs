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
        protected SpektrometerService SpektrometerService { get; private set; }
        protected MainWindow MainWindow { get; private set; }

        public MenuComponent(MainWindow mainWindow)
        {
            SpektrometerService = mainWindow.spektrometerService;
            MainWindow = mainWindow;
            SetReferencesFromSpektrometerService();
        }
        protected abstract void SetReferencesFromSpektrometerService();
    }
}
