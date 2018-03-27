using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            objActivity.Call("CallFinishDialog");
        }
    }
}
