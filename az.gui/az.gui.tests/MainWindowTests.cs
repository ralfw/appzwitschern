using System.Windows;
using NUnit.Framework;

namespace az.gui.tests
{
    [TestFixture]
    public class MainWindowTests
    {
        [Test, Explicit, RequiresSTA]
        public void Event_wird_ausgelöst() {
            var mainWindow = new MainWindow();
            mainWindow.Versenden += s => MessageBox.Show(s);

            mainWindow.ShowDialog();
        }

        [Test, Explicit, RequiresSTA]
        public void Status_wird_gesetzt() {
            var mainWindow = new MainWindow();
            mainWindow.Versandstatus("Success!");

            mainWindow.ShowDialog();
        }
    }
}