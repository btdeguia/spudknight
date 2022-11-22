using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinanceController : Singleton<FinanceController>
{
    protected int currency = 25;

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
        WeaponBehavior weaponBehavior = gameObject.GetComponent<WeaponBehavior>();
        if (gameObject != null && weaponBehavior != null)
        {
            SetCurrency(GetCurrency() - weaponBehavior.GetCurrency());
        }
    }
}
