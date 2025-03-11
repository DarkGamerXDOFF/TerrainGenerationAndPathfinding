using System;
using UnityEngine;

public class MoveToPosition : MonoBehaviour, IMovement
{

    [SerializeField] private float stopDistance = 0.5f;

    private Vector2 targetPosition;
    private Action onPositionReached;

    private bool move;

    private IMoveObject moveObject;

    private void Awake()
    {
        moveObject = GetComponent<IMoveObject>();
    }

    public void MoveTo(Vector2 position, Action onPositionReached = null)
    {
        targetPosition = position;
        this.onPositionReached = onPositionReached;
        move = true;
    }

    private void Update()
    {
        if (move)
        {
            Vector2 position = transform.position;
            float distance = Vector2.Distance(position, targetPosition);
            
            Vector2 direction = Vector2.zero;
            
            if (distance > stopDistance)
            {
                direction = targetPosition - position;
            }
            else
            {
                move = false;
                onPositionReached?.Invoke();
            }

            moveObject.SetDirection(direction);
        }
    }

    public bool IsIdle()
    {
        return !move;
    }
}
