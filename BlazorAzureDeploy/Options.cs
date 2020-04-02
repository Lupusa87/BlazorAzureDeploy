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

        [Option('a', "account", Required = false,
         HelpText = "Storage account host. [mystorage]")]
        public string StorageAccount { get; set; }

        [Option('k', "key", Required = false,
            HelpText = "Storage account key.")]
        public string StorageKey { get; set; }


        [Option('f', "container", Required = true,
           HelpText = "Container to search in.")]
        public string Container { get; set; }


        [Option('e', "extensions", Required = false, Default = new []
        { "dll", "wasm", "js", "png", "jpg", "css", "json", "pdb"},
            HelpText = "Extensions to gzip befor upload. [dll, wasm, js, png jpg css json pdb]")]
        public IEnumerable<string> Extensions { get; set; }

       
        [Option('x', "cacheage", Required = false, Default = 2592000,
            HelpText = "Duration for cache control max age header, in seconds.  Default 2592000 (30 days).")]
        public int MaxAgeSeconds { get; set; }

        [Option('w', "wildcardcors", Required = false, Default = false,
            HelpText = "Enable wildcard CORS for this storage account.")]
        public bool Wildcard { get; set; }

        [Option('t', "defaultcontentype", Required = false, Default = "application/octet-stream",
           HelpText = "Default content type for missing extensions and files with no extension")]
        public string DefaultContenType { get; set; }

        [Option('z', "clearcontainer", Required = true, Default = true,
          HelpText = "Clear or not container before upload.")]
        public bool ClearContainer { get; set; }

    }
}
