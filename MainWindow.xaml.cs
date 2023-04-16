using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vortice.XInput;

namespace NotifyBatteryXBPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        GamePadDetector gpd = new GamePadDetector();

        public MainWindow()
        {
            InitializeComponent();

            this.ResetText();
        }

        private void ChkButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateXPadStatus();
        }

        private void UpdateXPadStatus()
        {

            if(gpd.GetBatterystate())
            {
                // public enum BatteryLevel : byte
                // {
                //     Empty,  //  0
                //     Low,    //  1
                //     Medium, //  2
                //     Full    //  3
                // }
                byte balevel = (byte)gpd.BaLevel;

                BatteryFullTextBlock.Foreground   = (balevel == (byte)3) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.LightGray);
                BatteryMediumTextBlock.Foreground = (balevel == (byte)2) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.LightGray);
                BatteryLowTextBlock.Foreground    = (balevel == (byte)1) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.LightGray);
                BatteryEmptyTextBlock.Foreground  = (balevel == (byte)0) ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.LightGray);

                DetectTextBlock.Text = "Detected.";
                DetectTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                this.ResetText();
            }


        }

        private void ResetText()
        {
            BatteryFullTextBlock.Foreground = new SolidColorBrush(Colors.LightGray);
            BatteryMediumTextBlock.Foreground = new SolidColorBrush(Colors.LightGray);
            BatteryLowTextBlock.Foreground = new SolidColorBrush(Colors.LightGray);
            BatteryEmptyTextBlock.Foreground = new SolidColorBrush(Colors.LightGray);
            DetectTextBlock.Text = "No detected.";
            DetectTextBlock.Foreground = new SolidColorBrush(Colors.LightGray);
        }
    }
}
