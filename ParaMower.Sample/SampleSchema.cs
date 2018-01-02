using System;
using ParaMower.Supports;

namespace ParaMower.Sample
{
    public class SampleSchema
    {
        public ModeTag ModeTag
        {
            get
            {
                var success = Enum.TryParse(ModeTagStr, true, out ModeTag modeTag);
                return success ? modeTag : ModeTag.Online;
            }
            set => ModeTagStr = value.ToString();
        }

        [CommandParam("m", "Mode tag[Online|Offline], Default is Online")]
        private string ModeTagStr { get; set; } = ModeTag.Online.ToString();

        [CommandParam("c", "Broadcast description file path[filename/filepath]")]
        public string ResDescConfigPath { get; set; }

        public AudioFormat OutputFormat
        {
            get
            {
                var success = Enum.TryParse(FormatStr, true, out AudioFormat format);
                return success ? format : AudioFormat.Wav;
            }
            set => FormatStr = value.ToString();
        }

        [CommandParam("f", "Output audio format[Wav|Mp3], Default is Wav")]
        private string FormatStr { get; set; } = AudioFormat.Wav.ToString();

        public QualityLevel OutputQuality
        {
            get
            {
                var success = Enum.TryParse(QualityStr, true, out QualityLevel level);
                return success ? level : QualityLevel.Medium;
            }
            set => QualityStr = value.ToString();
        }

        [CommandParam("q", "Output quality[Low|Medium|High], Default is Medium")]
        internal string QualityStr { get; set; } = QualityLevel.Medium.ToString();
        [CommandParam("o", "Output file name[name], Default pattern is 'yyyy-MM-dd_{5 bit guid}'")]
        public string OutputFileName { get; set; } = $"{DateTimeOffset.UtcNow:yyyy-MM-dd}_{Guid.NewGuid().ToString("N").Substring(0, 5)}";

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ResDescConfigPath);
        }
    }

    public enum ModeTag
    {
        Online,
        Offline
    }

    public enum AudioFormat
    {
        Wav,
        Mp3
    }

    public enum QualityLevel
    {
        High,
        Medium,
        Low
    }
}
