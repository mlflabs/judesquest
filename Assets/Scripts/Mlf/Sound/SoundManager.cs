using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mlf.Sound
{
    public enum BackgroundSoundState
    {
        normal, fast, slow
    }

    [Serializable]
    public struct SoundItem
    {
        public string name;
        public AudioClip clip;
    }

    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {

        // Determine default state and whether to save
        public bool startOn = true, saveSettings = true, keepAlive = false;
        // Links to components
        public Sprite musicOnSprite, musicOffSprite;
        public Image image;
        public AudioSource audioSource;
        public static SoundManager instance;

        private BackgroundSoundState state;

        [SerializeField] public AudioClip normal;
        [SerializeField] public AudioClip fast;
        [SerializeField] public AudioClip dialogue;
        public SoundItem[] audioClips;

        // Current music status
        bool musicOn;

        /// <summary>Public accessor for current music status</summary>
        public bool MusicOn
        {
            get { return musicOn; }
        }

        public BackgroundSoundState State
        {
            get => state; set
            {
                state = value;

                switch (state)
                {
                    case BackgroundSoundState.normal:
                        audioSource.clip = normal;
                        break;

                    case BackgroundSoundState.fast:
                        audioSource.clip = fast;
                        break;

                    default:
                        audioSource.clip = normal;
                        break;
                }
            }
        }


        void Start()
        {

            audioSource = GetComponent<AudioSource>();
            if (keepAlive)
                // Ensure object stays alive between scenes
                DontDestroyOnLoad(gameObject);

            // Set default state based on startOn or PlayerPrefs.
            if (saveSettings && PlayerPrefs.HasKey("sumMusicOn"))
                musicOn = !(PlayerPrefs.GetInt("sumMusicOn") > 0);    // Convert from int to (flipped) bool
            else
                musicOn = !startOn; // Flip default before toggle

            ToggleMusic(true); // Use toggle to set initial state (w/o save)
        }

        /// <summary>
        /// Public accessor for toggling music from button of script
        /// </summary>
        public void ToggleMusic()
        {
            ToggleMusic(false);
        }

        public void PlayInteractSound(string name)
        {
            Debug.Log("Playing Sound::::::::::::: " + name);
            if (audioClips == null)
            {
                Debug.LogWarning("Sound Manager Audio Clips missing");
                return;
            }

            for (int i = 0; i < audioClips.Length; i++)
            {
                if (audioClips[i].name == name)
                {
                    audioSource.PlayOneShot(audioClips[i].clip);
                    return;
                }
            }

            Debug.LogWarning("No Audio Clip named: " + name);

        }

        /// <summary>
        /// Toggles music status, switches sprite, and saves if needed
        /// </summary>
        /// <param name="skipSave">
        /// Allows you to Toggle without saving regardless of saveSettings value
        /// </param>
        void ToggleMusic(bool skipSave)
        {

            // Check that sprites are linked properly
            if (!checkReqs())
            {
                Debug.LogError("Link references missing on <b>sumMusic</b> object. Please check assignments in editor.");
                return;
            }

            // Flip value of musicOn
            musicOn = !musicOn;
            Debug.Log("Music status changed to " + musicOn);

            // Play or stop music
            if (musicOn)
                audioSource.Play();
            else
                audioSource.Stop();

            // Switch sprite to appropriate value
            if (image != null)
                image.sprite = musicOn ? musicOnSprite : musicOffSprite;

            // Save status to PlayerPrefs as int if needed (1=on,0=off)
            if (saveSettings && !skipSave)
            {
                Debug.Log("Saving sound settings");
                PlayerPrefs.SetInt("sumMusicOn", musicOn ? 1 : 0);
            }
        }

        /// <summary>Checks to make sure necessary objects are assigned</summary>
        /// <returns>True or False</returns>
        bool checkReqs()
        {
            return (musicOnSprite != null && musicOffSprite != null && image != null);
        }


        void Awake()
        {

            if (instance == null)
            {

                instance = this;
                //DontDestroyOnLoad(this.gameObject);

                //Rest of your Awake code

            }
            else
            {
                Destroy(this);
            }
        }

    }
}