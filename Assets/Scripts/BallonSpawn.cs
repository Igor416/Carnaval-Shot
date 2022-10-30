using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Score();

public class BallonSpawn : MonoBehaviour
{
    public GameObject[] ballonsTypesPrefabs = new GameObject[4]; //red, blue, yellow, black

    public static int score;
    public static int allBallons = 0;

    [SerializeField]
    private int _maxBallons = 30;
    private readonly static List<GameObject> _ballons = new List<GameObject>();
    private Vector3 _screenBounds;

    private IEnumerator Spawn()
    {
        while (allBallons <= _maxBallons)
        {
            yield return new WaitForSeconds(1f);
            if (!Game.pause)
            {
                float posX = -_screenBounds.x + 1f;
                float posY = Random.Range(_screenBounds.y - 5f, _screenBounds.y - 1f);
                GameObject ballon = ballonsTypesPrefabs[0];
                int probability, sum = 100; //red 50%, blue 15%, yellow 15%, black 20%
                probability = Random.Range(0, sum + 1);

                if (probability < 50)
                {
                    ballon = ballonsTypesPrefabs[0];
                }
                else if (50 <= probability && probability < 65)
                {
                    ballon = ballonsTypesPrefabs[1];
                }
                else if (65 <= probability && probability < 80)
                {
                    ballon = ballonsTypesPrefabs[2];
                }
                else if (80 <= probability && probability < 100)
                {
                    ballon = ballonsTypesPrefabs[3];
                }

                ballon = Instantiate(ballon, new Vector3(posX, posY), Quaternion.identity);
                _ballons.Add(ballon);
                allBallons++;
            }
        }

    }

    void Start()
    {
        _screenBounds = Game.screenBounds;
        _screenBounds.x += ballonsTypesPrefabs[0].transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        _screenBounds.y += ballonsTypesPrefabs[0].transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        StartCoroutine(Spawn());
    }

    public static void PopUpAll()
    {
        for (int i = 0; i < _ballons.Count; i++)
        {
            allBallons--;
            Destroy(_ballons[i], Random.Range(0.2f, 1f));
        }
    }
}
