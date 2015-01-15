using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MediaPlayerv2
{
    public struct item
    {
        public string path {get; set;}
    }

    public class PlayList
    {
        private ObservableCollection<string> _fileList;
        private ObservableCollection<string> _saveList;
        private int _index;

        public  PlayList()
        {
            _index = 0;
            _fileList = new ObservableCollection<string>();
            _saveList = new ObservableCollection<string>();
        }

        public void     add(string File)
        {
            _fileList.Add(File);
            ++_index;
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

        public string   first()
        {
            if (_fileList.Count > 0)
            {
                _index = 0;
                return (_fileList[_index]);
            }
            return null;
        }

        public void save()
        {
            XDocument xdoc = new XDocument();
            xdoc.Add(new XElement("playlist"));
            for (int i = 0; i < _fileList.Count; ++i)
                xdoc.Root.Add(new XElement("Path", _fileList[i]));
            xdoc.Save(@"playlist.txt");
        }
        
        public ObservableCollection<string> load()
        {
            XDocument doc;
            try
            {
                 doc = XDocument.Load(@"playlist.txt");
                 var test = (from c in doc.Root.Descendants("Path")
                             select new item()
                             {
                                 path = c.Value
                             });
                 foreach (var VariablePath in test)
                 {
                     _fileList.Add(VariablePath.path);
                     _saveList.Add(Path.GetFileNameWithoutExtension(VariablePath.path));
                 }
                 _index = _fileList.Count - 1;
            }

            catch (FileNotFoundException)
            {
            }
            
            return (_saveList);
        }
    }
}
