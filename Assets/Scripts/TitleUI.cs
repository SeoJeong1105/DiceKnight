using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public GameObject createPanel;
    public Text nameText;
    public Button createButton;

    public void OnClickStartPanel()
    {
        if (!File.Exists(DataManager.instance.path))
            CreateData();
        else
            StartGame();
    }

    public void OnClickCreateButton()
    {
        if (nameText.text == "") return;

        DataManager.instance.playerData.name = nameText.text;
        DataManager.instance.SaveData();
        StartGame();
    }

    private void CreateData()
    {
        createPanel.gameObject.SetActive(true);
    }

    private void StartGame()
    {
        DataManager.instance.LoadData();
        SceneManager.LoadScene(1);
    }
}
