using UnityEngine;

public class Player : MonoBehaviour
{
    public bool Move(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) < 0.5)
        {
            direction.x = 0;
        }
        else if (Mathf.Abs(direction.y) < 0.5)
        {
            direction.y = 0;
        }

        direction.Normalize();

        if (this.IsBlocked(this.transform.position, direction))
        {
            return false;
        }

        this.transform.Translate(direction);
        return true;
    }

    private bool IsBlocked(Vector3 position, Vector2 direction)
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
                Box boxComponent = box.GetComponent<Box>();

                if (boxComponent != null && boxComponent.Move(direction))
                {
                    return false;
                }

                return true;
            }
        }

        return false;
    }
}