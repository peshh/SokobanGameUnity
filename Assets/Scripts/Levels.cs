using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class Level
{
    public List<string> Rows = new List<string>();

    public int Height
    {
        get
        {
            return this.Rows.Count;
        }
    }

    public int Width
    {
        get
        {
            int longestRow = 0;

            foreach (var row in this.Rows)
            {
                if (row.Length > longestRow)
                {
                    longestRow = row.Length;
                }
            }

            return longestRow;
        }
    }
}

public class Levels : MonoBehaviour
{
    public string FileName;
    public List<Level> LevelsList;

    void Awake()
    {
        TextAsset textAsset = (TextAsset)Resources.Load(FileName);

        string text = textAsset.text;

        string[] levelsArr = text.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);

        foreach (var levelText in levelsArr)
        {
            string[] levelArr = levelText.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
            Level level = new Level();
            level.Rows = new List<string>(levelArr);
            LevelsList.Add(level);
        }
    }
}
