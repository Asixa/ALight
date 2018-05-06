using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ALight.Render.Denoise
{
    public class Denoiser
    { 
        [DllImport("OptixAIDenoiser.dll", CharSet = CharSet.Ansi)]
        public static extern int DenoiseImp(int file_count, IntPtr[] input_files, IntPtr[] output_files, float blend, ImageCallBack image_callback, ProgressCallBack progress_CallBack, FinishedCallBack finished_callback);
        public static int Denoise(ImageCallBack image_callback, ProgressCallBack progress_CallBack, FinishedCallBack finished_callback)
        {
            if (File.Exists(input_image))
            {
                if (Directory.Exists(output_image.Substring(0, output_image.LastIndexOf('\\'))))
                {
                    input_files = new IntPtr[1];
                    output_files = new IntPtr[1];
                    input_files[0] = Marshal.StringToHGlobalAnsi(input_image);
                    output_files[0] = Marshal.StringToHGlobalAnsi(output_image);
                    return DenoiseImp(1, input_files, output_files, blend, image_callback, progress_CallBack, finished_callback);
                }
                MessageBox.Show("The output directory does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                MessageBox.Show("The input image does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return -1;
        }
       
        public static string input_image = ".\\in.png";
        public static string output_image = ".\\out.png";
        public static float blend = 0.05f;
        public static IntPtr[] input_files;
        public static IntPtr[] output_files;
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int ImageCallBack(IntPtr data, int w, int h, int size);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int ProgressCallBack(float progress);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int FinishedCallBack();
    }
}
