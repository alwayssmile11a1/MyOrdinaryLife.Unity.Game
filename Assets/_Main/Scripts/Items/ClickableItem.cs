using Gamekit2D;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(RandomAudioPlayer))]
public abstract class ClickableItem : MonoBehaviour
{
    public string effect = "Effect";

    protected int m_HashEffect;
    protected Button m_Button;
    protected Image m_backGroundImage;
    protected RandomAudioPlayer m_Audio;
    protected PlayerPlatformerController m_Player;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnClick);
        m_backGroundImage = GetComponentsInChildren<Image>(true)[1];
        m_backGroundImage.enabled = false;
        m_HashEffect = Gamekit2D.VFXController.StringToHash(effect);
        m_Audio = GetComponent<RandomAudioPlayer>();
        m_Player = FindObjectOfType<PlayerPlatformerController>();
    }

    private void OnClick()
    {
        if(m_Player != null && m_Player.CanAct())
        {
            OnClick(m_Player);

            Gamekit2D.VFXController.Instance.Trigger(m_HashEffect, m_Player.transform.position, 0f, false, null);
            m_Audio.PlayRandomSound();
            m_Button.enabled = false;
            m_backGroundImage.enabled = true;

        }
    }

    protected abstract void OnClick(PlayerPlatformerController player);



}
