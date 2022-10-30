using UnityEngine;

public class RedBallon : Ballon
{
    void Start()
    {
        _value = 1;
        _color = Color.red;
    }

    protected override void PopUp()
    {
        BallonSpawn.score += _value;
        UI.OnStatsChanged();
    }
}
