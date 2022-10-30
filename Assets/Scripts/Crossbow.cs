using System.Collections;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject[] blindZonesPrefabs = new GameObject[1];

    public static int ammo;
    public static int allAmmo = 15;
    public static string rechargeStatus;
    public static bool isCharged = true;

    private readonly string[] _statuses = new string[4] { "Charging.  ", "Charging.. ", "Charging...", "Charged   " };
    [SerializeField]
    private float _chargingTime = 0.1f;

    private IEnumerator Recharge()
    {
        if (ammo == 0)
        {
            rechargeStatus = "no ammo";
            UI.OnStatsChanged();
        }
        else
        {
            for (int i = 0; i < _statuses.Length; i++)
            {
                rechargeStatus = _statuses[i];
                UI.OnStatsChanged();
                yield return new WaitForSeconds(_chargingTime / _statuses.Length);
            }
            isCharged = true;
        }
    }

    void Start()
    {
        ammo = allAmmo;
        rechargeStatus = _statuses[3];
        UI.OnStatsChanged();
    }

    void Update()
    {
        if (!Game.pause && Input.GetMouseButtonDown(0))
        {
            if (ammo == 0)
            {
                rechargeStatus = "no ammo";
                UI.OnStatsChanged();
            }
            else if (isCharged && ammo > 0)
            {
                Vector3 point = Input.mousePosition;
                bool isInBlindZone = false;
                for (int i = 0; i < blindZonesPrefabs.Length; i++)
                {
                    Vector3 pos = blindZonesPrefabs[i].transform.position;
                    pos.x -= blindZonesPrefabs[i].GetComponent<RectTransform>().rect.width / 2;
                    pos.y -= blindZonesPrefabs[i].GetComponent<RectTransform>().rect.height / 2;
                    Vector3 offset = pos;
                    offset.x += blindZonesPrefabs[i].GetComponent<RectTransform>().rect.width;
                    offset.y += blindZonesPrefabs[i].GetComponent<RectTransform>().rect.height;
                    if (pos.x < point.x && point.x < offset.x)
                    {
                        if (pos.y < point.y && point.y < offset.y)
                        {
                            isInBlindZone = true;
                            break;
                        }
                    }
                }
                if (!isInBlindZone)
                {
                    point = Camera.main.ScreenToWorldPoint(point);
                    LookAt(point - transform.position);
                    Shoot();
                }
            }
        }
    }

    private void LookAt(Vector3 point)
    {
        float rotationZ = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    private void Shoot()
    {
        UI.OnStatsChanged();
        isCharged = false;
        Instantiate(arrowPrefab, transform.position, transform.rotation);
        StartCoroutine(Recharge());
    }
}
