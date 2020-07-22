#if UNITY_STANDALONE_WIN

using System;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Ookii.Dialogs;


namespace SFB
{
    // For fullscreen support
    // - WindowWrapper class and GetActiveWindow() are required for modal file dialog.
    // - "PlayerSettings/Visible In Background" should be enabled, otherwise when file dialog opened app window minimizes automatically.


    public class WindowWrapper : IWin32Window 
    {
        private IntPtr _hwnd;


        public WindowWrapper(IntPtr _handle) { _hwnd = _handle; }
        public IntPtr Handle { get { return _hwnd; } }
    }


    public class StandaloneFileBrowserWindows : IStandaloneFileBrowser
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();


        public string[] OpenFilePanel(string _title, string _directory, ExtensionFilter[] _extensions, bool _isMultiselect)
        {
            var fd = new VistaOpenFileDialog();
            fd.Title = _title;
            if (_extensions != null)
            {
                fd.Filter = GetFilterFromFileExtensionList(_extensions);
                fd.FilterIndex = 1;
            }
            else
            {
                fd.Filter = string.Empty;
            }
            fd.Multiselect = _isMultiselect;
            if (!string.IsNullOrEmpty(_directory))
            {
                fd.FileName = GetDirectoryPath(_directory);
            }
            var res = fd.ShowDialog(new WindowWrapper(GetActiveWindow()));
            var filenames = res == DialogResult.OK ? fd.FileNames : new string[0];
            fd.Dispose();
            return filenames;
        }


        public void OpenFilePanelAsync(string _title, string _directory, ExtensionFilter[] _extensions, bool _isMultiselect, Action<string[]> _cb)
        {
            _cb.Invoke(OpenFilePanel(_title, _directory, _extensions, _isMultiselect));
        }


        public string[] OpenFolderPanel(string _title, string _directory, bool _isMultiselect)
        {
            var fd = new VistaFolderBrowserDialog();
            fd.Description = _title;
            if (!string.IsNullOrEmpty(_directory))
            {
                fd.SelectedPath = GetDirectoryPath(_directory);
            }
            var res = fd.ShowDialog(new WindowWrapper(GetActiveWindow()));
            var filenames = res == DialogResult.OK ? new []{ fd.SelectedPath } : new string[0];
            fd.Dispose();
            return filenames;
        }


        public void OpenFolderPanelAsync(string _title, string _directory, bool _isMultiselect, Action<string[]> _cb)
        {
            _cb.Invoke(OpenFolderPanel(_title, _directory, _isMultiselect));
        }


        public string SaveFilePanel(string _title, string _directory, string _defaultName, ExtensionFilter[] _extensions)
        {
            var fd = new VistaSaveFileDialog();
            fd.Title = _title;

            var finalFilename = "";

            if (!string.IsNullOrEmpty(_directory))
            {
                finalFilename = GetDirectoryPath(_directory);
            }

            if (!string.IsNullOrEmpty(_defaultName))
            {
                finalFilename += _defaultName;
            }

            fd.FileName = finalFilename;
            if (_extensions != null)
            {
                fd.Filter = GetFilterFromFileExtensionList(_extensions);
                fd.FilterIndex = 1;
                fd.DefaultExt = _extensions[0].extensions[0];
                fd.AddExtension = true;
            }
            else
            {
                fd.DefaultExt = string.Empty;
                fd.Filter = string.Empty;
                fd.AddExtension = false;
            }
            var res = fd.ShowDialog(new WindowWrapper(GetActiveWindow()));
            var filename = res == DialogResult.OK ? fd.FileName : "";
            fd.Dispose();
            return filename;
        }


        public void SaveFilePanelAsync(string _title, string _directory, string _defaultName, ExtensionFilter[] _extensions, Action<string> _cb)
        {
            _cb.Invoke(SaveFilePanel(_title, _directory, _defaultName, _extensions));
        }


        // .NET Framework FileDialog Filter format
        // https://msdn.microsoft.com/en-us/library/microsoft.win32.filedialog.filter
        private static string GetFilterFromFileExtensionList(ExtensionFilter[] _extensions)
        {
            var filterString = "";
            foreach (var filter in _extensions)
            {
                filterString += filter.name + "(";

                foreach (var ext in filter.extensions)
                {
                    filterString += "*." + ext + ",";
                }

                filterString = filterString.Remove(filterString.Length - 1);
                filterString += ") |";

                foreach (var ext in filter.extensions)
                {
                    filterString += "*." + ext + "; ";
                }

                filterString += "|";
            }
            filterString = filterString.Remove(filterString.Length - 1);
            return filterString;
        }


        private static string GetDirectoryPath(string _directory)
        {
            var directoryPath = Path.GetFullPath(_directory);
            if (!directoryPath.EndsWith("\\"))
            {
                directoryPath += "\\";
            }
            if (Path.GetPathRoot(directoryPath) == directoryPath)
            {
                return _directory;
            }
            return Path.GetDirectoryName(directoryPath) + Path.DirectorySeparatorChar;
        }
    }
}

#endif