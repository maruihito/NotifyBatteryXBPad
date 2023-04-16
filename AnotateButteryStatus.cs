using NotifyBatteryXBPad.Properties;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyBatteryXBPad
{
    public class AnotateButteryStatus
    {
        // 音声ファイル定義
        UnmanagedMemoryStream WAVFULL = Resources.s001_zunda_full;
        UnmanagedMemoryStream WAVMID = Resources.s002_zunda_mid;
        UnmanagedMemoryStream WAVLOW = Resources.s003_zunda_low;
        UnmanagedMemoryStream WAVEMP = Resources.s004_zunda_emp;
        UnmanagedMemoryStream WAVNODET = Resources.s005_zunda_nodet;

        private System.Media.SoundPlayer? player = null;

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
            player = new System.Media.SoundPlayer(wavfile);
            player.Play();
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
