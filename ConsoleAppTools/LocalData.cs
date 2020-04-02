using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTools
{
    internal static class LocalData
    {
        internal static int ProgressTotal = 0;
        internal static int ProgressValue = 0;
       
        internal static List<string> AnimationSymbols = new List<string> { " / ", "--", @" \ ", " | " };
        internal static int AnimationCounter = 0;

        internal static System.Timers.Timer TimerAnimation = new System.Timers.Timer();

        internal static System.Timers.Timer TimerProcessTimeCounter = new System.Timers.Timer();

        internal static TimeSpan ProcessTimeDuration;
        internal static DateTime ProcessStartDateTime = new DateTime();


        internal static TimeSpan SubProcessTimeDuration;
        internal static DateTime SubProcessStartDateTime = new DateTime();


        internal static string TitleTime = "00:00:00";
        internal static string TitleProgressbar = "";
        internal static string TitleAnimation = "";
        internal static string OpenBracket = "[";
        internal static string CloseBracket = "]";

        internal static string TitleStatus = string.Empty;


        internal static int TitleTimeLenght = 10;
        internal static int TitleProgressbarLenght = 60;
        internal static int TitleAnimationLenght = 14;

        internal static string AppName = "Blazor Azure Deploy";


    }
}
