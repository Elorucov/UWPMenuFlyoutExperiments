using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPMenuFlyoutExperiments {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {
        public MainPage() {
            this.InitializeComponent();
        }

        private void ShowContextMenu(UIElement sender, ContextRequestedEventArgs args) {
            FrameworkElement el = sender as FrameworkElement;
            args.Handled = true;

            MenuFlyout mf = new MenuFlyout();
            mf.Opened += MenuFlyoutOpened;

            MenuFlyoutItem mfi1 = new MenuFlyoutItem { Icon = new FontIcon { Glyph = "" }, Text = "Anna", Style = Application.Current.Resources["MenuFlyoutItemWithRightContentStyle"] as Style };
            MenuFlyoutItem mfi2 = new MenuFlyoutItem { Icon = new FontIcon { Glyph = "" }, Text = "Belle" };
            MenuFlyoutItem mfi3 = new MenuFlyoutItem { Icon = new FontIcon { Glyph = "" }, Text = "Carla" };

            mfi1.Tag = new Ellipse { 
                Width = 16,
                Height = 16,
                Fill = new SolidColorBrush(Colors.Red)
            };

            mf.Items.Add(mfi1);
            mf.Items.Add(mfi2);
            mf.Items.Add(mfi3);

            Point pos;
            args?.TryGetPosition(el, out pos);
            mf.ShowAt(el, pos);
        }

        private async void MenuFlyoutOpened(object sender, object e) {
            await Task.Delay(300);

            double windowWidth = Window.Current.Bounds.Width;
            double reactionsWidth = 192;
            double reactionsHeight = 32;
            MenuFlyout mf = sender as MenuFlyout;

            var first = mf.Items.FirstOrDefault();
            var transform = first.TransformToVisual(Window.Current.Content);
            var pos = transform.TransformPoint(new Point());

            var x = pos.X;
            var y = pos.Y - reactionsHeight - 8;

            if (x > windowWidth - reactionsWidth) x = windowWidth - reactionsWidth;

            Popup p = new Popup {
                Child = new Border { 
                    Background = new SolidColorBrush(Colors.Red),
                    Width = reactionsWidth,
                    Height = reactionsHeight,
                    Child = new TextBlock {
                        Text = "Reactions!",
                        FontSize = 20,
                        Foreground = new SolidColorBrush(Colors.White),
                        TextAlignment = TextAlignment.Right
                    }
                },
                Margin = new Thickness(x, y, 0, 0),
                ShouldConstrainToRootBounds = false,
                AllowFocusOnInteraction = false,
                IsOpen = true
            };

            mf.Closed += (a, b) => p.IsOpen = false;
        }
    }
}