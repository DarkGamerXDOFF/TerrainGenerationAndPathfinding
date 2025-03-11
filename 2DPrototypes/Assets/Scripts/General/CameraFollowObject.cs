using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public static CameraFollowObject I;

    [SerializeField] private float moveSpeed = 10;

    private Vector2 movement;

    private Vector2 minBounds;
    private Vector2 maxBounds;
    [SerializeField] private Vector2 margins;

    [SerializeField] private bool viewBounds = false;

    [Header("Gizmos")]
    [SerializeField] private float radius = 0.1f;
    [SerializeField] private Color color;

    private void Awake()
    {
        if (I == null)
            I = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        SetBounds(GridManager.I.grid);
    }

    private void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed * Time.deltaTime;

        transform.Translate(movement);

        if (transform.position.x < minBounds.x)
            transform.position = new Vector2(minBounds.x, transform.position.y);
        if (transform.position.y < minBounds.y)
            transform.position = new Vector2(transform.position.x, minBounds.y);
        if (transform.position.x > maxBounds.x)
            transform.position = new Vector2(maxBounds.x, transform.position.y);
        if (transform.position.y > maxBounds.y)
            transform.position = new Vector2(transform.position.x, maxBounds.y);
    }

    public void SetBounds(NodeGrid grid)
    {
        int width = grid.width;
        int height = grid.height;

        maxBounds = new Vector2(width + margins.x, height + margins.y);
        minBounds = new Vector2(-margins.x, -margins.y);
    }

    private void OnDrawGizmos()
    {
        if (viewBounds)
        {
            Vector2 topLeft = new Vector2(minBounds.x, maxBounds.y);
            Vector2 bottomRight = new Vector2(maxBounds.x, minBounds.y);
            
            Gizmos.color = color;

            Gizmos.DrawSphere(minBounds, radius);
            Gizmos.DrawSphere(maxBounds, radius);
            Gizmos.DrawSphere(topLeft, radius);
            Gizmos.DrawSphere(bottomRight, radius);
            Gizmos.DrawLine(minBounds, topLeft);
            Gizmos.DrawLine(topLeft, maxBounds);
            Gizmos.DrawLine(maxBounds, bottomRight);
            Gizmos.DrawLine(bottomRight, minBounds);
        }
    }
}
