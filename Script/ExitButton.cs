using UnityEngine;
using System.Collections;

public class ExitButton : MonoBehaviour {
	void ExitControl()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            objActivity.Call("CallFinishDialog");
            //Application.Quit();
        }
    }
}
