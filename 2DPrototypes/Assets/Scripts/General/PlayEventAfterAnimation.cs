using System;
using UnityEngine;

public class PlayEventAfterAnimation : MonoBehaviour
{
    public Action onAnimationCompleted;

    public void TriggerEvent() => onAnimationCompleted?.Invoke();
}
