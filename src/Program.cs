using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageFormatChanger
{
    class Program
    {

        static string SourcePath = null;
        static string DestinationPath = null;
        static string SrcType = null;
        static string DesType = null;

        static void Main(string[] args)
        {
            Console.Title = "Image Format Changer";

            // Get User Inputs
            Console.Write("Enter the source folder: ");
            SourcePath = @"" + Console.ReadLine();
            if (!Directory.Exists(SourcePath))
            {
                SendError("Source path does not exist!");
            }
            Console.Write("Enter the target folder: ");
            DestinationPath = @"" + Console.ReadLine();
            if (!Directory.Exists(DestinationPath))
            {
                SendError("Destination path does not exist!");
            }
            Console.Write("Source Format [bmp/gif/jpg/jpeg/ico/png/tif] : ");
            SrcType = Console.ReadLine();
            if (IsInvalidFormat(SrcType))
            {
                SendError("You did you enter a supported image source format");
            }
            Console.Write("Target Format [bmp/gif/jpg/jpeg/ico/png/tif] : ");
            DesType = Console.ReadLine();

            if (IsInvalidFormat(DesType))
            {
                SendError("You did you enter a supported image destination format");
            }

            // Display Data
            string[] files = Directory.GetFiles(SourcePath);
            string[] requiredFiles = files.Where(c => c.EndsWith(SrcType, StringComparison.OrdinalIgnoreCase)).ToArray();
            Console.Clear();
            Console.WriteLine("SOURCE: \"" + SourcePath + "\"");
            Console.WriteLine("DESTINATION:\"" + DestinationPath + "\"");
            Console.WriteLine("TOTAL FILES: " + files.Length);
            Console.WriteLine("TOTAL " + SrcType + " FILES: " + requiredFiles.Length);
            Console.WriteLine();
            SendConsole("Press any key to start conversion...", ConsoleColor.Blue);
            Console.ReadKey();

            // Start Conversion
            int success = 0;
            int failed = 0;
            for (int i = 0; i < requiredFiles.Length; i++)
            {
                int percentage = (int)Math.Round((double)(100 * i) / requiredFiles.Length); ;
                Console.Title = percentage + "% Completed";
                Boolean isConverted = Convert(requiredFiles[i]);
                if (isConverted)
                {
                    success += 1;
                }
                else
                {
                    failed += 1;
                }
            }
            Console.WriteLine();
            SendConsole("Conversion Completed!", ConsoleColor.Blue);
            SendConsole("SUCCESS: " + success, ConsoleColor.Green);
            SendConsole("FAILED: " + failed, ConsoleColor.Red);
            Console.ReadKey();
        }

        static Boolean IsInvalidFormat(string format)
        {
            if (
                format.ToLower().Equals("bmp") ||
                format.ToLower().Equals("gif") ||
                format.ToLower().Equals("jpg") ||
                format.ToLower().Equals("jpeg") ||
                format.ToLower().Equals("ico") ||
                format.ToLower().Equals("png") ||
                format.ToLower().Equals("tif")
                )
            {
                return false;
            }
            return true;
        }


        static Boolean Convert(string file)
        {
            try
            {
                string name = Path.GetFileName(file);
                string nameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                Console.WriteLine("Converting " + name);
                Bitmap bitmap = new Bitmap(file);
                string des = DestinationPath + "\\" + nameWithoutExtension + "." + DesType;
                bitmap.Save(des, getImageFormat(SrcType));
                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                return false;
            }

        }

        static ImageFormat getImageFormat(string in_ext)
        {
            ImageFormat if_ret = new ImageFormat(Guid.NewGuid());
            switch (in_ext.ToLower())
            {
                case "bmp":
                    if_ret = ImageFormat.Bmp;
                    break;
                case "gif":
                    if_ret = ImageFormat.Gif;
                    break;
                case "jpg":
                case "jpeg":
                    if_ret = ImageFormat.Jpeg;
                    break;
                case "ico":
                    if_ret = ImageFormat.Icon;
                    break;
                case "png":
                    if_ret = ImageFormat.Png;
                    break;
                case "tif":
                    if_ret = ImageFormat.Tiff;
                    break;
            }
            return if_ret;
        }

        static void SendError(string message)
        {
            Console.WriteLine();
            SendConsole(message, ConsoleColor.Red);
            Console.ReadKey();
            Environment.Exit(0);
        }

        static void SendConsole(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

    }
}
