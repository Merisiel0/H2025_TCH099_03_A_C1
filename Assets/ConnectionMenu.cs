using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[System.Serializable]
public class ConnectionResponse
{
    public bool connexion;
    public string utilisateur;
    public bool admin;
    public string token;

    public void Validate()
    {
        if(utilisateur == null || token == null)
        {
            throw new ArgumentNullException();
        }
    }
}

[System.Serializable]
public class ConnectionError
{
    public string error;
}

public class UserConnectionObject : MonoBehaviour
{
    private static UserConnectionObject instance = null;

    public ConnectionResponse data;

    public static void Disconnect()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
    }

    private void Awake()
    {
        // On remplace l'ancienne instance si elle existe encore. Permet une gestion simplifié du singleton.
        // => Dernier arrivé = le seul qui existe.
        if (instance != null) Destroy(instance.gameObject);
        instance = this;

        data = new ConnectionResponse();

        DontDestroyOnLoad(gameObject);
    }

    public UserConnectionObject Init(ConnectionResponse data)
    {
        this.data = data;
        return this;
    }

    public static ConnectionResponse Get()
    {
        if (Exists())
        {
            return instance.data;
        }
        else return null;
    }

    public static bool Exists() { return instance != null; }
}

public class ConnectionRequest
{
    public string pseudo;
    public string mdp;
}

public class ConnectionMenu : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    private static string apiRoute = "/api/v1/login";

    [SerializeField] private TMP_InputField usernameInupt;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TextMeshProUGUI errorText;

    public void Connect()
    {
        if(usernameInupt.text.Length < 1 || passwordInput.text.Length < 1)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Veuillez remplir tous les champs.";
            return;
        }

        ConnectionRequest request = new ConnectionRequest();
        request.pseudo = usernameInupt.text;
        request.mdp = passwordInput.text;

        string body = JsonUtility.ToJson(request);
        ApiController.PostJsonToAPI(apiRoute, body, (data) => {
            try
            {
                ConnectionResponse rep = JsonUtility.FromJson<ConnectionResponse>(data);
                rep.Validate();

                // Creation d'un objet qui encapsule la data du joeur
                UserConnectionObject dataContainer = new GameObject("UserConnectionContainer")
                    .AddComponent<UserConnectionObject>()
                    .Init(rep);
                
                errorText.gameObject.SetActive(false);
                Debug.Log("Utilisateur Connectée!");
                mainMenu.OpenMainMenu();

                usernameInupt.text = "";
                passwordInput.text = "";
            }
            catch (Exception e)
            {
                UserConnectionObject.Disconnect();
                ConnectionError errorObj = JsonUtility.FromJson<ConnectionError>(data);

                errorText.gameObject.SetActive(true);
                errorText.text = (errorObj != null) ? errorObj.error : "Erreur de connection.";
            }
        });
    }

    public void Disconnect()
    {
        UserConnectionObject.Disconnect();
        mainMenu.OpenConnexionMenu();
    }
}
