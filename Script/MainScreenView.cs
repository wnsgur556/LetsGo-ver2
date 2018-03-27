using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MainScreenView : MonoBehaviour {
    public static Stack<string> lastScene = new Stack<string>();
    void Start()
    {
        Screen.SetResolution(1280, 800, true);
        //Screen.SetResolution(Screen.width, Screen.width * 16 / 10, true);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        if (PlayerPrefs.HasKey("MainSoundSave"))
            GlobalBGM.instance.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MainSoundSave", 0f);
        else
            GlobalBGM.instance.GetComponent<AudioSource>().volume = 1.0f;
        if (PlayerPrefs.HasKey("EffectSoundSave"))
            GlobalEffectSound.instance.mySound.volume = GlobalEffectSound.instance.mySound2.volume 
                = PlayerPrefs.GetFloat("EffectSoundSave", 0f);
        else
            GlobalEffectSound.instance.mySound.volume = GlobalEffectSound.instance.mySound2.volume = 1.0f;
    }
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (lastScene.Count == 0)
                    Application.Quit();
                else
                {
                    if (lastScene.Peek() == "Menu_Scene" || lastScene.Peek() == "AnimalMenu_Scene" ||
                        lastScene.Peek() == "BuildingMenu_Scene" || lastScene.Peek() == "ObjectMenu_Scene")
                    {
                        BrickManager.createBricks.Clear();
                        BrickManager.brickList.Clear();
                        MissionManager.matchCount = 0;
                        SceneManager.LoadScene(lastScene.Pop());
                    }
                    else
                        SceneManager.LoadScene(lastScene.Pop());
                }
            }
        }
    }
}
