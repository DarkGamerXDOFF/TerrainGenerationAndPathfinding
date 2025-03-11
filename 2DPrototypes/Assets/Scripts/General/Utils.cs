using UnityEngine;

public static class Utils 
{
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPosition(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static bool IsValidGridPosition(Vector2 pos, NodeGrid grid)
    {
        return pos.x >= 0 && pos.x < grid.width && pos.y >= 0 && pos.y < grid.height;
    }

    public static Vector2 WorldToGrid(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.y);
        return new Vector2(x, y);
    }
}
