using System;


namespace SFB
{
    public interface IStandaloneFileBrowser
    {
        string[] OpenFilePanel(string _title, string _directory, ExtensionFilter[] _extensions, bool _isMultiselect);
        string[] OpenFolderPanel(string _title, string _directory, bool _isMultiselect);
        string SaveFilePanel(string _title, string _directory, string _defaultName, ExtensionFilter[] _extensions);

        void OpenFilePanelAsync(string _title, string _directory, ExtensionFilter[] _extensions, bool _isMultiselect, Action<string[]> _cb);
        void OpenFolderPanelAsync(string _title, string _directory, bool _isMultiselect, Action<string[]> _cb);
        void SaveFilePanelAsync(string _title, string _directory, string _defaultName, ExtensionFilter[] _extensions, Action<string> _cb);
    }
}
