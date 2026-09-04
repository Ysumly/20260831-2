using UnityEngine;

/// <summary>
/// 音频管理器: 管理背景音乐与音效的音量 (0~1).
/// 背景音乐使用循环音源, 音效使用一次性播放.
/// </summary>
public class AudioManager : BaseManager<AudioManager>
{
    private AudioSource musicSource;
    private AudioSource sfxSource;

    /// <summary>背景音乐音量 (0~1)</summary>
    public float MusicVolume { get; private set; } = 0.5f;

    /// <summary>音效音量 (0~1)</summary>
    public float SfxVolume { get; private set; } = 0.5f;

    protected override void OnInit()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        //将数据读取到Manager中
        MusicVolume = ManagerCenter.Instance.Get<GameDataManager>().UserData.MusicVolume;
        SfxVolume = ManagerCenter.Instance.Get<GameDataManager>().UserData.SFXVolume;
        ApplyVolumes();
    }

    /// <summary>设置背景音乐音量, 自动限制在 0~1 范围内</summary>
    public void SetMusicVolume(float volume)
    {
        MusicVolume = Mathf.Clamp01(volume);
        ManagerCenter.Instance.Get<GameDataManager>().UserData.MusicVolume = MusicVolume;
        ApplyVolumes();
    }

    /// <summary>设置音效音量, 自动限制在 0~1 范围内</summary>
    public void SetSfxVolume(float volume)
    {
        SfxVolume = Mathf.Clamp01(volume);
        ManagerCenter.Instance.Get<GameDataManager>().UserData.SFXVolume = SfxVolume;
        ApplyVolumes();
    }

    /// <summary>播放背景音乐 (循环播放)</summary>
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    /// <summary>停止背景音乐</summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }

    /// <summary>播放一个音效</summary>
    public void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    private void ApplyVolumes()
    {
        if (musicSource != null) musicSource.volume = MusicVolume;
        if (sfxSource != null) sfxSource.volume = SfxVolume;
    }
}
