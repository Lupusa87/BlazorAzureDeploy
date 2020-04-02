using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppTools
{
    public static class CATFunctions
    {

        public static void ShowProgress(int total)
        {
            LocalData.ProgressTotal = total;
            LocalData.ProgressValue = 0;
            if (LocalData.ProgressTotal > 0 && LocalData.ProgressValue <= LocalData.ProgressTotal)
            {
                DrawTextProgressBar();
            }
        }

        public static void Progress()
        {
            LocalData.ProgressValue++;
            if (LocalData.ProgressTotal > 0 && LocalData.ProgressValue <= LocalData.ProgressTotal)
            {
                DrawTextProgressBar();
            }

            //Thread.Sleep(150);
        }

        public static void FinishProgress()
        {
            LocalData.ProgressValue = 0;
            LocalData.ProgressTotal = 0;
            LocalData.TitleProgressbar = string.Empty;
            UpdateTitle();
          
        }

        public static void StartProcess(string ParStatus)
        {
            LocalData.ProcessStartDateTime = DateTime.Now;
            LocalData.TimerProcessTimeCounter.Start();

            LocalData.ProcessTimeDuration = DateTime.Now - LocalData.ProcessStartDateTime;
            LocalData.TitleTime = " " + LocalData.ProcessTimeDuration.ToString(@"hh\:mm\:ss");



            UpdateStatusInTitle(ParStatus);

            WriteLine("Main process starting " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
        }

        public static void EndProcess()
        {
            LocalData.TimerProcessTimeCounter.Stop();

            UpdateStatusInTitle(string.Empty);

            WriteLine("Main process started at " + LocalData.ProcessStartDateTime.ToString("dd.MM.yyyy HH:mm:ss"), true);
            WriteLine("Main process finished at " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

            LocalData.ProcessTimeDuration = DateTime.Now - LocalData.ProcessStartDateTime;
            WriteLine("Main process duration " + LocalData.ProcessTimeDuration.ToString(@"hh\:mm\:ss"));

        }


        public static void StartSubProcess(string ParStatus)
        {
            LocalData.SubProcessStartDateTime = DateTime.Now;
            UpdateStatusInTitle(ParStatus);

            WriteLine("Sub process starting " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
        }

        public static void EndSubProcess()
        {
            UpdateStatusInTitle(string.Empty);


            WriteLine("Sub process started at " + LocalData.SubProcessStartDateTime.ToString("dd.MM.yyyy HH:mm:ss"), true);
            WriteLine("Sub process finished at " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

            LocalData.SubProcessTimeDuration = DateTime.Now - LocalData.SubProcessStartDateTime;
            WriteLine("Sub process duration " + LocalData.SubProcessTimeDuration.ToString(@"hh\:mm\:ss"));

        }


        public static void DisplayProcessnigAnimation(bool b)
        {
            if (b)
            {
                LocalData.TimerAnimation.Start();
            }
            else
            {
                LocalData.TimerAnimation.Stop();
                LocalData.TitleAnimation = string.Empty;
                UpdateTitle();
            }


        }

        public static void MethodExecutionDone()
        {
            Console.Write("-> ");
        }

        public static void DrawTextProgressBar()
        {
            StringBuilder sb1 = new StringBuilder();
            //https://stackoverflow.com/questions/24918768/progress-bar-in-console-application

            if (LocalData.ProgressTotal > 0 && LocalData.ProgressValue <= LocalData.ProgressTotal)
            {

                int totalChunks = 30;
                double pctComplete = 0.0;
                int numChunksComplete = 0;


                sb1.Append(" (");


                if (LocalData.ProgressValue > 0)
                {
                    pctComplete = Convert.ToDouble(LocalData.ProgressValue) / Convert.ToDouble(LocalData.ProgressTotal);
                    numChunksComplete = Convert.ToInt16(totalChunks * pctComplete);
                }
                else
                {
                    pctComplete = 0.0;
                    numChunksComplete = 0;
                }

                sb1.Append(new string('>', numChunksComplete));
                sb1.Append(new string('=', totalChunks - numChunksComplete));

                sb1.Append(") ");
                sb1.Append(CorrectProgressBarValue() + " of " + LocalData.ProgressTotal.ToString() + " ");
            }

            LocalData.TitleProgressbar = sb1.ToString();
            UpdateTitle();
        }

        public static string CorrectProgressBarValue()
        {
            string result = (LocalData.ProgressValue + 1).ToString();


            string b = LocalData.ProgressTotal.ToString();

            if (result.Length < b.Length)
            {
                result = new string(' ', b.Length - result.Length) + result;
                result = new string(' ', b.Length - result.Length) + result;
            }


            return result;
        }

        public static void DrawTextProgressBar2(string stepDescription)
        {

            //https://stackoverflow.com/questions/24918768/progress-bar-in-console-application

            if (LocalData.ProgressTotal > 0 && LocalData.ProgressValue <= LocalData.ProgressTotal)
            {

                int totalChunks = 30;
                double pctComplete = 0.0;
                int numChunksComplete = 0;

                //draw empty progress bar
                Console.CursorLeft = 0;
                Console.Write("["); //start
                Console.CursorLeft = totalChunks + 1;
                Console.Write("]"); //end
                Console.CursorLeft = 1;

                if (LocalData.ProgressValue > 0)
                {
                    pctComplete = Convert.ToDouble(LocalData.ProgressValue) / Convert.ToDouble(LocalData.ProgressTotal);
                    numChunksComplete = Convert.ToInt16(totalChunks * pctComplete);
                }
                else
                {
                    pctComplete = 0.0;
                    numChunksComplete = 0;
                }

                //draw completed chunks
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("".PadRight(numChunksComplete));

                //draw incomplete chunks
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write("".PadRight(totalChunks - numChunksComplete));

                //draw totals
                Console.CursorLeft = totalChunks + 5;
                Console.BackgroundColor = ConsoleColor.Black;

                string output = LocalData.ProgressValue.ToString() + " of " + LocalData.ProgressTotal.ToString();
                Console.Write(output.PadRight(15) + stepDescription + Environment.NewLine);
                //pad the output so when changing from 3 to 4 digits we avoid text shifting
            }
        }


        internal static void WriteLine(string ParInput, bool ParAddEmptyLineBefore = false, bool ParAddEmptyLineAfter = false)
        {
            if (ParAddEmptyLineBefore)
            {
                Console.WriteLine("->");
            }

            Console.Write("-> ");
            Console.WriteLine(ParInput);

            if (ParAddEmptyLineAfter)
            {
                Console.WriteLine("->");
            }
        }



        public static void Print(string ParText, bool ParAddEmptyLineBefore = false, bool ParAddEmptyLineAfter = false)
        {
            WriteLine(ParText, ParAddEmptyLineBefore, ParAddEmptyLineAfter);
        }

        public static void UpdateStatusInTitle(string ParStatus)
        {
            LocalData.TitleStatus = ParStatus;
            UpdateTitle();
        }




        internal static void ClearCurrentConsoleLine2()
        {
            int currentLineCursor = Console.CursorTop - 1;
            Console.SetCursorPosition(0, Console.CursorTop);
            //for (int i = 0; i < Console.WindowWidth; i++)
            //    Console.Write(" ");


            Console.Write(new string(' ', Console.WindowWidth));

            Console.SetCursorPosition(0, currentLineCursor);

        }




        public static void AnimationNext()
        {
            string a = LocalData.AnimationSymbols[LocalData.AnimationCounter];
            string b = string.Empty;
            LocalData.TitleAnimation = "  " + a + b + a + b + a;
            UpdateTitle();

            LocalData.AnimationCounter++;
            if (LocalData.AnimationCounter == LocalData.AnimationSymbols.Count)
            {
                LocalData.AnimationCounter = 0;
            }
        }

        public static void UpdateTitle()
        {

            if (string.IsNullOrEmpty(LocalData.TitleTime))
            {
                LocalData.TitleTime = new string(' ', LocalData.TitleTimeLenght);
            }
            else
            {
                if (LocalData.TitleTime.Length < LocalData.TitleTimeLenght)
                {
                    LocalData.TitleTime = LocalData.TitleTime + new string(' ', LocalData.TitleTimeLenght - LocalData.TitleTime.Length);
                }
            }


            if (string.IsNullOrEmpty(LocalData.TitleProgressbar))
            {
                LocalData.TitleProgressbar = new string(' ', LocalData.TitleProgressbarLenght);
            }
            else
            {
                if (LocalData.TitleProgressbar.Length < LocalData.TitleProgressbarLenght)
                {
                    LocalData.TitleProgressbar = LocalData.TitleProgressbar + new string(' ', LocalData.TitleProgressbarLenght - LocalData.TitleProgressbar.Length);
                }
            }

            if (string.IsNullOrEmpty(LocalData.TitleAnimation))
            {
                LocalData.TitleAnimation = new string(' ', LocalData.TitleAnimationLenght);
            }
            else
            {
                if (LocalData.TitleAnimation.Length < LocalData.TitleAnimationLenght)
                {
                    LocalData.TitleAnimation = LocalData.TitleAnimation + new string(' ', LocalData.TitleAnimationLenght - LocalData.TitleAnimation.Length);
                }
            }

            StringBuilder sb1 = new StringBuilder();

            sb1.Append(LocalData.AppName + " ");

            sb1.Append(LocalData.OpenBracket + LocalData.TitleTime + LocalData.CloseBracket + " ");

            sb1.Append(LocalData.TitleStatus + " ");


            sb1.Append(LocalData.OpenBracket + LocalData.TitleProgressbar + LocalData.CloseBracket + " ");
            sb1.Append(LocalData.OpenBracket + LocalData.TitleAnimation + LocalData.CloseBracket);
            Console.Title = sb1.ToString();

        }
    }
}
