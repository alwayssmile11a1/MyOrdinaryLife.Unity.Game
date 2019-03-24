using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamekit2D
{
    public class AudioPlayerController : MonoBehaviour
    {
        #region Singleton
        public static AudioPlayerController Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;

                s_Instance = FindObjectOfType<AudioPlayerController>();

                if (s_Instance != null)
                    return s_Instance;

                AudioPlayerController gameManagerPrefab = Resources.Load<AudioPlayerController>("AudioPlayerController");
                s_Instance = Instantiate(gameManagerPrefab);

                return s_Instance;
            }
        }

        protected static AudioPlayerController s_Instance;
        #endregion

        public RandomAudioPlayer[] audioPlayers;

        private Dictionary<int, RandomAudioPlayer> m_AudioSourceDictionary = new Dictionary<int, RandomAudioPlayer>();


        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < audioPlayers.Length; i++)
            {
                //RandomAudioPlayer randomAudioPlayer = new GameObject().AddComponent<RandomAudioPlayer>();
                //randomAudioPlayer.clips = audioPlayers[i].clips;
                //randomAudioPlayer.randomizePitch = audioPlayers[i].randomizePitch;
                //randomAudioPlayer.pitchRange = audioPlayers[i].pitchRange;
                //randomAudioPlayer.transform.parent = transform;
                m_AudioSourceDictionary[StringToHash(audioPlayers[i].gameObject.name)] = audioPlayers[i];
            }

        }

        public void PlayRandomSound(int hash)
        {
            RandomAudioPlayer randomAudioPlayer;
            if (m_AudioSourceDictionary.TryGetValue(hash, out randomAudioPlayer))
            {
                randomAudioPlayer.PlayRandomSound();
            }
            else
            {
                Debug.LogWarning("Audio does not exist.");
            }
        }


        public static int StringToHash(string name)
        {
            return name.GetHashCode();
        }

    }

    //[System.Serializable]
    //public class AudioPlayer
    //{
    //    public string name;
    //    public AudioClip[] clips;
    //    public bool randomizePitch = false;
    //    public float pitchRange = 0.2f;
    //}


}