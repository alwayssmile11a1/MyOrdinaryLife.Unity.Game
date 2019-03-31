using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : ClickableItem
{
    public      GameObject      shieldEffect;

    private     GameObject      shieldEffectClone;
    private new ParticleSystem  particleSystem;

    new void Start()
    {
        base.Start();
        shieldEffectClone = Instantiate(shieldEffect, GameManager.Instance.GetPlayer().transform);
        particleSystem = shieldEffectClone.GetComponentInChildren<ParticleSystem>();
    }

    protected override bool OnClick(PlayerPlatformerController player)
    {
        shieldEffectClone.SetActive(true);
        Invoke("DisableShield", particleSystem.main.duration);
        return true;
    }

    private void DisableShield()
    {
        shieldEffectClone.SetActive(false);
    }
}
