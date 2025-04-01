using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminMenu : MonoBehaviour
{
    [SerializeField] private GameObject btnPrefab;
    [SerializeField] private int btnGap = 30;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject hintPanel;
    [SerializeField] private GameObject hintText;

    void Start()
    {
        panel.SetActive(false);
        hintPanel.SetActive(false);
        hintText.SetActive(false);

        if (!UserConnectionObject.Exists())
            return;

        hintPanel.SetActive(true);
        hintText.SetActive(true);

        MissionEventStatus[] statuses = MissionEventManager.instance.GetLaunchableEvents();
        float btnHeight = btnPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < statuses.Length; i++)
        {
            GameObject btn = Instantiate(btnPrefab, panel.transform);
            float x = i * btnHeight + (i + 1) * btnGap;
            btn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -x);

            string text = "Toggle ";
            switch (statuses[i].trigger)
            {
                case MissionEvent.LightsOut:
                    text += "lights";
                    break;
                case MissionEvent.ThrustersShutdown:
                    text += "thrusters";
                    break;
                case MissionEvent.ElectricFailure:
                    text += "electrics";
                    break;
                case MissionEvent.PatPlayTrigger:
                    text += "patplay";
                    break;
            }
            text += " event";

            btn.GetComponentInChildren<TMP_Text>().text = text;

            MissionEventStatus status = statuses[i];
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                MissionEventManager.SendEvent(status.isActive ? status.solver : status.trigger);
            });
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && UserConnectionObject.Exists())
        {
            panel.SetActive(!panel.activeInHierarchy);
            hintPanel.SetActive(!panel.activeInHierarchy);
        }
    }
}
