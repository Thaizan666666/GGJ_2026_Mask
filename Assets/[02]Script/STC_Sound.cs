using UnityEngine;

public static class STC_Sound
{
    private static AudioSource musicSource;
    private static AudioSource sfxSource;
    private static GameObject soundObject;

    // ตั้งค่าเริ่มต้น
    public static void Initialize()
    {
        if (soundObject == null)
        {
            // สร้าง GameObject สำหรับเสียง
            soundObject = new GameObject("STC_SoundManager");
            Object.DontDestroyOnLoad(soundObject);

            // สร้าง AudioSource สำหรับเพลง
            musicSource = soundObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;

            // สร้าง AudioSource สำหรับ Sound Effects
            sfxSource = soundObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
    }

    public static void PlayMusic(AudioClip clip, float volume = 1f)
    {
        Initialize();

        if (clip != null && musicSource != null)
        {
            musicSource.clip = clip;
            musicSource.volume = volume;
            musicSource.Play();
        }
    }

    // หยุดเพลง
    public static void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    // Pause เพลง
    public static void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }

    // เล่นเพลงต่อ
    public static void ResumeMusic()
    {
        if (musicSource != null)
        {
            musicSource.UnPause();
        }
    }

    // เล่น Sound Effect
    public static void PlaySFX(AudioClip clip, float volume = 1f)
    {
        Initialize();

        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    // ปรับ Volume เพลง
    public static void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp01(volume);
        }
    }

    // ปรับ Volume SFX
    public static void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }
    }

    // ตรวจสอบว่าเพลงกำลังเล่นอยู่หรือไม่
    public static bool IsMusicPlaying()
    {
        return musicSource != null && musicSource.isPlaying;
    }
}