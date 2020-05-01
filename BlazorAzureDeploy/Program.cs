using CommandLine;
using CommandLine.Text;
using ConsoleAppTools;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
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


            CloudStorageAccount storageAccount;

            if (!string.IsNullOrEmpty(opts.StorageAccount) && !string.IsNullOrEmpty(opts.StorageKey))
            {
                storageAccount = new CloudStorageAccount(new StorageCredentials(opts.StorageAccount, opts.StorageKey), true);
            }
            else
            {
                return;
            }

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(opts.Container);

            
            Utility.Process(opts.SourceDirectory,
                blobContainer,
                opts.Extensions,
                opts.MaxAgeSeconds,
                opts.DefaultContenType,
                opts.ClearContainer,
                opts.ExcludeDirs);

           
            if (opts.Wildcard)
            {
                Utility.SetWildcardCorsOnBlobService(storageAccount);
            }


            CATFunctions.Print("Process is done.", true, false);


            CATFunctions.DisplayProcessnigAnimation(false);
            CATFunctions.EndProcess();
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            CATFunctions.Print(errs.Count() + "errors occured");
            foreach (Error item in errs)
            {
                CATFunctions.Print(item.ToString());
            }
        }
    }


}
