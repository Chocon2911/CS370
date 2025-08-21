using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UICode
{
    SHOP_SLOT = 0,
    SHOP_ITEM = 1,
}

public static class UICodeExtension
{
    public static string GetStr(this UICode code)
    {
        switch (code)
        {
            case UICode.SHOP_ITEM:
                return "ShopItem"; 
            default:
                return "";
        }
    }
}
