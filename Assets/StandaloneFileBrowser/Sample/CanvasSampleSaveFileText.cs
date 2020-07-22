using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace SFB.Samples
{
    [RequireComponent(typeof(Button))]
    public class CanvasSampleSaveFileText : MonoBehaviour, IPointerDownHandler
    {
        public Text output;

        // Sample text data
        private string data = "Example text created by StandaloneFileBrowser";


#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL


        [DllImport("__Internal")]
        private static extern void DownloadFile(string _gameObjectName, string _methodName, string _fileName, byte[] _byteArray, int _byteArraySize);


        // Broser plugin should be called in OnPointerDown.
        public void OnPointerDown(PointerEventData _eventData)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            DownloadFile(gameObject.name, "OnFileDownload", "sample.txt", bytes, bytes.Length);
        }


        // Called from browser
        public void OnFileDownload()
        {
            output.text = "File Successfully Downloaded";
        }
#else
        // Standalone platforms & editor
        public void OnPointerDown(PointerEventData _eventData) { }


        // Listen OnClick event in standlone builds
        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }


        public void OnClick()
        {
            var path = StandaloneFileBrowser.SaveFilePanel("Title", "", "sample", "txt");
            if (!string.IsNullOrEmpty(path))
            {
                File.WriteAllText(path, data);
            }
        }
#endif
    }
}