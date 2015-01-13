using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MediaPlayerv2
{
    public class PlayList
    {
        private ObservableCollection<string> _fileList;
        private int _index;

        public  PlayList()
        {
            _index = 0;
            _fileList = new ObservableCollection<string>();
        }

        public void     add(string File)
        {
            _fileList.Add(File);
        }

       public string      next()
        {
            if (_fileList.Count > 0)
            {
                if (_index < _fileList.Count - 1)
                    ++_index;
            }
            return (_fileList[_index]);
        }

        public string     prev()
       {
            if (_fileList.Count > 0)
            {
                if (_index > 0)
                    --_index;
            }
            return (_fileList[_index]);
       }

        public void     save()
        {
            XDocument xdoc = new XDocument();
            xdoc.Add(new XElement("playlist"));
            for (int i = 0; i < _fileList.Count; ++i)
                xdoc.Root.Add(new XElement("Path", _fileList[i]));
            xdoc.Save("C:/Users/dasson_w/playlist.txt");
        }
    }
}
