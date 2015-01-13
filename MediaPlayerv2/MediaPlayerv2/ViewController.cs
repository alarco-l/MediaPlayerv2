using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;

namespace MediaPlayerv2
{
    public class ViewController : INotifyPropertyChanged
    {
        private MediaElement _media;
        private ViewModelBase _viewModel;
        private Double _valueTime;
        private Double _maximumDuration;

        public ViewController(MediaElement media, ViewModelBase viewModel)
        {
            _media = media;
            _viewModel = viewModel;
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
        }

        void    _viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "playWithFile")
            {
                _media.Source = new Uri(_viewModel.fileName, UriKind.RelativeOrAbsolute);
                _media.Play();
            }
            if (e.PropertyName == "totalTime")
            {
                if (_media.NaturalDuration.HasTimeSpan)
                    maximumDuration = _media.NaturalDuration.TimeSpan.TotalSeconds;
            }
            if (e.PropertyName == "play")
                _media.Play();
            if (e.PropertyName == "update")
            {
                valueTime = _media.Position.TotalSeconds;
                if (valueTime == maximumDuration)
                    _media.Stop();
            }
            if (e.PropertyName == "pause")
                _media.Pause();
            if (e.PropertyName == "stop")
                _media.Stop();
            if (e.PropertyName == "mute")
                _media.IsMuted = true;
            if (e.PropertyName == "unMute")
                _media.IsMuted = false;
        }

        public Double maximumDuration
        {
            get { return _maximumDuration; }
            set
            {
                _maximumDuration = value;
                RaisePropertyChanged("maximumDuration");
            }
        }

        public Double valueTime
        {
            get { return _valueTime; }
            set
            {
                _valueTime = value;
                RaisePropertyChanged("valueTime");
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
