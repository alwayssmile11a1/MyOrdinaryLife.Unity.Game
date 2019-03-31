using Gamekit2D;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(RandomAudioPlayer))]
public abstract class ClickableItem : MonoBehaviour
{
    [Tooltip("How many time you can use this item. Set to minus value for infinite use.")]
    public int count = 1;

    public float countdownTime = 2f;

    [Tooltip("Partical effect on player")]
    public string effect = "Effect";


    //protected Text m_CountText;
    protected int m_HashEffect;
    protected Button m_Button;
    protected Image m_backGroundImage;
    protected RandomAudioPlayer m_Audio;
    protected PlayerPlatformerController m_Player;
    protected int m_CurrentCount = 0;
    protected float m_CurrentCountdownTime = 0;

    protected void Awake()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(OnClick);
        //m_CountText = GetComponentInChildren<Text>();
        m_backGroundImage = GetComponentsInChildren<Image>(true)[1];
        m_backGroundImage.enabled = false;
        m_HashEffect = Gamekit2D.VFXController.StringToHash(effect);
        m_Audio = GetComponent<RandomAudioPlayer>();
    }

    protected void Start()
    {
        m_Player = GameManager.Instance.GetPlayer();
    }

    private void Update()
    {
        if(m_CurrentCountdownTime>0)
        {
            m_CurrentCountdownTime -= Time.deltaTime;
            m_backGroundImage.fillAmount = m_CurrentCountdownTime / countdownTime;
            if (m_CurrentCountdownTime<=0)
            {
                m_Button.enabled = true;
                m_backGroundImage.enabled = false;
                m_backGroundImage.fillAmount = 1;
            }
        }
    }

    private void OnClick()
    {
        if (m_Player != null && m_Player.CanAct() && m_CurrentCountdownTime <= 0 && (m_CurrentCount < count || count < 0))
        {
            if (OnClick(m_Player))
            {
                Gamekit2D.VFXController.Instance.Trigger(m_HashEffect, m_Player.transform.position, 0f, false, null);
                m_Audio.PlayRandomSound();
                m_CurrentCount++;
                m_CurrentCountdownTime = countdownTime;
                m_Button.enabled = false;
                m_backGroundImage.enabled = true;

                // Add this to disabled count down (for item using more than once)
                if (m_CurrentCount >= count)
                {
                    m_CurrentCountdownTime = 0;
                }
            }
        }
    }

    protected abstract bool OnClick(PlayerPlatformerController player);



}
