using Plugin.Maui;
using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Helpers
{
    public class AudioService
    {
        private readonly IAudioManager _audioManager;
        private IAudioPlayer _audioPlayer;

        public AudioService(IAudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public async Task PlaySoundAsync(string fileName)
        {
            // Carga el archivo de sonido desde Resources/Raw y crea el reproductor de audio
            _audioPlayer = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(fileName));
            _audioPlayer.Play();
        }
    }
}
