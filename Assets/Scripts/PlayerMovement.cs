using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 15f;
    public float speedMultiplier = 1f;
    public Rigidbody2D rigidbody;
    private SpriteRenderer foxSprite;
    public Vector2 initialDirection;
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPosition { get; private set; }
    public LayerMask obstacleLayer;

    Vector2 movement;

    void Start()
    {
        foxSprite = GetComponent<SpriteRenderer>();
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rigidbody.isKinematic = false;
        enabled = true;
    }
    public bool Occupied(Vector2 direction)
    {
        // If no collider is hit then there is no obstacle in that direction
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        return hit.collider != null;
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

        rigidbody.MovePosition(position + translation);
    }

    void Update()
    {
        if (nextDirection != Vector2.zero) { SetDirection(nextDirection); }
        if (Input.GetKeyDown(KeyCode.W)) { SetDirection(Vector2.up); }
        else if (Input.GetKeyDown(KeyCode.S)) { SetDirection(Vector2.down); }
        else if (Input.GetKeyDown(KeyCode.A)) { SetDirection(Vector2.left); foxSprite.flipY = true; }
        else if (Input.GetKeyDown(KeyCode.D)) { SetDirection(Vector2.right); foxSprite.flipY = false; }

        // Rotate pacman to face the movement direction
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
}