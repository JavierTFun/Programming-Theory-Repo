using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;


    public int bestScore;
    public string bestScoreText;
    public string lastName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }

    [Serializable]
    class SaveDataUser
    {
        public int bestScore;
        public string bestScoreText;
        public string lastName;
    }

    public void SaveData(int globalCount)
    {
        SaveDataUser data = new SaveDataUser();
        if (globalCount > bestScore)
        {
            data.bestScore = globalCount;
            data.bestScoreText = lastName;
        }
        else
        {
            data.bestScore = bestScore;
            data.bestScoreText = bestScoreText;
        }

        data.lastName = lastName;


        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saveData.json", json);

    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/saveData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataUser data = JsonUtility.FromJson<SaveDataUser>(json);

            bestScore = data.bestScore;
            bestScoreText = data.bestScoreText;
            lastName = data.lastName;

        }
    }

    public void DeleteData()
    {
        string path = Application.persistentDataPath + "/saveData.json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        Debug.Log("Datos JSON eliminados");

    }

}