using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoostItem : ClickableItem {

    public float speed = 8f;
    public float increasedSpeed = 2f;
    public float decreasedSpeed = 1f;

    protected override void OnClick(PlayerPlatformerController player)
    {
        player.GetComponent<Animator>().SetBool(player.m_HashRunFastPara, true);
        StartCoroutine(Boost(player));
    }

    private IEnumerator Boost(PlayerPlatformerController player)
    {
        float oldSpeed = player.speed;

        while (player.speed < speed)
        {
            player.speed = player.speed + Time.deltaTime * increasedSpeed;
            if (player.speed > speed) player.speed = speed;
            yield return null;
        }

        while (player.speed > oldSpeed)
        {
            player.speed = player.speed - Time.deltaTime * decreasedSpeed;
            if (player.speed < oldSpeed) player.speed = oldSpeed;
            yield return null;
        }

        player.GetComponent<Animator>().SetBool(player.m_HashRunFastPara, false);

    }

}
