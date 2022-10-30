using UnityEngine;

public abstract class Ballon : MonoBehaviour
{
    public GameObject textPrefab;

    protected int _value;
    protected Color _color;
    [SerializeField]
    private float _speed = 1f;
    private Vector3 _offset = new Vector3(0, 0.2f, 0);
    private Vector3 _randomizeSpawn = new Vector3(0.05f, 0.01f, 0);

    void Update()
    {
        if (!Game.pause)
        {
            Vector3 pos = transform.position;
            if (-Game.screenBounds.x < pos.x && pos.x < Game.screenBounds.x)
            {
                transform.Translate(Vector2.right * _speed * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnDestroy()
    {
        BallonSpawn.allBallons--;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Arrow"))
        {
            GameObject text = Instantiate(textPrefab, transform.position, Quaternion.identity, transform);
            if (_value < 0)
            {
                text.GetComponent<TextMesh>().text = _value.ToString();
            }
            else
            {
                text.GetComponent<TextMesh>().text = "+" + Mathf.Abs(_value).ToString();
            }
            text.GetComponent<TextMesh>().color = _color;
            text.transform.localPosition += _offset;
            text.transform.localPosition += new Vector3(Random.Range(-_randomizeSpawn.x, _randomizeSpawn.x), Random.Range(-_randomizeSpawn.y, _randomizeSpawn.y), 0f);

            PopUp();
            Destroy(gameObject, 0.1f);
        }
    }

    protected abstract void PopUp();
}
