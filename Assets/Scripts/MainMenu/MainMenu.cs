using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public GameObject levelButtonHolder;

    public void LoadLevelsData()
    {
        foreach(Transform child in levelButtonHolder.transform)
        {
            Destroy(child.gameObject);
        }

        ApiController.FetchDataFromAPI("api/v1/niveaux", (response) =>
        {
            LevelDataWrapper data = JsonUtility.FromJson<LevelDataWrapper>("{\"levels\":" + response + "}");
            foreach (LevelData level in data.levels)
            {
                LevelOptionButton levelOption = Instantiate(levelButtonPrefab).GetComponent<LevelOptionButton>();
                levelOption.transform.SetParent(levelButtonHolder.transform);
                levelOption.Init(level);
            }
        });
    }

    public void Quit()
    {
        Application.Quit();
    }
}
