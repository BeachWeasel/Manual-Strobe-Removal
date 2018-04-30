using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manual_Strobe_Removal
{
    public partial class Form1 : Form
    {
        Initial_Video IV;
        List<VidClip> Clips;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void ImportBTN_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                IV = new Initial_Video(ofd.FileName);
            }
        }

        // future work in detecting regions and automatically building VTC's for clipping
//        var proc = new Process
//        {
//            StartInfo = new ProcessStartInfo
//            {
//                FileName = "program.exe",
//                Arguments = "command line arguments to your executable",
//                UseShellExecute = false,
//                RedirectStandardOutput = true,
//                CreateNoWindow = true
//            }
//        };

//        proc.Start();
//while (!proc.StandardOutput.EndOfStream) {
//    string line = proc.StandardOutput.ReadLine();
//        // do something with line
//    }

//    proc.WaitForExit();

    private void ProcBTN_Click(object sender, EventArgs e)
        {
            // create directories for storing clips
            string Parent_Dir = IV.VFI.DirectoryName;
            string force_I_frame_dir = Path.Combine(Parent_Dir, "IFF");
            string modify_dir = Path.Combine(Parent_Dir, "mod");
            string concat_dir = Path.Combine(Parent_Dir, "concat");
            Directory.CreateDirectory(force_I_frame_dir);
            Directory.CreateDirectory(modify_dir);
            Directory.CreateDirectory(concat_dir);

            // build list of clips that are listed
            string comma_delim_vtc_ranges = ListedTB.Text;
            Clips = new List<VidClip>();
            // parse by ','
            string[] Array_comma_delim_vtc_ranges = comma_delim_vtc_ranges.Split(',');
            int counter = 0;
            foreach (var range in Array_comma_delim_vtc_ranges)
            {
                // parse by '_'
                string[] R = range.Split('_');
                //string format = "hh:mm:ss";
                string clip_name = counter.ToString() + "_L_" + Path.GetFileNameWithoutExtension(IV.VFI.FullName)+".mp4";
                string vc_path = Path.Combine(modify_dir, clip_name);
                VidClip vc = new VidClip(vc_path, R[0], R[1], true);
                vc.Proc_Vid_Paths.Add(vc_path);
                Clips.Add(vc);
                ++counter;
            }

            int count = Clips.Count;

            // add VTC ranges not in clipperTB
            TimeSpan Begining_of_vid = new TimeSpan(0, 0, 0);
            TimeSpan End_of_vid = IV.VidLength;

            Clips = Clips.OrderBy(o => o.StartTime).ToList();

            counter = 0;

            // add clips in between the user's selections
            if (count > 1)
            {
                for (int i = 0; i < count; i++)
                {
                    if (Clips[i].EndTime < Clips[i + 1].StartTime)
                    {
                        string clip_name = counter.ToString() + "_N_" + Path.GetFileNameWithoutExtension(IV.VFI.FullName)+".mp4";
                        string vc_path = Path.Combine(concat_dir, clip_name);
                        VidClip vc_inter = new VidClip(vc_path, Clips[i].EndTime, Clips[i + 1].StartTime, false);
                        vc_inter.Proc_Vid_Paths.Add(vc_path);
                        Clips.Add(vc_inter);
                        ++counter;
                    }
                }
            }
            Clips = Clips.OrderBy(o => o.StartTime).ToList();

            // add begining and endings of video
            string clip_name_end = counter.ToString() + "_N_" + Path.GetFileNameWithoutExtension(IV.VFI.FullName)+".mp4";
            string vc_path_end = Path.Combine(concat_dir, clip_name_end);
            VidClip vc_end = new VidClip(vc_path_end, Clips[Clips.Count - 1].EndTime, End_of_vid, false);
            vc_end.Proc_Vid_Paths.Add(vc_path_end);
            Clips.Add(vc_end);

            ++counter;

            Clips = Clips.OrderBy(o => o.StartTime).ToList();

            string clip_name_beg = counter.ToString() + "_N_" + Path.GetFileNameWithoutExtension(IV.VFI.FullName)+".mp4";
            string vc_path_beg = Path.Combine(concat_dir, clip_name_beg);
            VidClip vc_beg = new VidClip(vc_path_beg, Begining_of_vid, Clips[0].StartTime, false);
            vc_beg.Proc_Vid_Paths.Add(vc_path_beg);
            Clips.Add(vc_beg);

            Clips = Clips.OrderBy(o => o.StartTime).ToList();
            
            // force i frames in inital video and fps to be 24 // output mp4
            var ForceIFrameProc = new Process { StartInfo = new ProcessStartInfo(@"C:\ffmpeg\bin\ffmpeg.exe") };
            // location of forced i frame video:
            string init_vid_force_i_frame_path = Path.Combine(force_I_frame_dir, Path.GetFileNameWithoutExtension(IV.VFI.FullName)+".mp4");
            ForceIFrameProc.StartInfo.Arguments = "-i \"" + IV.VFI.FullName + "\"" +" -r 24 -g 12 -crf 20 -preset ultrafast " + "\""+ init_vid_force_i_frame_path + "\"";

            ForceIFrameProc.Start();
            ForceIFrameProc.WaitForExit();


            // create clips both listed and unlisted 
            #region

            foreach (var item in Clips)
            {
                //TimeSpan sixtysec_pre = new TimeSpan(0, 1, 0);
                //TimeSpan ffmpegStart = StartTime - sixtysec_pre;
                //TimeSpan ffmpegLen = StartTime - EndTime;

                TimeSpan sixtysec_pre = new TimeSpan(0, 1, 0);
                TimeSpan ffmpegStart = (item.StartTime > sixtysec_pre) ? item.StartTime - sixtysec_pre : item.StartTime;
                TimeSpan ffmpegLen = item.StartTime - item.EndTime;

                string Ssixtysec_pre = sixtysec_pre.ToString(@"hh\:mm\:ss"); ;// will be in arguments
                string SffmpegStart = ffmpegStart.ToString(@"hh\:mm\:ss"); ;// will be in arguments
                string SffmpegLen = ffmpegLen.ToString(@"hh\:mm\:ss"); ;// will be in arguments
                
                var ClipProc = new Process { StartInfo = new ProcessStartInfo(@"C:\ffmpeg\bin\ffmpeg.exe") };
                
                ClipProc.StartInfo.Arguments = (item.StartTime > sixtysec_pre) ?
                    "-ss " + SffmpegStart + " -i " + "\"" + init_vid_force_i_frame_path + "\"" + " -ss " + Ssixtysec_pre + " -t " + SffmpegLen + " -c:a copy -c:v copy " + "\"" + item.VCFI.FullName + "\""
                    :
                    "-ss " + SffmpegStart + " -i " + "\"" + init_vid_force_i_frame_path + "\"" + " -t " + SffmpegLen + " -c:a copy -c:v copy " + "\"" + item.VCFI.FullName + "\"";

                ClipProc.Start();
                ClipProc.WaitForExit();
            }

            #endregion

            // modify listed clips and rename to replace the listed clips // normalize or lut adjust // lower fps // raise fps to 24
            foreach (var item in Clips)
            {
                if(item.Listed)
                {
                    string NormPrefix = "Norm";
                    string fps1Prefix = "1fps";
                    string fps24Prefix = "24fps";

                    //// normalize
                    //string NormOutputPath = Path.Combine(modify_dir,NormPrefix+item.VCFI.Name);
                    //var NormProc = new Process { StartInfo = new ProcessStartInfo(@"C:\ffmpeg\bin\ffmpeg.exe") };

                    //NormProc.StartInfo.Arguments = "-i " + "\"" + item.Proc_Vid_Paths[item.Proc_Vid_Paths.Count-1] + "\"" + " -vf normalize=whitept=black:smoothing=7:independence=0:strength=0.7 -crf 20 -preset ultrafast " + "\"" + NormOutputPath + "\"";

                    //NormProc.Start();
                    //NormProc.WaitForExit();
                    //item.Proc_Vid_Paths.Add(NormOutputPath);

                    // lutyuv proc lutyuv="y=val/2" // consider not reducing luma
                    string NormOutputPath = Path.Combine(modify_dir, NormPrefix + item.VCFI.Name);
                    var NormProc = new Process { StartInfo = new ProcessStartInfo(@"C:\ffmpeg\bin\ffmpeg.exe") };

                    NormProc.StartInfo.Arguments = "-i " + "\"" + item.Proc_Vid_Paths[item.Proc_Vid_Paths.Count - 1] + "\"" + " -vf lutyuv=\"y=val\" -crf 20 -preset ultrafast " + "\"" + NormOutputPath + "\"";
                    //NormProc.StartInfo.Arguments = "-i " + "\"" + item.Proc_Vid_Paths[item.Proc_Vid_Paths.Count - 1] +  " -c:v copy -c:a copy "  + "\"" + NormOutputPath + "\"";

                    NormProc.Start();
                    NormProc.WaitForExit();
                    item.Proc_Vid_Paths.Add(NormOutputPath);

                    // lower fps to 1
                    string fps1OutputPath = Path.Combine(modify_dir, fps1Prefix+ NormPrefix + item.VCFI.Name);
                    var fps1Proc = new Process { StartInfo = new ProcessStartInfo(@"C:\ffmpeg\bin\ffmpeg.exe") };

                    fps1Proc.StartInfo.Arguments = "-i " + "\"" + item.Proc_Vid_Paths[item.Proc_Vid_Paths.Count - 1] + "\"" + " -preset ultrafast -crf 20 -r 0.75 -c:a copy -max_muxing_queue_size 400 " + "\"" + fps1OutputPath + "\"";

                    fps1Proc.Start();
                    fps1Proc.WaitForExit();
                    item.Proc_Vid_Paths.Add(fps1OutputPath);


                    //raise fps to 24 dup interpolation
                    //string fps24OutputPath = Path.Combine(concat_dir, fps24Prefix+fps1Prefix + NormPrefix + item.VCFI.Name);
                    //var fps24Proc = new Process { StartInfo = new ProcessStartInfo(@"C:\ffmpeg\bin\ffmpeg.exe") };

                    //fps24Proc.StartInfo.Arguments = "-i " + "\"" + item.Proc_Vid_Paths[item.Proc_Vid_Paths.Count - 1] + "\"" + " -map 0 -c copy -c:v h264 -filter:v \"minterpolate='fps=24 : mi_mode = dup'\" -max_muxing_queue_size 400 " + "\"" + fps24OutputPath + "\"";

                    //fps24Proc.Start();
                    //fps24Proc.WaitForExit();
                    //item.Proc_Vid_Paths.Add(fps24OutputPath);


                    // raise fps to 24 blend interpolation
                    string fps24OutputPath = Path.Combine(concat_dir, fps24Prefix + fps1Prefix + NormPrefix + item.VCFI.Name);
                    var fps24Proc = new Process { StartInfo = new ProcessStartInfo(@"C:\ffmpeg\bin\ffmpeg.exe") };

                    fps24Proc.StartInfo.Arguments = "-i " + "\"" + item.Proc_Vid_Paths[item.Proc_Vid_Paths.Count - 1] + "\"" + " -map 0 -c copy -c:v h264 -filter:v \"minterpolate='fps=24 : mi_mode = blend'\" -max_muxing_queue_size 400 " + "\"" + fps24OutputPath + "\"";

                    fps24Proc.Start();
                    fps24Proc.WaitForExit();
                    item.Proc_Vid_Paths.Add(fps24OutputPath);

                }

            }


            // create text file with all clip names for ffmpeg to use in concat
            string VidClipsTXTPath = "";
            try
            {
                VidClipsTXTPath = Path.Combine(concat_dir, "AllVidClips.txt");
                using (StreamWriter mylist = new StreamWriter(VidClipsTXTPath))
                {
                    foreach (var f in Clips)
                    {
                        //mylist.WriteLine("file '" + Path.GetFileNameWithoutExtension(f.Proc_Vid_Paths[f.Proc_Vid_Paths.Count - 1]) + ".mp4" + "'");
                        mylist.WriteLine("file '" + f.Proc_Vid_Paths[f.Proc_Vid_Paths.Count-1] + "'");
                    }
                }
            }
            catch(Exception ex)
            { MessageBox.Show(ex.ToString()); }


            // concat modified listed clips with unlisted clips

            string ConcatOutputPath = Path.Combine(concat_dir, "AllClips.mp4");
            var concatProc = new Process { StartInfo = new ProcessStartInfo(@"C:\ffmpeg\bin\ffmpeg.exe") };

            concatProc.StartInfo.Arguments = "-f concat -safe 0 -i \""+ VidClipsTXTPath + "\" -vf framerate=fps=24 -vsync -1 -crf 20 -preset ultrafast \""+ ConcatOutputPath + "\"";

            concatProc.Start();
            concatProc.WaitForExit();


        }
    }
}
