using Gamekit2D;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(RandomAudioPlayer))]
public abstract class ClickableItem : MonoBehaviour
{
    public string effect = "Effect";

    protected int HashEffect;
    protected Button m_Button;
    protected Image backGroundImage;
    protected RandomAudioPlayer m_Audio;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnClick);
        backGroundImage = GetComponentsInChildren<Image>(true)[1];
        backGroundImage.enabled = false;
        HashEffect = Gamekit2D.VFXController.StringToHash(effect);
        m_Audio = GetComponent<RandomAudioPlayer>();
    }

    private void OnClick()
    {
        PlayerPlatformerController player = FindObjectOfType<PlayerPlatformerController>();
        if(player!=null)
        {
            OnClick(player);

            Gamekit2D.VFXController.Instance.Trigger(HashEffect, player.transform.position, 0f, false, null);
            m_Audio.PlayRandomSound();
            m_Button.enabled = false;
            backGroundImage.enabled = true;

        }
    }

    protected abstract void OnClick(PlayerPlatformerController player);



}
