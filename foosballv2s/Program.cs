﻿using System;
using System.IO;

namespace foosballv2s
{
    class Program
    {
        static void Main(string[] args)
        {
            String videoName = "\\data\\video_samples\\20170914_120510.mp4";
            try
            {
                String projectRootDirectory = Directory.GetParent(Directory.GetCurrentDirectory())
                    .Parent.FullName;
                //Console.WriteLine(projectRootDirectory + videoName);
                IVideo videoFile = new VideoFile(projectRootDirectory + videoName);
                MovementDetector detector = new MovementDetector(videoFile);
                detector.DetectBall();
            }
            catch (FileNotFoundException exception)
            {
                Console.WriteLine("Video file \"" + exception.Message + "\" could not be opened");
            }
            
        }
    }
}
