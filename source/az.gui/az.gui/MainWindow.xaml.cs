using System;
using System.Windows;
using az.contracts;

namespace az.gui
{
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();

            btnSenden.Click += (o, e) => {
                lblStatus.Text = "";
                Versenden(new Versandauftrag {
                    Text = txtTweetText.Text,
                    Termin = DateTime.Parse(txtTermin.Text)
                });
            };
        }

        public event Action<Versandauftrag> Versenden;

        public void Versandstatus(string message) {
            lblStatus.Text = message;
        }
    }
}