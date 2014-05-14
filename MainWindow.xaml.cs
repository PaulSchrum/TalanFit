using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TalanFit
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      public MainWindow()
      {
         InitializeComponent();
      }

      private void Window_KeyUp(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Escape) Environment.Exit(0);
      }

      private void Window_ContentRendered(object sender, EventArgs e)
      {
         var grid = (Grid)this.Content;
         if (null == grid) return;
         var dc = ((WiiBalanceBoardViewModel)grid.DataContext);
         if (null == dc) return;
         dc.Gem.parentHeight = (Single)this.Oscilloscope.ActualHeight;
         dc.Gem.parentWidth = (Single)this.Oscilloscope.ActualWidth;
      }
   }
}
