using UnityEngine;

public class YellowBallon : Ballon
{
    void Start()
    {
        _value = 3;
        _color = Color.yellow;
    }

    protected override void PopUp()
    {
        if (Crossbow.ammo + _value > Crossbow.allAmmo)
        {
            Crossbow.ammo = Crossbow.allAmmo;
        }
        else
        {
            Crossbow.ammo += _value;
        }
        UI.OnStatsChanged();
    }
}
