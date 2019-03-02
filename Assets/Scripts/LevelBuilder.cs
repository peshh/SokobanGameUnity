using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelElement
{
    public char Character;
    public GameObject Prefab;
}

public class LevelBuilder : MonoBehaviour
{
    public int CurrentLevel;
    public List<LevelElement> LevelElements;
    private Level level;

    private GameObject GetPrefab(char c)
    {
        LevelElement levelElement = this.LevelElements.Find(l => l.Character == c);

        if (levelElement != null)
        {
            return levelElement.Prefab;
        }

        return null;
    }

    public void NextLevel()
    {
        this.CurrentLevel++;

        if (this.CurrentLevel == GetComponent<Levels>().LevelsList.Count)
        {
            this.CurrentLevel = 0;
        }
    }

    public void Build()
    {
        this.level = GetComponent<Levels>().LevelsList[CurrentLevel];

        int startX = -this.level.Width / 2;
        int x = startX;
        int y = this.level.Height / 2;
        foreach (var row in this.level.Rows)
        {
            foreach (var ch in row)
            {
                GameObject prefab = this.GetPrefab(ch);

                if (prefab != null)
                {
                    if (ch == '+' || ch == '*')
                    {
                        GameObject prefabSlot = this.GetPrefab('.');
                        Instantiate(prefabSlot, new Vector3(x, y, 0), Quaternion.identity);
                    }
                    Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
                }

                x++;
            }

            y--;
            x = startX;
        }
    }
}