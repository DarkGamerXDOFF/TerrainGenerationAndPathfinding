using System;
using UnityEngine;

public class UnitGFX : MonoBehaviour, IUnitGFX
{
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Action onAnimationFinished;

    [SerializeField] private string specialAnimation;

    bool flip = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.linearVelocityX > 0)
            flip = false;
        else if (rb.linearVelocityX < 0)
            flip = true;
        
        anim.SetBool("IsIdle", IsIdle());

        sr.flipX = flip;
    }

    public void PlaySpecialAnimation(Action onAnimationFinished = null)
    {
        if (!string.IsNullOrEmpty(specialAnimation))
            anim.Play(specialAnimation);

        this.onAnimationFinished = onAnimationFinished;
    }

    public void TriggerAction() => onAnimationFinished?.Invoke();

    public bool IsIdle()
    {
        return !(rb.linearVelocity.magnitude > 0);
    }
}


interface IUnitGFX
{
    public bool IsIdle();
    public void PlaySpecialAnimation(Action onAnimationFinished = null);
}