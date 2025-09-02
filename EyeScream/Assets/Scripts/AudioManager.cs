using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class AudioManager
    {

        private static bool musicIsSetup;
        private static AudioSource musicSource;
        private static bool sfxIsSetup;
        private static AudioSource sfxSource;

        public static void ManualSetup()
        {
            SetupSFXSystem();
            SetupMusicSystem();
        }

        public static void PlaySFX(string clipName)
        {
            if (!sfxIsSetup) SetupSFXSystem();
            sfxSource.PlayOneShot(Resources.Load<AudioClip>("Audio/SFX/"+clipName));
        }

        public static void PlayMusic(AudioClip clip)
        {
            if (!musicIsSetup) SetupMusicSystem();
            musicSource.clip = clip;
            if (!musicSource.isPlaying) musicSource.Play();
        }

        private static void SetupMusicSystem()
        {
            musicIsSetup = true;
            GameObject MusicHandler = new GameObject("MusicHandler");
            musicSource = MusicHandler.AddComponent<AudioSource>();
        }

        private static void SetupSFXSystem()
        {
            sfxIsSetup = true;
            GameObject SFXHandler = new GameObject("SFXHandler");
            sfxSource = SFXHandler.AddComponent<AudioSource>();
        }
    }