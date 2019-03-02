using System.Collections;
using System.IO;
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

    private void Start()
    {
        this.ResetScene();
        this.AddLevelInput.SetActive(false);
        this.AddButton.SetActive(false);
    }

    private void Update()
    {
        Vector2 inputMove = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputMove.Normalize();

        if (inputMove.sqrMagnitude > 0.5)
        {
            if (this.readyForInput)
            {
                this.readyForInput = false;
                this.player.Move(inputMove);
            }
        }
        else
        {
            this.readyForInput = true;
        }

        if (this.IsLevelComplete())
        {
            if (this.IsLevelLoaded)
            {
                this.NextLevel();
            }
        }
    }

    private bool IsLevelComplete()
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
        this.LevelBuilder.NextLevel();
        StartCoroutine(this.ResetSceneASync());
    }

    public void ResetScene()
    {
        StartCoroutine(this.ResetSceneASync());
    }

    private IEnumerator ResetSceneASync()
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
        this.LevelBuilder.Build();
        this.player = FindObjectOfType<Player>();
        this.IsLevelLoaded = true;
    }
}