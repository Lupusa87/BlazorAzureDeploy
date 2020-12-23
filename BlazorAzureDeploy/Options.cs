using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAzureDeploy
{
    class Options
    {

        [Option('d', "sourceDirectory", Required = true,
            HelpText = "Source directory which should be uploaded.")]
        public string SourceDirectory { get; set; }


        [Option('c', "connstring", Required = false,
        HelpText = "Connection string. [mystorage]")]
        public string ConnectionString { get; set; }


        [Option('f', "container", Required = true,
           HelpText = "Container to search in.")]
        public string Container { get; set; }


        [Option('e', "extensions", Required = false, Default = new []
        { "dll", "wasm", "js", "png", "jpg", "css", "json", "pdb"},
            HelpText = "Extensions to gzip befor upload. [dll wasm js png jpg css json pdb]")]
        public IEnumerable<string> Extensions { get; set; }

       
        [Option('x', "cacheage", Required = false, Default = 2592000,
            HelpText = "Duration for cache control max age header, in seconds.  Default 2592000 (30 days).")]
        public int MaxAgeSeconds { get; set; }

        [Option('t', "defaultcontentype", Required = false, Default = "application/octet-stream",
           HelpText = "Default content type for missing extensions and files with no extension")]
        public string DefaultContenType { get; set; }

        [Option('z', "clearcontainer", Required = true, Default = true,
          HelpText = "Clear or not container before upload.")]
        public bool? ClearContainer { get; set; }


        [Option('p', "excludeDirs", Required = false,
           HelpText = "Excluded dirs will not be uploaded.")]
        public IEnumerable<string> ExcludeDirs { get; set; }



        [Option('s', "synccontainer", Required = false, Default = false,
      HelpText = "Sync folder and container (by file names). (adds new files from local, removes blobs not presented as files, does not update files)")]
        public bool? SyncContainer { get; set; }

    }
}
