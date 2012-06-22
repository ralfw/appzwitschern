using System.Windows;
using NUnit.Framework;

namespace az.gui.tests
{
    [TestFixture]
    public class MainWindowTests
    {
        private MainWindow mainWindow;

        [SetUp]
        public void Setup() {
            mainWindow = new MainWindow();
        }

        [Test, Explicit, RequiresSTA]
        public void Event_wird_ausgelöst() {
            mainWindow.Versenden += versandauftrag => MessageBox.Show(
                string.Format("'{0}' - um {1}", versandauftrag.Text, versandauftrag.Termin));
            mainWindow.ShowDialog();
        }

        [Test, Explicit, RequiresSTA]
        public void Status_wird_gesetzt() {
            mainWindow.Versandstatus("Success!");
            mainWindow.ShowDialog();
        }

        [Test, Explicit, RequiresSTA]
        public void ShortenText_feuert() {
            mainWindow.ShortenText += s => MessageBox.Show(s);
            mainWindow.ShowDialog();
        }

        [Test, Explicit, RequiresSTA]
        public void Anzahl_verbleibender_Zeichen() {
            mainWindow.ShortenText += mainWindow.ShortenedText;
            mainWindow.ShowDialog();
        }
    }
}