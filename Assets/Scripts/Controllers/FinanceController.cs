using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinanceController : Singleton<FinanceController>
{
    protected int currency = 0;

    public int GetCurrency()
    {
        return currency;
    }

    public void SetCurrency(int currency)
    {
        this.currency = currency;
    }
}
