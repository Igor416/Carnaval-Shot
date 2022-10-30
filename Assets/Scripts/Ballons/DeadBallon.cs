using UnityEngine;

public class DeadBallon : Ballon
{
    void Start()
    {
        _value = -5;
        _color = Color.black;
    }

    protected override void PopUp()
    {
        BallonSpawn.score += _value;
        UI.OnStatsChanged();
    }
}
