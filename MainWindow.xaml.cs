using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;
using Vortice.XInput;

namespace NotifyBatteryXBPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // タイマのインスタンス
        private DispatcherTimer _timer;

        int? GamepadIndex = null;

        // 音声再生クラス
        AnotateButteryStatus abs = new AnotateButteryStatus();

        public MainWindow()
        {
            InitializeComponent();

            this.MinuteTextBlock.Text = "30";
            this.ResetText();

            this.SetupTimer();

            // 音量は50に設定
            VolComboBox.SelectedIndex = 2;
        }

        private void ChkButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateXPadStatus();
        }

        // タイマを設定する
        private void SetupTimer()
        {
            // GUIの設定を読み取り
            int valminute = (int)MinuteSlider.Value;

            // タイマのインスタンスを生成
            _timer = new DispatcherTimer(); // 優先度はDispatcherPriority.Background
                                            // インターバルを設定
            _timer.Interval = new TimeSpan(0, valminute, 0);
            // タイマメソッドを設定
            _timer.Tick += new EventHandler(MyTimerMethod);
            // タイマを開始
            _timer.Start();

            // 画面が閉じられるときに、タイマを停止
            this.Closing += new CancelEventHandler(StopTimer);
        }

        // タイマメソッド
        private void MyTimerMethod(object sender, EventArgs e)
        {
            // ゲームパッド情報の更新
            UpdateXPadStatus();

        }

        // タイマを停止
        private void StopTimer(object sender, CancelEventArgs e)
        {
            _timer.Stop();
        }

        private void UpdateXPadStatus()
        {
            GamePadDetector gpd = new GamePadDetector(GamepadIndex);

            Task.WaitAll();
            string? volumestr = VolComboBox.SelectedItem.ToString();
            string[] volumestrsp = volumestr.Split(':');

            abs.SoundVolume = (byte)Convert.ToInt32(volumestrsp[1]);

            if (gpd.GetBatterystate())
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

                if(balevel == (byte)0)
                {
                    Task.Run(abs.PlayWavEmp);
                }
                else if(balevel == (byte)1)
                {
                    Task.Run(abs.PlayWavLow);
                }
                else if (balevel == (byte)2)
                {
                    Task.Run(abs.PlayWavMid);
                }
                else if((balevel == (byte)3))
                {
                    Task.Run(abs.PlayWavFull);
                }
            }
            else
            {
                this.ResetText();

                Task.Run(abs.PlayWavNodet);
            }

            // ゲームパッドの識別番号を記憶してインスタンスは閉じる
            GamepadIndex = gpd.GamepadIndex;

            gpd.Dispose();
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

        private void MinuteSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(MinuteTextBlock == null)
            {
                return;
            }
            int minval = (int)MinuteSlider.Value;
            MinuteTextBlock.Text = minval.ToString();

            if (_timer != null)
            {
                _timer.Stop();
            }

            SetupTimer();

        }
    }
}
