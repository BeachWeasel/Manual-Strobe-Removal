using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Manual_Strobe_Removal
{
    class VidClip
    {
        // fileinfo
        public FileInfo VCFI { get; set; }

        public List<string> Proc_Vid_Paths { get; set; }

        // start time
        public TimeSpan StartTime { get; set; }

        // end time 
        public TimeSpan EndTime { get; set; }

        // duration
        public TimeSpan Duration { get; set; }

        // listed
        public bool Listed { get; set; }

        public VidClip(string file_path,string Start_Time, string End_Time,bool In_List)
        {
            VCFI = new FileInfo(file_path);
            List<string> Proc_paths = new List<string>();
            Proc_paths.Add(VCFI.FullName);
            Proc_Vid_Paths = Proc_paths;
            Listed = In_List;

            TimeSpan start = new TimeSpan();
            TimeSpan end = new TimeSpan();
            TimeSpan dur = new TimeSpan();

            CultureInfo culture = CultureInfo.CurrentCulture;
            
            TimeSpan.TryParse(Start_Time, culture, out start);
            TimeSpan.TryParse(End_Time, culture, out end);
            dur = end - start;

            StartTime = start;
            EndTime = end;
            Duration = dur;            
        }

        public VidClip(string file_path, TimeSpan Start_Time, TimeSpan End_Time, bool In_List)
        {
            VCFI = new FileInfo(file_path);
            List<string> Proc_paths = new List<string>();
            Proc_paths.Add(VCFI.FullName);
            Proc_Vid_Paths = Proc_paths;
            Listed = In_List;

            TimeSpan start = new TimeSpan();
            TimeSpan end = new TimeSpan();
            TimeSpan dur = new TimeSpan();

            start = Start_Time;
            end = End_Time;
            dur = end - start;

            StartTime = Start_Time;
            EndTime = End_Time;
            Duration = dur;
        }
    }
}
