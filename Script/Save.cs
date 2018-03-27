using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class Save : MonoBehaviour {
    private Camera _camera;
    private int resWidth;
    private int resHeight;
    private Sprite TempImage;
    private GameObject Plane;
    private string IDstring, FileTitle;
    private BlockInfo blockinfo;
    void Start()
    {
        _camera = Camera.main;
        resWidth = Screen.width;
        resHeight = Screen.height;
        Plane = GameObject.Find("Plane");
        blockinfo = GameObject.Find("Main Camera").GetComponent<BlockInfo>();
    }
    void DataSave()
    {
        StartCoroutine(PlaneActivefalse());
    }
    IEnumerator PlaneActivefalse()
    {
        Plane.SetActive(false);
        TakeHiResShot();
        yield return new WaitForSeconds(0.5f);
        Plane.SetActive(true);
    }
    public Sprite TakeHiResShot()
    {
        Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        string MyFileName = unixTimestamp + ".png";
        string myDefaultLocation = Application.persistentDataPath + "/" + MyFileName;
        string myFolderLocation = "/storage/emulated/0/DCIM/Unity/";
        string myScreenshotLocation = myFolderLocation + MyFileName;
        if (!System.IO.Directory.Exists(myFolderLocation)) // 폴더가 존재하지 않다면
        {
            System.IO.Directory.CreateDirectory(myFolderLocation);
        }
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        _camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        _camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        TempImage = Sprite.Create(screenShot, rec, new Vector2(0, 0), .01f);

        _camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(myScreenshotLocation, bytes);
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        objActivity.Call("CallSaveDialog", blockinfo.BlockNameInfo, MyFileName); // 안드로이드로 블럭 정보, 파일 이름 전달
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2] { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + myScreenshotLocation) });
        objActivity.Call("sendBroadcast", objIntent);
        return TempImage;
    }
}
