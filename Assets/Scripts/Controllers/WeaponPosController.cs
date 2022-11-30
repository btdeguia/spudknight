using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPosController : Singleton<WeaponPosController>
{
    protected Vector2 weapon_pos = new Vector2(0.0f, 0.0f);

    public Vector2 GetWeaponPos()
    {
        return weapon_pos;
    }

    public void SetWeaponPos(Vector2 wp)
    {
        weapon_pos = wp;
    }

}
