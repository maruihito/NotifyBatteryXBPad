using NotifyBatteryXBPad.Properties;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace NotifyBatteryXBPad
{
    public class AnotateButteryStatus
    {
        public byte SoundVolume { get; set; }

        // 音声ファイル定義
        UnmanagedMemoryStream WAVFULL = Resources.s001_zunda_full;
        UnmanagedMemoryStream WAVMID = Resources.s002_zunda_mid;
        UnmanagedMemoryStream WAVLOW = Resources.s003_zunda_low;
        UnmanagedMemoryStream WAVEMP = Resources.s004_zunda_emp;
        UnmanagedMemoryStream WAVNODET = Resources.s005_zunda_nodet;

        private SoundPlayer? player = null;

        public AnotateButteryStatus()
        {
            // コンストラクタ
            SoundVolume = 50;
        }


        public void PlayWavFull()
        {
            PlayWaveFile(WAVFULL);
        }

        public void PlayWavMid()
        {
            PlayWaveFile(WAVMID);
        }

        public void PlayWavLow()
        {
            PlayWaveFile(WAVLOW);
        }
        public void PlayWavEmp()
        {
            PlayWaveFile(WAVEMP);
        }

        public void PlayWavNodet()
        {
            PlayWaveFile(WAVNODET);
        }

        private void PlayWaveFile(UnmanagedMemoryStream wavfile)
        {
            wavfile.Position = 0;           // Manually rewind stream 
            StopWaveFile();                 // Then we have to set stream to null 
            var wavstream = new WaveStream(wavfile);
            wavstream.Volume = SoundVolume;

            using (player = new SoundPlayer(wavstream))
            {
                player.Play();
            }
        }

        public void StopWaveFile()
        {
            if (player != null)
            {
                player.Stop();
                player.Dispose();
                player = null;
            }
        }
    }
}
