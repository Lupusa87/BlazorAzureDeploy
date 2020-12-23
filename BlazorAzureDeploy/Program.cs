using Azure.Storage;
using Azure.Storage.Blobs;
using CommandLine;
using CommandLine.Text;
using ConsoleAppTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace BlazorAzureDeploy
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleAppTool.Bootstrap();

            Parser.Default.ParseArguments<Options>(args)
              .WithParsed(RunOptions)
              .WithNotParsed(HandleParseError);
        }
        static void RunOptions(Options opts)
        {

            //if (string.IsNullOrEmpty(opts.Extension) && !opts.Replace)
            //{
         
            //    return;
            //}

            CATFunctions.StartProcess("");
            CATFunctions.DisplayProcessnigAnimation(true);


            BlobServiceClient service = new BlobServiceClient(opts.ConnectionString);

            BlobContainerClient blobContainer = service.GetBlobContainerClient(opts.Container);


            CATFunctions.Print("!!!!!!!!!!!!!!!!!!!!!!!!!!!", true, false);
            CATFunctions.Print(opts.SyncContainer.ToString(), false, false);
            CATFunctions.Print("!!!!!!!!!!!!!!!!!!!!!!!!!!!", false, true);




            bool sync = false;
            if (opts.SyncContainer.HasValue)
            {
                if (opts.SyncContainer.Value)
                {
                    sync=true;
                }
            }

            Utility.Process(opts.SourceDirectory,
                blobContainer,
                opts.Extensions,
                opts.MaxAgeSeconds,
                opts.DefaultContenType,
                opts.ClearContainer,
                sync,
                opts.ExcludeDirs);


            CATFunctions.Print("Process is done.", true, false);


            CATFunctions.DisplayProcessnigAnimation(false);
            CATFunctions.EndProcess();
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            CATFunctions.Print(errs.Count() + " errors occured");
            foreach (Error item in errs)
            {
                CATFunctions.Print(item.ToString());
            }
        }
    }


}
