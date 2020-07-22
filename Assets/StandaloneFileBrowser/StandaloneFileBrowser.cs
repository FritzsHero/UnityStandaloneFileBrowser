using System;


namespace SFB
{
    public struct ExtensionFilter
    {
        public string name;
        public string[] extensions;


        public ExtensionFilter(string _filterName, params string[] _filterExtensions)
        {
            name = _filterName;
            extensions = _filterExtensions;
        }
    }


    public class StandaloneFileBrowser
    {
        private static IStandaloneFileBrowser _platformWrapper = null;


        static StandaloneFileBrowser()
        {
#if UNITY_STANDALONE_OSX
            _platformWrapper = new StandaloneFileBrowserMac();
#elif UNITY_STANDALONE_WIN
            _platformWrapper = new StandaloneFileBrowserWindows();
#elif UNITY_STANDALONE_LINUX
            _platformWrapper = new StandaloneFileBrowserLinux();
#elif UNITY_EDITOR
            _platformWrapper = new StandaloneFileBrowserEditor();
#endif
        }


        /// <summary>
        /// Native open file dialog
        /// </summary>
        /// <param name="_title">Dialog title</param>
        /// <param name="_directory">Root directory</param>
        /// <param name="_extension">Allowed extension</param>
        /// <param name="_isMultiselect">Allow multiple file selection</param>
        /// <returns>Returns array of chosen paths. Zero length array when cancelled</returns>
        public static string[] OpenFilePanel(string _title, string _directory, string _extension, bool _isMultiselect)
        {
            var extensions = string.IsNullOrEmpty(_extension) ? null : new [] { new ExtensionFilter("", _extension) };
            return OpenFilePanel(_title, _directory, extensions, _isMultiselect);
        }


        /// <summary>
        /// Native open file dialog
        /// </summary>
        /// <param name="_title">Dialog title</param>
        /// <param name="_directory">Root directory</param>
        /// <param name="_extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param name="_isMultiselect">Allow multiple file selection</param>
        /// <returns>Returns array of chosen paths. Zero length array when cancelled</returns>
        public static string[] OpenFilePanel(string _title, string _directory, ExtensionFilter[] _extensions, bool _isMultiselect)
        {
            return _platformWrapper.OpenFilePanel(_title, _directory, _extensions, _isMultiselect);
        }


        /// <summary>
        /// Native open file dialog async
        /// </summary>
        /// <param name="_title">Dialog title</param>
        /// <param name="_directory">Root directory</param>
        /// <param name="_extension">Allowed extension</param>
        /// <param name="_isMultiselect">Allow multiple file selection</param>
        /// <param name="_cb">Callback")</param>
        public static void OpenFilePanelAsync(string _title, string _directory, string _extension, bool _isMultiselect, Action<string[]> _cb)
        {
            var extensions = string.IsNullOrEmpty(_extension) ? null : new [] { new ExtensionFilter("", _extension) };
            OpenFilePanelAsync(_title, _directory, extensions, _isMultiselect, _cb);
        }


        /// <summary>
        /// Native open file dialog async
        /// </summary>
        /// <param name="_title">Dialog title</param>
        /// <param name="_directory">Root directory</param>
        /// <param name="_extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param name="_isMultiselect">Allow multiple file selection</param>
        /// <param name="_cb">Callback")</param>
        public static void OpenFilePanelAsync(string _title, string _directory, ExtensionFilter[] _extensions, bool _isMultiselect, Action<string[]> _cb)
        {
            _platformWrapper.OpenFilePanelAsync(_title, _directory, _extensions, _isMultiselect, _cb);
        }


        /// <summary>
        /// Native open folder dialog
        /// NOTE: Multiple folder selection doesn't supported on Windows
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_directory">Root directory</param>
        /// <param name="_isMultiselect"></param>
        /// <returns>Returns array of chosen paths. Zero length array when cancelled</returns>
        public static string[] OpenFolderPanel(string _title, string _directory, bool _isMultiselect)
        {
            return _platformWrapper.OpenFolderPanel(_title, _directory, _isMultiselect);
        }


        /// <summary>
        /// Native open folder dialog async
        /// NOTE: Multiple folder selection doesn't supported on Windows
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_directory">Root directory</param>
        /// <param name="_isMultiselect"></param>
        /// <param name="_cb">Callback")</param>
        public static void OpenFolderPanelAsync(string _title, string _directory, bool _isMultiselect, Action<string[]> _cb)
        {
            _platformWrapper.OpenFolderPanelAsync(_title, _directory, _isMultiselect, _cb);
        }


        /// <summary>
        /// Native save file dialog
        /// </summary>
        /// <param name="_title">Dialog title</param>
        /// <param name="_directory">Root directory</param>
        /// <param name="_defaultName">Default file name</param>
        /// <param name="_extension">File extension</param>
        /// <returns>Returns chosen path. Empty string when cancelled</returns>
        public static string SaveFilePanel(string _title, string _directory, string _defaultName , string _extension)
        {
            var extensions = string.IsNullOrEmpty(_extension) ? null : new [] { new ExtensionFilter("", _extension) };
            return SaveFilePanel(_title, _directory, _defaultName, extensions);
        }


        /// <summary>
        /// Native save file dialog
        /// </summary>
        /// <param name="_title">Dialog title</param>
        /// <param name="_directory">Root directory</param>
        /// <param name="_defaultName">Default file name</param>
        /// <param name="_extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <returns>Returns chosen path. Empty string when cancelled</returns>
        public static string SaveFilePanel(string _title, string _directory, string _defaultName, ExtensionFilter[] _extensions)
        {
            return _platformWrapper.SaveFilePanel(_title, _directory, _defaultName, _extensions);
        }


        /// <summary>
        /// Native save file dialog async
        /// </summary>
        /// <param name="_title">Dialog title</param>
        /// <param name="_directory">Root directory</param>
        /// <param name="_defaultName">Default file name</param>
        /// <param name="_extension">File extension</param>
        /// <param name="_cb">Callback")</param>
        public static void SaveFilePanelAsync(string _title, string _directory, string _defaultName , string _extension, Action<string> _cb)
        {
            var extensions = string.IsNullOrEmpty(_extension) ? null : new [] { new ExtensionFilter("", _extension) };
            SaveFilePanelAsync(_title, _directory, _defaultName, extensions, _cb);
        }


        /// <summary>
        /// Native save file dialog async
        /// </summary>
        /// <param name="_title">Dialog title</param>
        /// <param name="_directory">Root directory</param>
        /// <param name="_defaultName">Default file name</param>
        /// <param name="_extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param name="_cb">Callback")</param>
        public static void SaveFilePanelAsync(string _title, string _directory, string _defaultName, ExtensionFilter[] _extensions, Action<string> _cb)
        {
            _platformWrapper.SaveFilePanelAsync(_title, _directory, _defaultName, _extensions, _cb);
        }
    }
}