using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ConsoleAppTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BlazorAzureDeploy
{
    static class Utility
    {
        public static void Process(string SourceDir,
                                       BlobContainerClient container,
                                       IEnumerable<string> extensions,
                                       int cacheControlMaxAgeSeconds,
                                       string defaultContenType,
                                       bool clearContainer,
                                       bool syncContainer,
                                       IEnumerable<string> excludeDirs)
        {



            DirectoryInfo dirInfo = new DirectoryInfo(SourceDir);

            //step 1
            DeleteUnnecessaryFilesFromSourceDir(dirInfo, ".gz");
            DeleteUnnecessaryFilesFromSourceDir(dirInfo, ".br");

            List<FileInfo> fileslist = dirInfo.GetFiles("*", SearchOption.AllDirectories).ToList();

            //step 2
            ReportFilesWithNoExtension(fileslist);

            //step 3
            EnsureContentTypes(fileslist, defaultContenType);

            //step 4
            if (clearContainer)
            {

                EmptyContainer(container);

            }




            //step 5
            if (excludeDirs.Any())
            {
                fileslist.RemoveAll(x => excludeDirs.Any(y => y.ToLower().Equals(x.Directory.FullName.ToLower())));
            }


            //step 6 sync or upload
            if (syncContainer)
            {
                SyncFiles(SourceDir, container, fileslist, extensions, cacheControlMaxAgeSeconds);
            }
            else
            {
                UploadFiles(SourceDir, container, fileslist, extensions, cacheControlMaxAgeSeconds);
            }

            //step 7
            List<string> allExtensions = fileslist.Select(x => x.Extension.ToLower()).Distinct().ToList();

            CATFunctions.Print(string.Empty, true);
            foreach (var item in allExtensions.Where(x => !extensions.Any(y => y.ToLower().Equals(x))))
            {
                CATFunctions.Print(item + " extension was not compressed, is this ok?????????????????????");
            }
            CATFunctions.Print(string.Empty, true);

        }

        public static void SyncFiles(string SourceDir,
                                 BlobContainerClient container,
                                 List<FileInfo> fileslist,
                                 IEnumerable<string> extensions,
                                 int cacheControlMaxAgeSeconds)
        {


            var blobs = container.GetBlobs().ToList();


            List<string> localFilesList = new();
            foreach (var item in fileslist)
            {
                localFilesList.Add(GetFilePath(SourceDir, item, false).Replace(@"\", "/"));
            }

            //will also delete blobs which should be updated because their names in local folder are changed with prefix - shouldupdate_
            SyncDeleteObsoletedBlobs(container, blobs, localFilesList);

            List<FileInfo> ShouldRemoveFileInfos = new();
            foreach (var item in fileslist.Where(x=> !x.Name.Contains("shouldupdate_")))
            {
                if (blobs.Any(x => x.Name.Equals(GetFilePath(SourceDir, item, true).Replace(@"\", "/"), StringComparison.InvariantCultureIgnoreCase)))
                {
                    ShouldRemoveFileInfos.Add(item);
                }

            }


            foreach (var item in ShouldRemoveFileInfos)
            {
                fileslist.Remove(item);
            }

            if (fileslist.Any(x => x.Name.Contains("shouldupdate_")))
            {
                
                foreach (var item in fileslist.Where(x => x.Name.Contains("shouldupdate_")))
                {
                    //shouldupdate_ will be removed from name and updated regular way.
                    item.MoveTo(item.FullName.Replace("shouldupdate_", null));
                   
                }
            }





            if (fileslist.Any())
            {
                UploadFiles(SourceDir, container, fileslist, extensions, cacheControlMaxAgeSeconds);
            }
        }

            public static void UploadFiles(string SourceDir,
                                 BlobContainerClient container,
                                 IEnumerable<FileInfo> fileslist,
                                 IEnumerable<string> extensions,
                                 int cacheControlMaxAgeSeconds)
        {

            string cacheControlHeader = "public, max-age=" + cacheControlMaxAgeSeconds.ToString();


            CATFunctions.Print("Processing files...", true, false);

            CATFunctions.ShowProgress(fileslist.Count());
            CATFunctions.StartSubProcess("Uploading files...");


            Parallel.ForEach(fileslist, (fileInfo) =>
            {
                CATFunctions.Progress();


                ContentTypeHelper.CurrentContentTypes.TryGetValue(fileInfo.Extension, out string contentType);

                string filePath = GetFilePath(SourceDir, fileInfo, true);

                BlobClient blob = container.GetBlobClient(filePath);

                if (extensions.Contains(fileInfo.Extension, StringComparer.OrdinalIgnoreCase))
                {





                    CATFunctions.Print("Compress file - " + filePath);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (var brotliStream = new BrotliStream(memoryStream, CompressionMode.Compress, true))

                        using (var blobStream = fileInfo.OpenRead())
                        {
                            blobStream.CopyTo(brotliStream);


                        }


                        memoryStream.Position = 0;
                        blob.Upload(memoryStream);




                        CATFunctions.Print("uploading file - " + filePath);


                        BlobHttpHeaders headers = new()
                        {

                            ContentType = contentType,
                            ContentEncoding = "br",
                            CacheControl = cacheControlHeader,
                        };

                        blob.SetHttpHeadersAsync(headers);
                    }


                }
                else
                {

                    CATFunctions.Print("uploading file - " + filePath);
                    blob.Upload(fileInfo.FullName);


                    BlobHttpHeaders headers = new()
                    {

                        ContentType = contentType,
                        CacheControl = cacheControlHeader,
                    };

                    blob.SetHttpHeadersAsync(headers);
                }
            });


            CATFunctions.EndSubProcess();
            CATFunctions.FinishProgress();

        }




        public static void DeleteUnnecessaryFilesFromSourceDir(DirectoryInfo dirInfo, string ext)
        {
            IEnumerable<FileInfo> fileslist = dirInfo.GetFiles("*", SearchOption.AllDirectories).Where(x => x.Extension.Equals(ext));

            if (fileslist.Any())
            {
                CATFunctions.Print("======!!!!!!! Warning !!!!!!=======", true, false);
                CATFunctions.Print("Found " + fileslist.Count() + " files with extension " + ext);
                CATFunctions.Print(ext + " files are not necessary for deploy.", false, true);

                foreach (var item in fileslist)
                {
                    File.Delete(item.FullName);
                    CATFunctions.Print("Deleting local file - " + item.FullName);
                }
            }
        }

        public static void ReportFilesWithNoExtension(IEnumerable<FileInfo> fileslist)
        {
            if (fileslist.Any(x => string.IsNullOrEmpty(x.Extension)))
            {

                CATFunctions.Print("======!!!!!!! Warning !!!!!!=======", true, false);
                CATFunctions.Print("Found " + fileslist.Where(x => string.IsNullOrEmpty(x.Extension)).Count() + " files with no extension", false, true);

                foreach (var item in fileslist.Where(x => string.IsNullOrEmpty(x.Extension)))
                {
                    CATFunctions.Print("Warning - No file extension - " + item.FullName);
                }


            }
        }



        public static void SyncDeleteObsoletedBlobs(BlobContainerClient container, List<BlobItem> blobs, List<string> localFilesList)
        {


            List<string> ShouldDeleteBlobs = new();

            foreach (var item in blobs)
            {
                if (!localFilesList.Any(x => x.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    CATFunctions.Print("!!!!!!!!!!!!! should delete      " + item.Name, true, false);
                    ShouldDeleteBlobs.Add(item.Name);
                }
            }

            CATFunctions.Print("Deleting blobs not existing localy (" + ShouldDeleteBlobs.Count() + ")", true, false);
            CATFunctions.ShowProgress(ShouldDeleteBlobs.Count());
            CATFunctions.StartSubProcess("Deleting blobs not existing localy...");


            Parallel.ForEach(ShouldDeleteBlobs, (blobName) =>
            {

                container.DeleteBlob(blobName, DeleteSnapshotsOption.IncludeSnapshots);

                CATFunctions.Progress();
                CATFunctions.Print("Deleting blob " + blobName);
            });


            CATFunctions.Print("Container is cleared from obsoleted blobs", true, true);



            CATFunctions.EndSubProcess();
            CATFunctions.FinishProgress();

        }


        public static void EmptyContainer(BlobContainerClient container)
        {



            var blobs = container.GetBlobs();


            CATFunctions.Print("Deleting old blobs (" + blobs.Count() + ")", true, false);
            CATFunctions.ShowProgress(blobs.Count());
            CATFunctions.StartSubProcess("Deleting old blobs...");


            Parallel.ForEach(blobs, (blob) =>
            {

                container.DeleteBlob(blob.Name, DeleteSnapshotsOption.IncludeSnapshots);

                CATFunctions.Progress();
                CATFunctions.Print("Deleting blob " + blob.Name);
            });


            CATFunctions.Print("Container is empty", true, true);



            CATFunctions.EndSubProcess();
            CATFunctions.FinishProgress();

        }


        public static void EnsureContentTypes(IEnumerable<FileInfo> fileslist, string defaultContenType)
        {
            bool b = true;

            CATFunctions.Print("Checking content types...", true, true);

            List<string> fileExtensions = fileslist.Select(o => o.Extension).Distinct().ToList();
            ContentTypeHelper.CurrentContentTypes = new Dictionary<string, string>();



            string contentType;
            foreach (var item in fileExtensions)
            {
                if (string.IsNullOrEmpty(item))
                {
                    contentType = defaultContenType;
                }
                else
                {

                    if (ContentTypeHelper.contentTypes.TryGetValue(item, out contentType))
                    {
                        ContentTypeHelper.CurrentContentTypes.Add(item, contentType);
                    }
                    else
                    {
                        CATFunctions.Print("Warning - Content type is missing for " + item);
                        ContentTypeHelper.CurrentContentTypes.Add(item, defaultContenType);
                        b = false;
                    }
                }
            }


            if (!b)
            {
                CATFunctions.Print("======!!!!!!! Warning !!!!!!=======", true, false);
                CATFunctions.Print("For all missing cointent type extensions will be set to default or provided value - " + defaultContenType, false, true);

            }



            CATFunctions.Print("Content types are ready.", true, true);


        }

        public static string GetFilePath(string SourceDir, FileInfo fi, bool replaceShouldUpdate)
        {

            int SourceDirLenght = SourceDir.Length + 1;

            string result = fi.FullName.Substring(SourceDirLenght, fi.FullName.Length - SourceDirLenght);


            if (replaceShouldUpdate)
            {
                return result.Replace("shouldupdate_", null);
            }


            return result;
        }





    }
}
