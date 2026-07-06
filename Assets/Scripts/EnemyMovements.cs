using UnityEngine;

public class EnemyMovements : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D enemyRb;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        enemyRb.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing(); 
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRb.linearVelocity.x)),1f);
    }
}
