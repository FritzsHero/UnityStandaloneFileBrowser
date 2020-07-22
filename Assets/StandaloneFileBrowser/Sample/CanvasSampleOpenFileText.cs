using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SFB;


[RequireComponent(typeof(Button))]
public class CanvasSampleOpenFileText : MonoBehaviour, IPointerDownHandler
{
    public Text output;


#if UNITY_WEBGL && !UNITY_EDITOR
    // WebGL


    [DllImport("__Internal")]
    private static extern void UploadFile(string _gameObjectName, string _methodName, string _filter, bool isMultiple);


    public void OnPointerDown(PointerEventData _eventData)
    {
        UploadFile(gameObject.name, "OnFileUpload", ".txt", false);
    }


    // Called from browser
    public void OnFileUpload(string _url)
    {
        StartCoroutine(OutputRoutine(_url));
    }
#else
    // Standalone platforms & editor


    public void OnPointerDown(PointerEventData _eventData) { }


    void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }


    private void OnClick()
    {
        var paths = StandaloneFileBrowser.OpenFilePanel("Title", "", "txt", false);
        if (paths.Length > 0)
        {
            StartCoroutine(OutputRoutine(new System.Uri(paths[0]).AbsoluteUri));
        }
    }
#endif


    private IEnumerator OutputRoutine(string _url)
    {
        var loader = new WWW(_url);
        yield return loader;
        output.text = loader.text;
    }
}