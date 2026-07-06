using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float BulletSpeed = 20f;
    Rigidbody2D myRigidbody;
    PlayerMovements player;
    float xSpeed;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

        player = FindAnyObjectByType<PlayerMovements>();

        xSpeed = player.transform.localScale.x * BulletSpeed;
    }

    void Update()
    {
        myRigidbody.linearVelocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0.5f);
    }

}
