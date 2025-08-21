using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemDropRateData
{
    public string ItemName;
    public int DropRate; // 100000 = 100.000%
    public List<AmountDropRateData> AmountRates = new();
}
