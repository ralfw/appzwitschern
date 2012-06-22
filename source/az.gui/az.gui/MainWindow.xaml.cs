using System;
using System.Windows;
using az.contracts;

namespace az.gui
{
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();

            var now = DateTime.Now;
            txtTermin.Text = now.ToShortDateString() + " " + now.ToShortTimeString();

            btnSenden.Click += (o, e) => {
                lblStatus.Text = "";
                DateTime termin;
                if (!DateTime.TryParse(txtTermin.Text, out termin)) {
                    txtError.Text = "Fehlerhaftes Datumsformat";
                    return;
                }

                txtError.Text = "";
                Versenden(new Versandauftrag {
                    Text = txtTweetText.Text,
                    Termin = termin
                });
            };
        }

        public event Action<Versandauftrag> Versenden;

        public void Versandstatus(string message) {
            lblStatus.Text = message;
        }
    }
}