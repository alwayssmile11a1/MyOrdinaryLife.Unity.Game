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
        StartCoroutine(DisableShield());
        return true;
    }

    private IEnumerator DisableShield()
    {
        yield return new WaitForSeconds(particleSystem.main.duration);
        shieldEffectClone.SetActive(false);
    }
}
