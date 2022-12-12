using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinanceController : Singleton<FinanceController>
{
    protected int currency = 0;
    protected Vector2 weapon_pos = new Vector2(0.0f, 0.0f);

    public Vector2 GetWeaponPos()
    {
        return weapon_pos;
    }

    public void SetWeaponPos(Vector2 wp)
    {
        weapon_pos = wp;
    }

    public int GetCurrency()
    {
        return currency;
    }

    public void SetCurrency(int currency)
    {
        this.currency = currency;
    }

    public void BuyWeapon(GameObject gameObject)
    {
        WeaponBehavior weaponBehavior = gameObject.transform.GetChild(0).GetComponent<WeaponBehavior>();
        SetCurrency(GetCurrency() - weaponBehavior.GetCurrency());
    }
}
