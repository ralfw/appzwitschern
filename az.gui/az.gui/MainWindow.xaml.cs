using System;
using System.Windows;

namespace az.gui
{
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();

            btnSenden.Click += (o, e) => Versenden(txtTweetText.Text);
        }

        public event Action<string> Versenden;

        public void Versandstatus(string message) {
            lblStatus.Text = message;
        }
    }
}