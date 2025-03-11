using UnityEngine;
using System;

public class UnitRTS : MonoBehaviour
{
    [SerializeField] private GameObject selectedGameObject;
    
    private IMovement movement;
    private IUnitGFX unitGFX;

    private void Awake()
    {
        movement = GetComponent<IMovement>();
        unitGFX = GetComponentInChildren<IUnitGFX>();
    }

    public bool IsIdle() => movement.IsIdle();

    public void SetSelectedVisible(bool visible) => selectedGameObject.SetActive(visible);

    public void MoveTo(Vector2 targetPosition, Action onPositionReached = null)
    {
        movement.MoveTo(targetPosition, onPositionReached);
    }

    public void PlayHarvestAnimation(Action onAnimationCompleted)
    {
        unitGFX.PlaySpecialAnimation(() => onAnimationCompleted?.Invoke());
    }
}
