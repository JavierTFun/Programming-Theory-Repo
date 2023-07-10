using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    // ABSTRACTION

    [SerializeField] InputField inputName;
    [SerializeField] TextMeshProUGUI scoreText;
    public string playerName;

    private void Start()
    {
        MainManager.Instance.LoadData();
        if (MainManager.Instance != null)
        {
            scoreText.text = "Best Score: " + MainManager.Instance.bestScoreText + " : " + MainManager.Instance.bestScore;
            inputName.text = MainManager.Instance.lastName;
        }
    }


    public void StartNew()
    {
        SavePlayerName();
        SceneManager.LoadScene(1);
        Counter.RestartGlobalCount();
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }


    public void SavePlayerName()
    {
        MainManager.Instance.lastName = inputName.text.ToString();
        MainManager.Instance.SaveData(0);
    }

    public void ResetScoreHistory()
    {
        MainManager.Instance.DeleteData();
    }
}