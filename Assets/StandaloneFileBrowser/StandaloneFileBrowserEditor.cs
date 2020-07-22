#if UNITY_EDITOR

using System;
using UnityEditor;


namespace SFB
{
    public class StandaloneFileBrowserEditor : IStandaloneFileBrowser
    {
        public string[] OpenFilePanel(string _title, string _directory, ExtensionFilter[] _extensions, bool _isMultiselect)
        {
            string path = "";

            if (_extensions == null)
            {
                path = EditorUtility.OpenFilePanel(_title, _directory, "");
            }
            else
            {
                path = EditorUtility.OpenFilePanelWithFilters(_title, _directory, GetFilterFromFileExtensionList(_extensions));
            }

            return string.IsNullOrEmpty(path) ? new string[0] : new[] { path };
        }


        public void OpenFilePanelAsync(string _title, string _directory, ExtensionFilter[] _extensions, bool _isMultiselect, Action<string[]> _cb)
        {
            _cb.Invoke(OpenFilePanel(_title, _directory, _extensions, _isMultiselect));
        }


        public string[] OpenFolderPanel(string _title, string _directory, bool _isMultiselect)
        {
            var path = EditorUtility.OpenFolderPanel(_title, _directory, "");
            return string.IsNullOrEmpty(path) ? new string[0] : new[] {path};
        }


        public void OpenFolderPanelAsync(string _title, string _directory, bool _isMultiselect, Action<string[]> _cb)
        {
            _cb.Invoke(OpenFolderPanel(_title, _directory, _isMultiselect));
        }


        public string SaveFilePanel(string _title, string _directory, string _defaultName, ExtensionFilter[] _extensions)
        {
            var ext = _extensions != null ? _extensions[0].extensions[0] : "";
            var name = string.IsNullOrEmpty(ext) ? _defaultName : _defaultName + "." + ext;
            return EditorUtility.SaveFilePanel(_title, _directory, name, ext);
        }


        public void SaveFilePanelAsync(string _title, string _directory, string _defaultName, ExtensionFilter[] _extensions, Action<string> _cb)
        {
            _cb.Invoke(SaveFilePanel(_title, _directory, _defaultName, _extensions));
        }


        // EditorUtility.OpenFilePanelWithFilters extension filter format
        private static string[] GetFilterFromFileExtensionList(ExtensionFilter[] _extensions)
        {
            var filters = new string[_extensions.Length * 2];
            for (int i = 0; i < _extensions.Length; i++)
            {
                filters[(i * 2)] = _extensions[i].name;
                filters[(i * 2) + 1] = string.Join(",", _extensions[i].extensions);
            }
            return filters;
        }
    }
}
#endif
