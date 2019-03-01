using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LevelBuilder LevelBuilder;
    public GameObject AddLevelButton;
    public GameObject AddLevelInput;
    public GameObject AddButton;
    private bool readyForInput;
    private Player player;
    private bool IsLevelLoaded;

    void Start()
    {
        this.ResetScene();
        this.AddLevelInput.SetActive(false);
        this.AddButton.SetActive(false);
    }

    void Update()
    {
        Vector2 inputMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputMove.Normalize();

        if (inputMove.sqrMagnitude > 0.5)
        {
            if (readyForInput)
            {
                readyForInput = false;
                player.Move(inputMove);
            }
        }
        else
        {
            readyForInput = true;
        }

        if (this.IsLevelComplete())
        {
            if (this.IsLevelLoaded)
            {
                this.NextLevel();
            }
        }
    }

    bool IsLevelComplete()
    {
        Box[] boxes = GameObject.FindObjectsOfType<Box>();

        foreach (var box in boxes)
        {
            if (box.IsInSlot == false)
            {
                return false;
            }
        }

        return true;
    }

    public void AddNewLevel()
    {
        string newLevel = this.AddLevelInput.GetComponent<InputField>().text;
        string path = "Assets/Resources/Levels.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(';');
        writer.WriteLine(newLevel);
        writer.Close();
        //this.LevelBuilder.Build();
        this.AddLevelInput.SetActive(false);
        this.AddButton.SetActive(false);
    }

    public void OpenNewLevelInputBox()
    {
        this.AddLevelInput.SetActive(true);
        this.AddButton.SetActive(true);
    }

    public void NextLevel()
    {
        LevelBuilder.NextLevel();
        StartCoroutine(ResetSceneASync());
    }

    public void ResetScene()
    {
        StartCoroutine(ResetSceneASync());
    }

    IEnumerator ResetSceneASync()
    {
        this.IsLevelLoaded = false;

        if (SceneManager.sceneCount > 1)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("LevelScene");

            while (asyncUnload.isDone == false)
            {
                yield return null;
            }

            Resources.UnloadUnusedAssets();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelScene", LoadSceneMode.Additive);

        while (asyncLoad.isDone == false)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelScene"));
        LevelBuilder.Build();
        player = FindObjectOfType<Player>();
        this.IsLevelLoaded = true;
    }
}
