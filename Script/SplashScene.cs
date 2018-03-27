using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour {
    public float delayTime = 2;
    IEnumerator Start()
    {
        Screen.SetResolution(Screen.width, Screen.width * 16 / 10, true);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("Menu_Scene");
    }
}
