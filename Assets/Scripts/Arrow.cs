using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    private Vector3 _screenBounds;

    void Start()
    {
        _screenBounds = Game.screenBounds;
        _screenBounds.x += transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        _screenBounds.y += transform.GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    void Update()
    {
        if (!Game.pause)
        {
            Vector3 pos = transform.position;
            if (-_screenBounds.x < pos.x && pos.x < _screenBounds.x && -_screenBounds.y < pos.y && pos.y < _screenBounds.y)
            {
                transform.Translate(Vector2.up * _speed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ballon"))
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        //we should wait till the last arrow is destroyed
        if (Crossbow.ammo == 0)
        {
            Game.OnLose();
        }
        else
        {
            Crossbow.isCharged = true;
        }
    }
}
