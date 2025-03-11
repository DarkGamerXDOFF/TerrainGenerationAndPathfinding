using UnityEngine;

public class MoveRbVelocity : MonoBehaviour, IMoveObject
{
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * moveSpeed * Time.fixedDeltaTime * 40;
    }
}
