using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using npantarhei.runtime;

namespace az.application
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var config = new FlowRuntimeConfiguration();

            using(var fr = new FlowRuntime(config))
            {
                
            }
        }
    }
}
