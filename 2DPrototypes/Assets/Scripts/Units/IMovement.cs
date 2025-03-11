using System;
using UnityEngine;

public interface IMovement
{
    public void MoveTo(Vector2 position, Action onPositionReached = null);

    public bool IsIdle();
}
