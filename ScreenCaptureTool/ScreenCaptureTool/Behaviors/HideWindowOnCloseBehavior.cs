using System.Windows;
using System.Windows.Interactivity;

namespace ScreenCaptureTool.Behaviors
{
    public class HideWindowOnCloseBehavior : Behavior<Window>
    {
        public static DependencyProperty HideOnCloseProperty =
            DependencyProperty.Register("HideOnClose", typeof(bool), typeof(HideWindowOnCloseBehavior),
            new FrameworkPropertyMetadata(false));

        public bool HideOnClose
        {
            get
            {
                return (bool)GetValue(HideOnCloseProperty);
            }
            set
            {
                SetValue(HideOnCloseProperty, value);
            }
        }

        protected override void OnAttached()
        {
            AssociatedObject.Closing += (sender, args) =>
            {
                args.Cancel = HideOnClose;

                if (HideOnClose)
                {
                    var win = sender as Window;
                    if (win != null)
                    {
                        win.Hide();
                    }
                }
            };
        }
    }
}
