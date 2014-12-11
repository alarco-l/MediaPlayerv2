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
        private bool isPlaying;
        private bool isPaused;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModelBase(ref MediaElement);
        }

        private void Grid_Drop(object sender, System.Windows.DragEventArgs e)
        {
            String[] file;

            file = (String[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop, true);
            MediaElement.Source = new Uri(file[0]);
      //      Text.Text = file[0];
        //    Play.Source = getNewImage("C:/Users/ludovic/Desktop/MediaPlayerv2/Ressource PointNet/button_pause.png");
            isPlaying = true;
            isPaused = false;
            MediaElement.Play();
        }

     /*   private void fullScreen(object sender, RoutedEventArgs e)
        {
            if (!isFullScreen)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                isFullScreen = true;
            }
            else
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                isFullScreen = false;
            }
        }*/

        private void TimeLine_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            MediaElement.Pause();
            //timer.Stop();
        }

        private void TimeLine_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            int SliderValue = (int)TimeLine.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            MediaElement.Position = ts;
            MediaElement.Play();
            //timer.Start();
        }

        private void TimeLine_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MediaElement.Source != null)
            {
                TimeLine.Value = e.GetPosition(TimeLine).X / TimeLine.ActualWidth * TimeLine.Maximum;
				int time = Convert.ToInt32(TimeLine.Value);
                MediaElement.Position = TimeSpan.FromSeconds(time);
				Console.Write(time);
                Console.Write("\n");
            }
        }

    }
}
