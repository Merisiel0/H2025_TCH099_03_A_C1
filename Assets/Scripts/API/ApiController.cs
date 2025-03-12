using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

[CustomEditor(typeof(ApiController))]
public class ApiControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Label("Api configuration: ");
        ApiController.baseUrl = EditorGUILayout.TextField("Base Url (ex: http://localhost:8080)", ApiController.baseUrl);
        DrawDefaultInspector();
    }
}

public class ApiController : MonoBehaviour
{
    public static string baseUrl = "http://127.0.0.1:5000/";
                                     
    private static ApiController instance;

    public void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public static void FetchDataFromAPI(string url, Action<string> callback)
    {
        instance.StartCoroutine(AsyncFetchDataFromAPI(url, callback));
    }

    public static void FetchDataFromAPI<T>(string url, Action<T> callback)
    {
        instance.StartCoroutine(AsyncFetchDataFromAPI(url, (json) =>
        {
            T response = JsonUtility.FromJson<T>(json);
            callback.Invoke(response);
        }));
    }

    private static IEnumerator AsyncFetchDataFromAPI(string url, Action<string> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(baseUrl + url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erreur API : " + request.error);
        }
        else
        {
            string json = request.downloadHandler.text;
            callback.Invoke(json);
        }
    }
}
