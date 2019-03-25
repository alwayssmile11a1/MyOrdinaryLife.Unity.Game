using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : ClickableItem
{
    public      GameObject      shieldEffect;

    private     GameObject      shieldEffectClone;

    new void Start()
    {
        base.Start();
        shieldEffectClone = Instantiate(shieldEffect, GameManager.Instance.GetPlayer().transform);
    }

    protected override bool OnClick(PlayerPlatformerController player)
    {
        shieldEffectClone.SetActive(true);
        return true;
    }
}
