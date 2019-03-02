using UnityEngine;

public class Box : MonoBehaviour
{
    private void Start()
    {
        this.TestForInSlot();
    }

    public bool IsInSlot;

    public bool Move(Vector2 direction)
    {
        if (this.IsBlocked(transform.position, direction))
        {
            return false;
        }

        this.transform.Translate(direction);
        this.TestForInSlot();
        return true;
    }

    public void TestForInSlot()
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("Slot");

        foreach (var slot in slots)
        {
            if (this.transform.position.x == slot.transform.position.x && this.transform.position.y == slot.transform.position.y)
            {
                GetComponent<SpriteRenderer>().color = Color.blue;
                this.IsInSlot = true;
                return;
            }
        }
        GetComponent<SpriteRenderer>().color = Color.gray;
        this.IsInSlot = false;
    }

    public bool IsBlocked(Vector3 position, Vector2 direction)
    {
        Vector2 newPos = new Vector2(position.x, position.y) + direction;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (var wall in walls)
        {
            if (wall.transform.position.x == newPos.x && wall.transform.position.y == newPos.y)
            {
                return true;
            }
        }

        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");

        foreach (var box in boxes)
        {
            if (box.transform.position.x == newPos.x && box.transform.position.y == newPos.y)
            {
                return true;
            }
        }

        return false;
    }
}