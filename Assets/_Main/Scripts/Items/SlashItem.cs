using Gamekit2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashItem : ClickableItem {

    

    protected override bool OnClick(PlayerPlatformerController player)
    {
        return player.Attack3();
    }


}
