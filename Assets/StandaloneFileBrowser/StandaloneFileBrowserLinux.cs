#if UNITY_STANDALONE_LINUX

using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;


namespace SFB
{
    public class StandaloneFileBrowserLinux : IStandaloneFileBrowser
    {
        private static Action<string[]> _openFileCb;
        private static Action<string[]> _openFolderCb;
        private static Action<string> _saveFileCb;

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void AsyncCallback(string _path);


        [DllImport("StandaloneFileBrowser")]
        private static extern void DialogInit();
        [DllImport("StandaloneFileBrowser")]
        private static extern IntPtr DialogOpenFilePanel(string _title, string _directory, string _extension, bool _isMultiselect);
        [DllImport("StandaloneFileBrowser")]
        private static extern void DialogOpenFilePanelAsync(string _title, string _directory, string _extension, bool _isMultiselect, AsyncCallback _callback);
        [DllImport("StandaloneFileBrowser")]
        private static extern IntPtr DialogOpenFolderPanel(string _title, string _directory, bool _isMultiselect);
        [DllImport("StandaloneFileBrowser")]
        private static extern void DialogOpenFolderPanelAsync(string _title, string _directory, bool _isMultiselect, AsyncCallback _callback);
        [DllImport("StandaloneFileBrowser")]
        private static extern IntPtr DialogSaveFilePanel(string _title, string _directory, string _defaultName, string _extension);
        [DllImport("StandaloneFileBrowser")]
        private static extern void DialogSaveFilePanelAsync(string _title, string _directory, string _defaultName, string _extension, AsyncCallback _callback);


        public StandaloneFileBrowserLinux()
        {
            DialogInit();
        }


        public string[] OpenFilePanel(string _title, string _directory, ExtensionFilter[] _extensions, bool _isMultiselect)
        {
            var paths = Marshal.PtrToStringAnsi(DialogOpenFilePanel(
                _title,
                _directory,
                GetFilterFromFileExtensionList(_extensions),
                _isMultiselect));
            return paths.Split((char)28);
        }


        public void OpenFilePanelAsync(string _title, string _directory, ExtensionFilter[] _extensions, bool _isMultiselect, Action<string[]> _cb)
        {
            _openFileCb = _cb;
            DialogOpenFilePanelAsync(
                _title,
                _directory,
                GetFilterFromFileExtensionList(_extensions),
                _isMultiselect,
                (string result) => { _openFileCb.Invoke(result.Split((char)28)); });
        }


        public string[] OpenFolderPanel(string _title, string _directory, bool _isMultiselect)
        {
            var paths = Marshal.PtrToStringAnsi(DialogOpenFolderPanel(
                _title,
                _directory,
                _isMultiselect));
            return paths.Split((char)28);
        }


        public void OpenFolderPanelAsync(string _title, string _directory, bool _isMultiselect, Action<string[]> _cb)
        {
            _openFolderCb = _cb;
            DialogOpenFolderPanelAsync(
                _title,
                _directory,
                _isMultiselect,
                (string result) => { _openFolderCb.Invoke(result.Split((char)28)); });
        }


        public string SaveFilePanel(string _title, string _directory, string _defaultName, ExtensionFilter[] _extensions)
        {
            return Marshal.PtrToStringAnsi(DialogSaveFilePanel(
                _title,
                _directory,
                _defaultName,
                GetFilterFromFileExtensionList(_extensions)));
        }


        public void SaveFilePanelAsync(string _title, string _directory, string _defaultName, ExtensionFilter[] _extensions, Action<string> _cb)
        {
            _saveFileCb = _cb;
            DialogSaveFilePanelAsync(
                _title,
                _directory,
                _defaultName,
                GetFilterFromFileExtensionList(_extensions),
                (string result) => { _saveFileCb.Invoke(result); });
        }


        private static string GetFilterFromFileExtensionList(ExtensionFilter[] _extensions)
        {
            if (_extensions == null)
            {
                return "";
            }

            var filterString = "";
            foreach (var filter in _extensions)
            {
                filterString += filter.Name + ";";

                foreach (var ext in filter.Extensions)
                {
                    filterString += ext + ",";
                }

                filterString = filterString.Remove(filterString.Length - 1);
                filterString += "|";
            }
            filterString = filterString.Remove(filterString.Length - 1);
            return filterString;
        }
    }
}
#endif