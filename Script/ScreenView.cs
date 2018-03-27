using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenView : MonoBehaviour {
    public float width, height;
	void Start () {
        GameObject ResetBox = GameObject.Find("ResetConfirm");
        ResetBox.SetActive(false);
        width = Screen.width;
        height = Screen.width / 9 * 16;
        Screen.SetResolution(Screen.width, Screen.width / 9 * 16, true);
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene(MainScreenView.lastScene.Pop());
            }
        }
    }
}
