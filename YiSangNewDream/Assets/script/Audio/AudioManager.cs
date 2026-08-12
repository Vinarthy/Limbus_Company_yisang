using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioManager Instance;


    [SerializeField]
    private AudioConfig config;//


    private AudioSource bgmSource;
    private AudioSource secondaryBgmSource;
    private AudioSource sfxSource;
    private AudioSource uiSource;


    private float bgmVolume = 0.7f;
    private float secondaryBgmVolume = 0.7f;
    private float sfxVolume = 1;
    private float uiVolume = 1;

    private const float MusicFadeDuration = 1f;
    private Coroutine bgmFadeCoroutine;
    private Coroutine secondaryBgmFadeCoroutine;
    private float bgmPlaybackVolume = 1f;
    private float secondaryBgmPlaybackVolume = 1f;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }



    private void Init()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();

        secondaryBgmSource = gameObject.AddComponent<AudioSource>();

        sfxSource = gameObject.AddComponent<AudioSource>();

        uiSource = gameObject.AddComponent<AudioSource>();


        bgmSource.loop = true;
        secondaryBgmSource.loop = false;


        bgmSource.volume = bgmVolume;

        secondaryBgmSource.volume = secondaryBgmVolume;

        sfxSource.volume = sfxVolume;

        uiSource.volume = uiVolume;

    }



    //���ű�������
    public void PlayBGM(string id, float volume = 1f)
    {
        AudioClip clip = config.GetBGM(id);

        if (clip == null)
            return;

        if (bgmSource.clip == clip)
            return;

        bgmPlaybackVolume = volume;
        StopBGMFade();
        bgmSource.clip = clip;
        bgmSource.volume = 0f;
        bgmSource.Play();
        StartBGMFade(bgmVolume * bgmPlaybackVolume, false);
    }




    //������Ч
    public void PlaySecondaryBGM(string id, float volume = 1f)
    {
        AudioClip clip = config.GetSecondaryBGM(id);

        if (clip == null)
            return;

        secondaryBgmPlaybackVolume = volume;
        StopSecondaryBGMFade();
        secondaryBgmSource.clip = clip;
        secondaryBgmSource.volume = 0f;
        secondaryBgmSource.Play();
        StartSecondaryBGMFade(secondaryBgmVolume * secondaryBgmPlaybackVolume, false);
    }



    public void StopSecondaryBGM()
    {
        if (!secondaryBgmSource.isPlaying)
        {
            StopSecondaryBGMFade();
            secondaryBgmSource.Stop();
            secondaryBgmSource.volume = 0f;
            return;
        }

        StartSecondaryBGMFade(0f, true);
    }



    public void PlaySFX(string id, float volume = 1f)
    {
        AudioClip clip = config.GetSFX(id);

        if (clip == null)
            return;


        sfxSource.PlayOneShot(
            clip,
            volume
        );
    }




    //����UI����
    public void PlayUI(string id, float volume = 1f)
    {
        AudioClip clip = config.GetUI(id);

        if (clip == null)
            return;


        uiSource.PlayOneShot(
            clip,
            volume
        );
    }



    //������������

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;

        bgmSource.volume = value;
    }



    public void SetSecondaryBGMVolume(float value)
    {
        secondaryBgmVolume = value;

        secondaryBgmSource.volume = value;
    }



    public void SetSFXVolume(float value)
    {
        sfxVolume = value;

        sfxSource.volume = value;
    }



    public void SetUIVolume(float value)
    {
        uiVolume = value;

        uiSource.volume = value;
    }



    public void PauseBGM()
    {
        if (bgmSource.isPlaying)
            StartBGMFade(0f, true);
    }



    public void ResumeBGM()
    {
        if (bgmSource.clip == null)
            return;

        StopBGMFade();
        bgmSource.UnPause();

        if (!bgmSource.isPlaying)
            bgmSource.Play();

        StartBGMFade(bgmVolume * bgmPlaybackVolume, false);
    }


    private void StartBGMFade(float targetVolume, bool pauseAfterFade)
    {
        StopBGMFade();
        bgmFadeCoroutine = StartCoroutine(FadeBGM(targetVolume, pauseAfterFade));
    }


    private IEnumerator FadeBGM(float targetVolume, bool pauseAfterFade)
    {
        yield return FadeVolume(bgmSource, targetVolume);

        if (pauseAfterFade)
            bgmSource.Pause();

        bgmFadeCoroutine = null;
    }


    private void StopBGMFade()
    {
        if (bgmFadeCoroutine == null)
            return;

        StopCoroutine(bgmFadeCoroutine);
        bgmFadeCoroutine = null;
    }


    private void StartSecondaryBGMFade(float targetVolume, bool stopAfterFade)
    {
        StopSecondaryBGMFade();
        secondaryBgmFadeCoroutine = StartCoroutine(FadeSecondaryBGM(targetVolume, stopAfterFade));
    }


    private IEnumerator FadeSecondaryBGM(float targetVolume, bool stopAfterFade)
    {
        yield return FadeVolume(secondaryBgmSource, targetVolume);

        if (stopAfterFade)
        {
            secondaryBgmSource.Stop();
            secondaryBgmSource.volume = 0f;
        }
        else
        {
            while (secondaryBgmSource.isPlaying
                && secondaryBgmSource.clip != null
                && secondaryBgmSource.time < secondaryBgmSource.clip.length - MusicFadeDuration)
            {
                yield return null;
            }

            if (secondaryBgmSource.isPlaying)
                yield return FadeVolume(secondaryBgmSource, 0f);

            secondaryBgmSource.Stop();
            secondaryBgmSource.volume = 0f;
        }

        secondaryBgmFadeCoroutine = null;
    }


    private void StopSecondaryBGMFade()
    {
        if (secondaryBgmFadeCoroutine == null)
            return;

        StopCoroutine(secondaryBgmFadeCoroutine);
        secondaryBgmFadeCoroutine = null;
    }


    private static IEnumerator FadeVolume(AudioSource source, float targetVolume)
    {
        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < MusicFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(
                startVolume,
                targetVolume,
                Mathf.Clamp01(elapsed / MusicFadeDuration));
            yield return null;
        }

        source.volume = targetVolume;
    }


    public float GetBGMVolume()
    {
        return bgmVolume;
    }


    public float GetSecondaryBGMVolume()
    {
        return secondaryBgmVolume;
    }


    public float GetSFXVolume()
    {
        return sfxVolume;
    }


    public float GetUIVolume()
    {
        return uiVolume;
    }
    //有一说一啊
    //AudioManager.Instance.PauseBGM();zan ting
    //AudioManager.Instance.ResumeBGM();ji xv dai ma
}
