using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vortice.XInput;

namespace NotifyBatteryXBPad
{
    public class GamePadDetector
    {
        public int? GamepadIndex { get; set; }
        public BatteryInformation BaInfo { get; set; }

        public BatteryLevel BaLevel { get; set; }

        public GamePadDetector()
        {
            // コンストラクタ
            BaLevel = BatteryLevel.Empty;
            GamepadIndex = null;
        }

        public bool GetBatterystate()
        {
            // 前回認識済みのパッドがあれば再度参照
            if (GamepadIndex != null)
            {
                if (XInput.GetBatteryInformation(GamepadIndex.Value, (BatteryDeviceType)0U, out BatteryInformation BaInfo))
                {
                    if ((byte)BaInfo.BatteryType != (byte)BatteryType.Disconnected)
                    {
                        // BatteryTypeが"Disconnected"でない場合に限定
                        BaLevel = (BatteryLevel)BaInfo.BatteryLevel;
                        return true;
                    }
                }
            }

            // 前回認識済みのゲームパッドが切断されたとみなす
            GamepadIndex = null;

            // 0 ～ 3 の順で有効なゲームパッドを探す
            for (var i = 0; i < 4; ++i)
            { 
                if (XInput.GetBatteryInformation(i, (BatteryDeviceType)0U, out BatteryInformation BaInfo))
                {
                    if ((byte)BaInfo.BatteryType != (byte)BatteryType.Disconnected)
                    {
                        // BatteryTypeが"Disconnected"でない場合に限定
                        BaLevel = (BatteryLevel)BaInfo.BatteryLevel;
                        GamepadIndex = i; // ゲームパッドの番号を保存
                        return true;
                    }
                }
            }
            
            // ゲームパッドが見つからなかったことを報告
            return false;
        }
    }
}
