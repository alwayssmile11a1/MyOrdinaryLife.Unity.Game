using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : ClickableItem
{
    public GameObject shieldEffect;

    protected override bool OnClick(PlayerPlatformerController player)
    {
        //throw new System.NotImplementedException();
        //VFXController.Instance
        shieldEffect.SetActive(true);
        return true;
    }
}
