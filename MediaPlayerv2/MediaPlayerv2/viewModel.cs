using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MediaPlayerv2
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        DispatcherTimer timer;
        public delegate void timerTick();
        timerTick tick;
        FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
        OpenFileDialog dialogBox = new System.Windows.Forms.OpenFileDialog();
        private bool            _canExecute;
        private string          _realName;
        private ImageSource     _playImage;
        private ImageSource     _soundImage;
        private CommandHandler  _stopCommand;
        private CommandHandler  _playCommand;
        private CommandHandler  _openFile;
        private CommandHandler  _soundClick;
        private CommandHandler  _prevButton;
        private CommandHandler  _nextButton;
        private string          _fileName;
        private bool            _isPlaying = false;
        private bool            _isOk = false;
        private bool            _isPaused = false;
        private bool            _isMuted = false;

        BitmapImage getNewImage(String path)
        {
            BitmapImage transition = new BitmapImage();

            transition.BeginInit();
            transition.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            transition.EndInit();
            return transition;
        }

        public ViewModelBase()
        {
            _canExecute = true;
            playImage = getNewImage("C:/Users/dasson_w/Desktop/MediaPlayerv2/Ressource PointNet/play.png");
            soundImage = getNewImage("C:/Users/dasson_w/Desktop/MediaPlayerv2/Ressource PointNet/sound.png");
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
            tick = new timerTick(changeStatus);
        }

        void changeStatus()
        {
            if (_isPlaying)
                RaisePropertyChanged("update");
            RaisePropertyChanged("totalTime");
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Dispatcher.Invoke(tick);
        }

        #region propertyChanged

        public ImageSource playImage
        {
            get { return _playImage; }
            set
            {
                _playImage = value;
                RaisePropertyChanged("playImage");
            }
        }

        public  ImageSource soundImage
        {
            get { return _soundImage; }
            set
            {
                _soundImage = value;
                RaisePropertyChanged("soundImage");
            }
        }

        public String fileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged("fileName");
            }
        }

        public string realName
        {
            get { return _realName; }
            set
            {
                _realName = value;
               RaisePropertyChanged("realName");
            }
        }

        #endregion

        #region command
        public CommandHandler playCommand
        {
            get
            {
                return _playCommand ?? (_playCommand = new CommandHandler(() => playAction(), _canExecute));
            }
        }

        public CommandHandler stopCommand
        {
            get
            {
                return _stopCommand ?? (_stopCommand = new CommandHandler(() => stopAction(), _canExecute));
            }
        }

        public CommandHandler openFile
        {
            get
            {
                return _openFile ?? (_openFile = new CommandHandler(() => openFileAction(), _canExecute));
            }
        }

        public CommandHandler soundClick
        {
            get
            {
                return _soundClick ?? (_soundClick = new CommandHandler(() => soundClickAction(), _canExecute));
            }
        }

        public CommandHandler PrevButton
        {
            get
            {
                return _prevButton ?? (_prevButton = new CommandHandler(() => PrevButtonAction(), _canExecute));
            }
        }

        public CommandHandler NextButton
        {
            get
            {
                return _nextButton ?? (_nextButton = new CommandHandler(() => NextButtonAction(), _canExecute));
            }
        }

        #endregion

        public void PrevButtonAction()
        {
            RaisePropertyChanged("prevPush");
        }

        public void NextButtonAction()
        {
            RaisePropertyChanged("nextPush");
        }

        public void stopAction()
        {
            BitmapImage bplay = getNewImage("C:/Users/dasson_w/Desktop/MediaPlayerv2/Ressource PointNet/play.png");
            ImageBrush brush = new ImageBrush();

            brush.ImageSource = bplay;
            playImage = brush.ImageSource;
            RaisePropertyChanged("stop");
            _isPlaying = false;
            _isPaused = false;
            timer.Stop();
        }

        public void soundClickAction()
        {
            BitmapImage bsound = getNewImage("C:/Users/dasson_w/Desktop/MediaPlayerv2/Ressource PointNet/mute.png");
            BitmapImage bmute = getNewImage("C:/Users/dasson_w/Desktop/MediaPlayerv2/Ressource PointNet/sound.png");
            ImageBrush brush = new ImageBrush();

            if (!_isMuted)
            {
                brush.ImageSource = bsound;
                soundImage = brush.ImageSource;
                _isMuted = true;
                RaisePropertyChanged("mute");
            }
            else
            {
                brush.ImageSource = bmute;
                soundImage = brush.ImageSource;
                _isMuted = false;
                RaisePropertyChanged("unMute");
            }
        }

        public void openFileAction()
        {
            dialogBox.InitialDirectory = folder.SelectedPath;
            dialogBox.FileName = null;
            dialogBox.DefaultExt = ".avi";
            dialogBox.Filter = "Videos (*.avi) |*.avi|Images (*.jpg, *.png)|*.jpg;*.png|Videos (*.mp4)|*.mp4";

            DialogResult result = dialogBox.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
                _fileName = dialogBox.FileName;
            if (_fileName != null)
                realName = Path.GetFileNameWithoutExtension(_fileName);
            _isOk = false;
        }

        public void playAction()
        {
            BitmapImage bplay = getNewImage("C:/Users/dasson_w/Desktop/MediaPlayerv2/Ressource PointNet/play.png");
            BitmapImage bpause = getNewImage("C:/Users/dasson_w/Desktop/MediaPlayerv2/Ressource PointNet/pause.png");
            ImageBrush brush = new ImageBrush();

            if (!_isPaused)
            {
                brush.ImageSource = bpause;
                playImage = brush.ImageSource;
                if (_fileName != null && !_isOk)
                {
                    RaisePropertyChanged("playWithFile");
                    _isOk = true;
                }
                RaisePropertyChanged("play");
                timer.Start();
                _isPaused = true;
                _isPlaying = true;
            }
            else
            {
                brush.ImageSource = bplay;
                playImage = brush.ImageSource;
                RaisePropertyChanged("pause");
                timer.Stop();
                _isPaused = false;
                _isPlaying = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(String propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
