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

    public class ViewModelBase : INotifyPropertyChanged
    {
        private bool            _canExecute;
        private MediaElement    _media;
        private bool            isPaused = false;
        private Image           _playImage = new Image();

        public Image playImage
        {
            get { return _playImage; }
            set 
            { 
                _playImage.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/button_play.png");
                RaisePropertyChanged("toto");
            }
        }

        BitmapImage getNewImage(String path)
        {
            BitmapImage transition = new BitmapImage();

            transition.BeginInit();
            transition.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            transition.EndInit();
            return transition;
        }

        public ViewModelBase(MediaElement media)
        {
            _canExecute = true;
            _media = media;
            _playImage.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/button_play.png");
        }
        private ICommand _clickCommand;
        public ICommand ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => MyAction(), _canExecute));
            }
        }
        private ICommand _stopCommand;
        public ICommand stopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand = new CommandHandler(() => stopAction(), _canExecute));
            }
        }
        
        public void stopAction()
        {
            _media.Stop();
        }
        
        public void MyAction()
        {
            if (!isPaused)
            {
                _playImage.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/button_pause.png");
                //((MainWindow)System.Windows.Application.Current.MainWindow).Play.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/button_pause.png");
                if (MainWindow.getFileName() != null)
                {
                    _media.Source = new Uri(MainWindow.getFileName(), UriKind.RelativeOrAbsolute);
                    //isOk = true;
                }
                _media.Play();
                ((MainWindow)System.Windows.Application.Current.MainWindow).Text.Text = MainWindow.getFileName();
                isPaused = true;
            }
            else
            {
                _playImage.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/button_play.png");
                //((MainWindow)System.Windows.Application.Current.MainWindow).Play.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/button_play.png");
                _media.Pause();
                isPaused = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(String playImage)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(playImage));
            }
        }
    }

    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }

    public partial class MainWindow : Window
    {
        DispatcherTimer         timer;   
        public delegate void    timerTick();   
        timerTick               tick;
        private bool            isPlaying = false;
        private bool            isPaused;
        FolderBrowserDialog     folder = new System.Windows.Forms.FolderBrowserDialog();
        OpenFileDialog          dialogBox = new System.Windows.Forms.OpenFileDialog();
        private static string   fileName;
        private bool            isOk = false;
        private bool            isFullScreen = false;
  
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            tick = new timerTick(changeStatus);
            DataContext = new ViewModelBase(MediaElement);
        }

        public static String getFileName()
        {
            return fileName;
        }

        void timer_Tick(object sender, EventArgs e)   
        {
            Dispatcher.Invoke(tick);
        }

        BitmapImage getNewImage(String path)
        {
            BitmapImage transition = new BitmapImage();

            transition.BeginInit();
            transition.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            transition.EndInit();
            return transition;
        }

         void changeStatus()
        {   
            if (isPlaying)   
                TimeLine.Value = MediaElement.Position.TotalMilliseconds;
            if (MediaElement.Volume == 0)
                Volume.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/mute.png");
            else
                Volume.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/button_mute.png");
        }

        private void Element_MediaOpened(object sender, EventArgs e)
        {
            TimeLine.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void Element_MediaEnded(object sender, EventArgs e)
        {
            MediaElement.Stop();
            isPlaying = false;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            dialogBox.InitialDirectory = folder.SelectedPath;
            dialogBox.FileName = null;

            DialogResult result =  dialogBox.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
                fileName = dialogBox.FileName;
            isOk = false;
        }

        private void Grid_Drop(object sender, System.Windows.DragEventArgs e)
        {
            String[] file;

            file = (String[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop, true);
            MediaElement.Source = new Uri(file[0]);
            Text.Text = file[0];
            Play.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/button_pause.png");
            isPlaying = true;
            isPaused = false;
            MediaElement.Play();
        }

        private void fullScreen(object sender, RoutedEventArgs e)
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
        }

        private void playVideo(object sender, RoutedEventArgs e)
        {
            timer.Start();
            if (!isPaused)
            {
                Play.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/button_pause.png");
                if (fileName != null && !isOk)
                {
                    MediaElement.Source = new Uri(fileName, UriKind.RelativeOrAbsolute);
                    isOk = true;
                }
                MediaElement.Play();
                Text.Text = fileName;
                isPaused = true;
                isPlaying = true;
            }
            else
            {
                Play.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/button_play.png");
                MediaElement.Pause();
                isPlaying = false;
                isPaused = false;
            }
        }

        private void stopVideo(object sender, RoutedEventArgs e)
        {
            MediaElement.Stop();
            isPlaying = false;
        }

        private void muteVideo(object sender, RoutedEventArgs e)
        {
            if (MediaElement.IsMuted == true)
            {
                Volume.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/button_mute.png");
                MediaElement.IsMuted = false;
                VolumeLine.Value = 0.5;
            }
            else
            {
                Volume.Source = getNewImage("C:/Users/dasson_w/Desktop/Ressource PointNet/mute.png");
                MediaElement.IsMuted = true;
                VolumeLine.Value = 0;
            }
        }

        private void TimeLine_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            MediaElement.Pause();
            timer.Stop();
        }

        private void TimeLine_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            int SliderValue = (int)TimeLine.Value;
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            MediaElement.Position = ts;
            MediaElement.Play();
            timer.Start();
        }
    }
}
