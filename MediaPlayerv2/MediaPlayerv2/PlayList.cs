using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MediaPlayerv2
{
    public class PlayList : ListBox
    {
        private static List<String> _list = new List<String>();

        protected override void OnDrop(System.Windows.DragEventArgs e)
        {
            base.OnDrop(e);
            String[] file;
            String add;
            String extension;

            file = (String[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop, true);
            add = file[0];
            _list.Add(add);
            extension = Path.GetExtension(add);
            if (extension == ".jpg" || extension == ".avi" || extension == ".mp4" || extension == "png")
            {
                add = Path.GetFileNameWithoutExtension(add);
                Items.Add(add);
            }
            else
                MessageBox.Show("Format de fichier non supporter");
        }

        public List<String>    GetListItem()
        {
            return _list;
        }
    }
}
