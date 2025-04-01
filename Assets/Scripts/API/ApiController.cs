using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using System.Text;

/// <summary>
/// Une classe qui permet de modifier les valeurs statiques (et donc communes) entre les instances de ApiController, 
/// Cela permet d'avoir un seul host pour notre API et de ne pas avoir d'oublie si des modification ont lieu.
/// </summary>
[CustomEditor(typeof(ApiController))]
public class ApiControllerEditor : Editor
{
    /// <summary>
    // Affichage d'un champ de texte dans l'�diteur pour modifier l'url de base (ex: 'http://127.0.0.1:5000')
    /// </summary>
    public override void OnInspectorGUI()
    {
        GUILayout.Label("Api configuration: ");
        ApiController.baseUrl = EditorGUILayout.TextField("Base Url (ex: http://localhost:8080)", ApiController.baseUrl);
        DrawDefaultInspector();
    }
}

/// <summary>
/// Une classe qui encapsule les requ�tes � l'api et permet de r�cuperer un r�sultat sous forme de JSON ou d'objet C# (� l'aide de la s�rializaiton)
/// </summary>
public class ApiController : MonoBehaviour
{
    /// <summary>
    /// L'url de base auquel on ajoutera les routes sp�cifi� pour l'api
    /// </summary>
    public static string baseUrl = "http://localhost:5000/";
                                     
    private static ApiController instance; // L'instance statique de notre singleton

    public void Awake()
    {
        // Initialisation du singleton
        if (instance == null) instance = this;
        else Destroy(this);
    }

    /// <summary>
    /// Fonction qui permet d'obtenir le r�sultat d'une requ�te � l'api au format JSON. Envoie un string null au callback en cas d'erreur.
    /// </summary>
    /// <param name="url">L'url � ajouter � l'url de base, la route � contacter</param>
    /// <param name="callback">Le callback ou la fonction � executer de mani�re asynchrone apr�s la r�ponse de l'api.</param>
    public static void FetchDataFromAPI(string url, Action<string> callback)
    {
        instance.StartCoroutine(AsyncFetchDataFromAPI(url, callback));
    }

    /// <summary>
    /// Fonction qui permet d'obtenir le r�sultat d'une requ�te � l'api sous forme d'un objet de type donn� (template). Envoie un objet default (valeur par d�faut) au callback en cas d'erreur.
    /// </summary>
    /// <param name="url">L'url � ajouter � l'url de base, la route � contacter</param>
    /// <param name="callback">Le callback ou la fonction � executer de mani�re asynchrone apr�s la r�ponse de l'api.</param>
    public static void FetchDataFromAPI<T>(string url, Action<T> callback)
    {
        instance.StartCoroutine(AsyncFetchDataFromAPI(url, (json) =>
        {
            Debug.Log(json);
            try
            {
                T response = JsonUtility.FromJson<T>(json);
                callback.Invoke(response);
            }
            catch(Exception e)
            {
                Debug.LogWarning(e);
                callback.Invoke(default);
            }
        }));
    }

    /// <summary>
    /// Fonction qui s'occupe de l'envoie de la requ�te web, v�rifie si une erreur � lieu et apelle le callback.
    /// </summary>
    /// <param name="url">L'url � ajouter � l'url de base, la route � contacter</param>
    /// <param name="callback">Le callback ou la fonction � executer de mani�re asynchrone apr�s la r�ponse de l'api.</param>
    /// <returns></returns>
    private static IEnumerator AsyncFetchDataFromAPI(string url, Action<string> callback)
    {
        //Debug.Log("Full request : " + baseUrl + url);
        UnityWebRequest request = UnityWebRequest.Get(baseUrl + url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erreur API : " + request.error);
            callback.Invoke(null);
        }
        else
        {
            string json = request.downloadHandler.text;
            callback.Invoke(json);
        }
    }

    public static void PostJsonToAPI(string url, string body, Action<string> callback)
    {
        instance.StartCoroutine(AsyncPostDataToAPI(url, body, callback));
    }

    private static IEnumerator AsyncPostDataToAPI(string url, string body, Action<string> callback)
    {
        UnityWebRequest request = new UnityWebRequest(baseUrl + url, "POST");

        byte[] jsonToSend = Encoding.UTF8.GetBytes(body);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        string json = request.downloadHandler.text;
        callback.Invoke(json);
    }
}
