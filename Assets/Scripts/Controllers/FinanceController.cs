using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinanceController : Singleton<FinanceController>
{

    protected int currency;

    // Start is called before the first frame update
    void Start()
    {
        currency = 0;
    }

    public int GetCurrency()
    {
        return currency;
    }

    public void SetCurrency(int currency)
    {
        this.currency = currency;
    }

    /*public void BuyWeapon(GameObject gameObject)
    {
        WeaponBehavior weaponBehavior = gameObject.GetComponent<WeaponBehavior>();
        if (gameObject != null && weaponBehavior != null)
        {
            SetCurrency(GetCurrency() - weaponBehavior.GetCurrency());
        }
    }*/
}
