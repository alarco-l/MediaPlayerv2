using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.ComponentModel;

namespace MediaPlayerv2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private ViewController _controller;

        public MainWindow()
        {
            InitializeComponent();
            _controller = new ViewController(MediaElement, (ViewModelBase)DataContext); //DataContext correspond to viewModel
        }

        public ViewController Controller
        {
            get { return _controller; }
            set
            {
                _controller = value;
            }
        }

        private void TimeLine_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MediaElement.Source != null)
            {
                TimeLine.Value = e.GetPosition(TimeLine).X / TimeLine.ActualWidth * TimeLine.Maximum;
				int time = Convert.ToInt32(TimeLine.Value);
                MediaElement.Position = TimeSpan.FromSeconds(time);
            }
        }

    }
}
