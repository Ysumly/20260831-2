using UnityEngine;

public class ManagerOfManagersTest : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("ManagerOfManagersTest Start");

        Debug.Log("=========================================");
        Debug.Log("UIManager Test");

        var uiManager = ManagerCenter.Instance.Get<UIManager>();
        uiManager.ShowPanel<TestPanel>();

        // AudioManagerTest();

    }

    private static void AudioManagerTest()
    {
        Debug.Log("=========================================");
        Debug.Log("AudioManager Test");

        Debug.Log($"Music Volume: {ManagerCenter.Instance.Get<AudioManager>().MusicVolume}");
        Debug.Log($"SFX Volume: {ManagerCenter.Instance.Get<AudioManager>().SfxVolume}");

        Debug.Log("Setting Music Volume to 0.5");
        ManagerCenter.Instance.Get<AudioManager>().SetMusicVolume(0.5f);
        Debug.Log("Setting SFX Volume to 0.8");
        ManagerCenter.Instance.Get<AudioManager>().SetSfxVolume(0.8f);

        Debug.Log($"Music Volume: {ManagerCenter.Instance.Get<AudioManager>().MusicVolume}");
        Debug.Log($"SFX Volume: {ManagerCenter.Instance.Get<AudioManager>().SfxVolume}");
    }
}

