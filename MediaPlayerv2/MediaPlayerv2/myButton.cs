using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MediaPlayerv2
{
    public class myButton : Button
    {
        private bool _isFullscreen = false;

        protected override void OnClick()
        {

            base.OnClick();
            var window = Window.GetWindow(this);

            if (!_isFullscreen)
            {
                window.WindowStyle = WindowStyle.None;
                window.WindowState = WindowState.Maximized;
                _isFullscreen = true;
            }
            else
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.WindowState = WindowState.Normal;
                _isFullscreen = false;
            }
        }
    }
}
