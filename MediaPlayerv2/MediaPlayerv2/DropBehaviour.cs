using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MediaPlayerv2
{
    class DropBehaviour : Behavior<ListBox>
    {
        public ICommand DropCommand
        {
            get { return (ICommand)GetValue(DropCommandProperty); }
            set { SetValue(DropCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropCommandProperty =
            DependencyProperty.Register("DropCommand", typeof(ICommand), typeof(DropBehaviour), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            this.AssociatedObject.Drop += AssociatedObject_Drop;
            base.OnAttached();
        }

        void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            if (DropCommand == null) { return; }
            var files = e.Data.GetData(System.Windows.DataFormats.FileDrop, true) as String[];

            if (DropCommand.CanExecute(files))
            {
                DropCommand.Execute(files);
            }
        }
    }
}
