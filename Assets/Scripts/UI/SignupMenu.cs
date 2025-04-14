using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

[System.Serializable]
public class SignupResponse
{
    public string message;
}

[System.Serializable]
public class SignupError
{
    public string error;
}

public class AdminSignupRequest
{
    public string pseudo;
    public string mdp;
    public string token;
}

public class SignupRequest
{
    public string pseudo;
    public string mdp;
}

public class SignupMenu : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    private static string apiRoute = "/api/v1/utilisateur";
    private static string adminRoute = "/api/v1/admin";

    [SerializeField] private TMP_InputField usernameInupt;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField passwordConfirmInput;
    [SerializeField] private TextMeshProUGUI errorText;
    [SerializeField] private Toggle adminToggle;

    public void Update()
    {
        if(UserConnectionObject.Exists() && UserConnectionObject.Get().admin)
        {
            adminToggle.gameObject.SetActive(true);
        }
        else
        {
            adminToggle.gameObject.SetActive(false);
        }
    }

    public void Signup()
    {
        // Vérification si tous les champs ont été remplit
        if (usernameInupt.text.Length < 1 || passwordInput.text.Length < 1 || passwordConfirmInput.text.Length < 1)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Veuillez remplir tous les champs.";
            return;
        }

        // Vérification si les deux mdp sont pareil
        if (passwordInput.text != passwordConfirmInput.text)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Les mots de passe ne correspondent pas. Veuillez réessayer.";
            return;
        }

        // Si les vérification passent, on envoie la requête
        string body = "";
        string url = apiRoute;
        if (adminToggle.gameObject.activeInHierarchy && adminToggle.isOn)
        {
            // Si on veut créer un admin
            url = adminRoute;
            AdminSignupRequest request = new AdminSignupRequest();
            request.pseudo = usernameInupt.text;
            request.mdp = passwordInput.text;
            request.token = UserConnectionObject.Get().token;
            body = JsonUtility.ToJson(request);
        } 
        else
        {
            // Si on créer un simple utilisateur
            SignupRequest request = new SignupRequest();
            request.pseudo = usernameInupt.text;
            request.mdp = passwordInput.text;
            body = JsonUtility.ToJson(request);
        }

        ApiController.PostJsonToAPI(url, body, (data) =>
        {
            try
            {
                SignupResponse rep = JsonUtility.FromJson<SignupResponse>(data);

                // Si on a bien recu une réponse
                if (rep != null)
                {
                    errorText.gameObject.SetActive(false);
                    Debug.Log("Utilisateur Créer!");
                    mainMenu.OpenConnexionMenu();

                    // Reset des champs
                    usernameInupt.text = "";
                    passwordInput.text = "";
                    passwordConfirmInput.text = "";
                }
                else
                {
                    throw new ArgumentNullException();
                }
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
}
