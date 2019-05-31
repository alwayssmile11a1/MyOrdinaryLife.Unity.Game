using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife
{
    private static PlayerLife instance;

    public static PlayerLife Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerLife();
            }
            return instance;
        }
    }

    private int m_PlayerLife;

    public double playerLifeExpandElapsedTime;
    
    public void CalculateOfflineLife()
    {
        int additionalLife = 0;
        string m_DatePlayerLeft;
        string m_TimePlayerLeft;
        if (PlayerPrefs.HasKey(PlayerPrefsString.datePlayerLeft))
        {
            m_DatePlayerLeft = PlayerPrefs.GetString(PlayerPrefsString.datePlayerLeft);

            m_TimePlayerLeft = PlayerPrefs.GetString(PlayerPrefsString.timePlayerLeft);

            DateTime timePlayerLeft = DateTime.Parse(m_TimePlayerLeft);
            TimeSpan timeSpan = DateTime.Now.Subtract(timePlayerLeft);
            //int minute = timeSpan.Minutes;

            //playerLifeExpandElapsedTime = PlayerPrefs.GetFloat(PlayerPrefsString.playerLifeExpandElapsedTime);
            //int playerLifeExpandInMinute = (int)(playerLifeExpandElapsedTime / 60);
            //if (minute - ( 15 - playerLifeExpandInMinute) >= 0)
            //{
            //    additionalLife++;
            //    minute -= (15 - playerLifeExpandInMinute);
            //    playerLifeExpandElapsedTime = 0;
            //}
            //else
            //{
            //    playerLifeExpandElapsedTime += minute * 60;
            //}
            //additionalLife = timeSpan.Minutes / 15;

            double second = timeSpan.TotalSeconds;
            playerLifeExpandElapsedTime = double.Parse(PlayerPrefs.GetString(PlayerPrefsString.playerLifeExpandElapsedTime));

            if (second + playerLifeExpandElapsedTime >= 900)
            {
                additionalLife++;
                second -= 900 - playerLifeExpandElapsedTime;
                additionalLife += (int)second / 900;
                playerLifeExpandElapsedTime = second % 900;
            }
            else
            {
                playerLifeExpandElapsedTime += second;
            }

        }

        if (PlayerPrefs.HasKey(PlayerPrefsString.playerLife))
        {
            m_PlayerLife = Mathf.Clamp(PlayerPrefs.GetInt(PlayerPrefsString.playerLife) + additionalLife, 0, 30);
            PlayerPrefs.SetInt(PlayerPrefsString.playerLife, m_PlayerLife);
        }
        else
        {
            PlayerPrefs.SetInt(PlayerPrefsString.playerLife, 30);
            m_PlayerLife = 30;
        }
    }

    public void TimePlayerOut()
    {
        PlayerPrefs.SetString(PlayerPrefsString.datePlayerLeft, DateTime.Now.ToShortDateString());
        PlayerPrefs.SetString(PlayerPrefsString.timePlayerLeft, DateTime.Now.ToLongTimeString());
        PlayerPrefs.SetString(PlayerPrefsString.playerLifeExpandElapsedTime, playerLifeExpandElapsedTime.ToString());
    }

    /// <summary>
    /// Update playerLife variable
    /// </summary>
    /// <param name="minus">if true subtract playerLife by 1, if false plus playerLife by 1</param>
    public void UpdatePlayerLife(bool minus)
    {
        m_PlayerLife = minus ? --m_PlayerLife : ++m_PlayerLife;
        PlayerPrefs.SetInt("playerLife", m_PlayerLife);
    }

    public int GetPlayerLife()
    {
        return m_PlayerLife;
    }
}
