using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.IO;
using SharpDX.Multimedia;
using SharpDX.XAudio2;

namespace SensorialRhythm
{
    public class SoundEffect
    {
        XAudio2 _xaudio;
        WaveFormat _waveFormat;
        AudioBuffer _buffer;
        SoundStream _soundstream;
        SourceVoice _sourceVoice;

        public bool IsPlaying { get; set; }

        public SoundEffect(string soundFxPath)
        {
            _xaudio = new XAudio2();
            var masteringsound = new MasteringVoice(_xaudio);

            var nativefilestream = new NativeFileStream(
            soundFxPath,
            NativeFileMode.Open,
            NativeFileAccess.Read,
            NativeFileShare.Read);

            _soundstream = new SoundStream(nativefilestream);
            _waveFormat = _soundstream.Format;
            _buffer = new AudioBuffer
            {
                Stream = _soundstream.ToDataStream(),
                AudioBytes = (int)_soundstream.Length,
                Flags = BufferFlags.EndOfStream
            };
        }

        public void Play()
        {
            _sourceVoice = new SourceVoice(_xaudio, _waveFormat, true);
            _sourceVoice.SubmitSourceBuffer(_buffer, _soundstream.DecodedPacketsInfo);
            _sourceVoice.Start();
            IsPlaying = true;
        }

        public void Stop()
        {
            _sourceVoice.Stop();
            IsPlaying = false;
        }


    }
}
