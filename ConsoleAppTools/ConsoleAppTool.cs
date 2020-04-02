using System;
using System.Timers;

namespace ConsoleAppTools
{
    public class ConsoleAppTool
    {
        public static void Bootstrap()
        {

            Run();
         

            LocalData.TimerAnimation.Elapsed += new ElapsedEventHandler(AnimationOnTimedEvent);
            LocalData.TimerAnimation.Interval = 200;
            LocalData.TimerAnimation.Enabled = true;
            LocalData.TimerAnimation.Stop();

            LocalData.TimerProcessTimeCounter.Elapsed += new ElapsedEventHandler(ProcessTimeCounterOnTimedEvent);
            LocalData.TimerProcessTimeCounter.Interval = 1000;
            LocalData.TimerProcessTimeCounter.Enabled = true;
            LocalData.TimerProcessTimeCounter.Stop();
   
        }

        private static void Run()
        {
            Console.Title = LocalData.AppName;
            
            CATFunctions.WriteLine("Hello...");
        }


        private static void AnimationOnTimedEvent(object source, ElapsedEventArgs e)
        {
            CATFunctions.AnimationNext();
        }


        private static void ProcessTimeCounterOnTimedEvent(object source, ElapsedEventArgs e)
        {
            LocalData.ProcessTimeDuration = DateTime.Now - LocalData.ProcessStartDateTime;
            LocalData.TitleTime = " " + LocalData.ProcessTimeDuration.ToString(@"hh\:mm\:ss");
            CATFunctions.UpdateTitle();
        }


       


    }
}
