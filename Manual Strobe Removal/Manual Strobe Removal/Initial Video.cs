using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Shell32;

namespace Manual_Strobe_Removal
{
    class Initial_Video
    {
        public FileInfo VFI { get; set; } 

        public TimeSpan VidLength { get; set; }

        public Initial_Video(string VideoPath)
        {
            VFI = new FileInfo(VideoPath);

            TimeSpan duration;// = new TimeSpan();

            if (GetDuration(@VideoPath, out duration))
            {
                Console.WriteLine("Video Duration: {0}", duration);
                VidLength = duration;
            }
            else
            {
                Console.WriteLine("Epic Fail :(");
            }

        }

        public static bool GetDuration(string filename, out TimeSpan duration)
        {
            try
            {
                var shl = new Shell();
                var fldr = shl.NameSpace(Path.GetDirectoryName(filename));
                var itm = fldr.ParseName(Path.GetFileName(filename));

                // Index 27 is the video duration [This may not always be the case]
                var propValue = fldr.GetDetailsOf(itm, 27);

                return TimeSpan.TryParse(propValue, out duration);
            }
            catch (Exception)
            {
                duration = new TimeSpan();
                return false;
            }
        }

    }
}
