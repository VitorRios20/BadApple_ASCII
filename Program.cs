using System.Data;
using System;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using NAudio;
using NAudio.Wave;
using System.Linq;

namespace BadAppleAscii
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Animação ASCII : Bad Apple");

            List<String> frames = new List<String>();
            int quantFrames = Directory.GetFiles(@"BadApple\frames").Length;
            int frameIndex = 1;

            Console.Write("Loading the animation frames, please wait : ");
            while (true)
            {
                string frame = @"BadApple\frames\" + frameIndex.ToString() +".png";

                if (!File.Exists(frame))
                {
                    break;
                }

                using (Bitmap image = new Bitmap(frame))
                {
                    Bitmap bmp = new Bitmap(Console.WindowWidth - 1,Console.WindowHeight - 2);
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImage(image, 0, 0, Console.WindowWidth - 1, Console.WindowHeight - 2);

                    StringBuilder sb = new StringBuilder();
                    String chars = " .+@";

                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            int index = (int)(bmp.GetPixel(x, y).GetBrightness() * chars.Length);

                            if (index < 0)
                            {
                                index = 0;
                            }
                            else if (index >= chars.Length)
                            {
                                index = chars.Length - 1;
                            }
                            sb.Append(chars[index]);
                        }
                        sb.Append("\n");
                    }

                    frames.Add(sb.ToString());
                    frameIndex++;

                    int percentage = (int)((frames.Count / (float)(quantFrames)) * 100);

                   Console.SetCursorPosition(44, Console.CursorTop);
                   Console.Write("|"+ percentage.ToString() + "%" + " | processed frames : " + frames.Count.ToString()+" ");
                }
            }
            
            AudioFileReader reader = new AudioFileReader(@"BadApple\audio.wav");
            WaveOutEvent woe = new WaveOutEvent();
            woe.Init(reader);
            Console.WriteLine("\n\n press ENTER to start!");
            Console.ReadLine();
            woe.Play();

            while (true)
            {
                float percentage = woe.GetPosition() / (float)reader.Length;
                int frame = (int)(frames.Count * percentage);
                if (frame >= frames.Count)
                {
                    break;
                }
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(frames[frame].ToString());
            }
            Console.WriteLine("The END, bye:)");
            Console.ReadLine();
        }
    }
}
