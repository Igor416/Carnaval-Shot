using UnityEngine;

public class BlueBallon : Ballon
{
    void Start()
    {
        _value = Random.Range(2, 5);
        _color = Color.cyan;
    }

    protected override void PopUp()
    {
        BallonSpawn.score += _value;
        UI.OnStatsChanged();
    }
}
