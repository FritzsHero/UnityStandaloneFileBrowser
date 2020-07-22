using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace SFB.Samples
{
    [RequireComponent(typeof(Button))]
    public class CanvasSampleOpenFileTextMultiple : MonoBehaviour, IPointerDownHandler
    {
        public Text output;


#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL


        [DllImport("__Internal")]
        private static extern void UploadFile(string _gameObjectName, string _methodName, string _filter, bool _isMultiple);


        public void OnPointerDown(PointerEventData _eventData)
        {
            UploadFile(gameObject.name, "OnFileUpload", ".txt", true);
        }


        // Called from browser
        public void OnFileUpload(string _urls)
        {
            StartCoroutine(OutputRoutine(_urls.Split(',')));
        }
#else
        // Standalone platforms & editor


        public void OnPointerDown(PointerEventData _eventData) { }


        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }


        private void OnClick()
        {
            // var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "txt", true);
            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", true);
            if (paths.Length > 0)
            {
                var urlArr = new List<string>(paths.Length);
                for (int i = 0; i < paths.Length; i++)
                {
                    urlArr.Add(new System.Uri(paths[i]).AbsoluteUri);
                }
                StartCoroutine(OutputRoutine(urlArr.ToArray()));
            }
        }
#endif


        private IEnumerator OutputRoutine(string[] _urls)
        {
            var outputText = "";
            for (int i = 0; i < _urls.Length; i++)
            {
                var loader = new WWW(_urls[i]);
                yield return loader;
                outputText += loader.text;
            }
            output.text = outputText;
        }
    }
}