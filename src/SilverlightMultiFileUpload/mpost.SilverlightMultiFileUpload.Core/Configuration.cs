using System.Collections.Generic;
using mpost.SilverlightFramework;

namespace mpost.SilverlightMultiFileUpload.Core
{
    public class Configuration
    {

        public string CustomParams { get; set; }
        public string FileFilter { get; set; }
        public string UploadHandlerName { get; set; }

        public int MaxUploads { get; set; }
        public int MaxFileSize { get; set; }

        public long ChunkSize { get; set; }
        public long WcfChunkSize { get; set; }

        public string SelectFileButtonImageSource { get; set; }

        private int _testInt;
        private long _testLong;

        private const string CustomParamsParam = "CustomParams";
        private const string MaxUploadsParam = "MaxUploads";
        private const string MaxFileSizeKBParam = "MaxFileSizeKB";
        private const string ChunkSizeParam = "ChunkSize";
        private const string FileFilterParam = "FileFilter";
        private const string UploadHandlerNameParam = "UploadHandlerName";
        private const string SelectFileButtonImageSourceParam = "SelectFileButtonImageSource";

        /// <summary>
        /// Load configuration first from initParams, then from .Config file
        /// </summary>
        /// <param name="initParams"></param>
        public Configuration(IDictionary<string, string> initParams)
        {
            //Defaults:
            MaxUploads = 2;
            ChunkSize = 1024 * 4096;
            WcfChunkSize = 16 * 1024;
            MaxFileSize = int.MaxValue;

            //Load settings from Init Params (if available)
            LoadFromInitParams(initParams);


            //Overwrite initParams using settings from .config file
            LoadFromConfigFile();

        }

        /// <summary>
        ///  Load settings from Init Params (if available)
        /// </summary>
        /// <param name="initParams"></param>
        private void LoadFromInitParams(IDictionary<string, string> initParams)
        {
            //Load Custom Config String
            if (initParams.ContainsKey(CustomParamsParam) && !string.IsNullOrEmpty(initParams[CustomParamsParam]))
                CustomParams = initParams[CustomParamsParam];

            if (initParams.ContainsKey(MaxUploadsParam) && !string.IsNullOrEmpty(initParams[MaxUploadsParam]))
            {
                if (int.TryParse(initParams[MaxUploadsParam], out _testInt))
                    MaxUploads = int.Parse(initParams[MaxUploadsParam]);
            }

            if (initParams.ContainsKey(MaxFileSizeKBParam) && !string.IsNullOrEmpty(initParams[MaxFileSizeKBParam]))
            {
                if (int.TryParse(initParams[MaxFileSizeKBParam], out _testInt))
                    MaxFileSize = int.Parse(initParams[MaxFileSizeKBParam]) * 1024;
            }

            if (initParams.ContainsKey(ChunkSizeParam) && !string.IsNullOrEmpty(initParams[ChunkSizeParam]))
            {
                if (long.TryParse(initParams[ChunkSizeParam], out _testLong))
                {
                    //Minimum Chunksize is 4096 bytes
                    if(_testLong >= 4096)
                        ChunkSize = int.Parse(initParams[ChunkSizeParam]);
                }
            }

            //if (initParams.ContainsKey(FileFilterParam) && !string.IsNullOrEmpty(initParams[FileFilterParam]))
            //    FileFilter = initParams[FileFilterParam];
            if (initParams.ContainsKey("FileFilter") && !string.IsNullOrEmpty(initParams["FileFilter"]))
            {
               if (initParams["FileFilter"].StartsWith("'"))
                  FileFilter = initParams["FileFilter"].Substring(1, initParams["FileFilter"].Length - 2);
               else
                  FileFilter = initParams["FileFilter"];
            }

            if (initParams.ContainsKey(UploadHandlerNameParam) && !string.IsNullOrEmpty(initParams[UploadHandlerNameParam]))
                UploadHandlerName = initParams[UploadHandlerNameParam];

            if (initParams.ContainsKey(SelectFileButtonImageSourceParam) && !string.IsNullOrEmpty(initParams[SelectFileButtonImageSourceParam]))
            {
               SelectFileButtonImageSource = initParams[SelectFileButtonImageSourceParam];
            }
        }



        /// <summary>
        /// Load settings from .config file
        /// </summary>
        private void LoadFromConfigFile()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[MaxFileSizeKBParam]))
            {
                if (int.TryParse(ConfigurationManager.AppSettings[MaxFileSizeKBParam], out _testInt))
                {
                    MaxFileSize = int.Parse(ConfigurationManager.AppSettings[MaxFileSizeKBParam]) * 1024;
                }
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[MaxUploadsParam]))
            {
                if (int.TryParse(ConfigurationManager.AppSettings[MaxUploadsParam], out _testInt))
                {
                    MaxUploads = int.Parse(ConfigurationManager.AppSettings[MaxUploadsParam]);
                }
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[FileFilterParam]))
                FileFilter = ConfigurationManager.AppSettings[FileFilterParam];
        }

    }
}
