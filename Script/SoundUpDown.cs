using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SoundUpDown : MonoBehaviour {
    public UISlider mainSoundSlider, effectSoundSlider;
    public float mainVol, effectVol;

    void Start()
    {
        Screen.SetResolution(1280, 800, true);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        if (PlayerPrefs.HasKey("MainSoundSave"))
            GlobalBGM.instance.GetComponent<AudioSource>().volume = mainSoundSlider.sliderValue = PlayerPrefs.GetFloat("MainSoundSave", 0f);
        else
            GlobalBGM.instance.GetComponent<AudioSource>().volume = mainSoundSlider.sliderValue = 1.0f;
        if (PlayerPrefs.HasKey("EffectSoundSave"))
            GlobalEffectSound.instance.mySound.volume = GlobalEffectSound.instance.mySound2.volume =
                 effectSoundSlider.sliderValue = PlayerPrefs.GetFloat("EffectSoundSave", 0f);
        else
            GlobalEffectSound.instance.mySound.volume = effectSoundSlider.sliderValue = 
                GlobalEffectSound.instance.mySound2.volume = 1.0f;
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (MainScreenView.lastScene.Count == 0)
                    Application.Quit();
                else
                {
                    SceneManager.LoadScene(MainScreenView.lastScene.Pop());
                    PlayerPrefs.SetFloat("MainSoundSave", mainVol);
                    PlayerPrefs.SetFloat("EffectSoundSave", effectVol);
                    PlayerPrefs.Save();
                }
            }
        }
    }

    public void mainSoundManager()
    {
        mainVol = GlobalBGM.instance.GetComponent<AudioSource>().volume = mainSoundSlider.sliderValue;
    }

    public void effectSoundManager()
    {
        effectVol = GlobalEffectSound.instance.mySound.volume = GlobalEffectSound.instance.mySound2.volume
            = effectSoundSlider.sliderValue;
    }
}
