using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MediaPlayerv2
{
    public class ViewController : INotifyPropertyChanged
    {
        private MediaElement _media;
        private ViewModelBase _viewModel;
        private Double _valueTime;
        private Double _maximumDuration;
        private PlayList _playList = new PlayList();
        private List<String> _list = new List<String>();
        private String[] _listString;
        private uint _playListIndex;

        public ViewController(MediaElement media, ViewModelBase viewModel)
        {
            _media = media;
            _viewModel = viewModel;
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
            _playListIndex = 0;
        }

        void    managePlay()
        {
            _list = _playList.GetListItem();
            if (_list.Count() > 0)
            {
                _listString = _list.ToArray();
                _media.Source = new Uri(_listString[_playListIndex], UriKind.RelativeOrAbsolute);
            }
            _media.Play();
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
            {
                managePlay();
            }
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
            if (e.PropertyName == "prevPush")
            {
                if (_list.Count() > 0)
                {
                    if (_playListIndex > 0)
                        _playListIndex = _playListIndex - 1;
                    else
                        _playListIndex = 0;
                    managePlay();
                }
            }
            if (e.PropertyName == "nextPush")
            {
                if (_list.Count() > 0)
                {
                    if (_playListIndex < _listString.Count() - 1)
                        _playListIndex = _playListIndex + 1;
                    else
                        _playListIndex = (uint)_listString.Count() - 1;
                    managePlay();
                }
            }
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
