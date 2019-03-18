using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostItem : ClickableItem {

    public float speed = 8f;
    public float decreasedSpeed = 1f;

    protected override void OnClick(PlayerPlatformerController player)
    {
        player.GetComponent<Animator>().SetBool(player.m_HashRunFastPara, true);
        StartCoroutine(Boost(player));
    }

    private IEnumerator Boost(PlayerPlatformerController player)
    {
        float oldSpeed = player.speed;
        player.speed = speed;

        while(player.speed > oldSpeed)
        {
            player.speed = player.speed - Time.deltaTime * decreasedSpeed;
            yield return null;
        }
        player.GetComponent<Animator>().SetBool(player.m_HashRunFastPara, false);
        player.speed = oldSpeed;

    }

}
