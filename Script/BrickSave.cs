using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BrickSave : MonoBehaviour {
    public string saveData;
    public string saveTitle;
    public UIAtlas saveListAtlas;
    public GameObject editText, okButton, ListPanel, ListSpriteDown, ListSprite, HomeBt, BackupBt, SaveBt, tmpGameObj;

    //Dictionary<string, string> tmp = new Dictionary<string, string>();
    GameObject tmpObject = null;

    private StringBuilder sb = new StringBuilder();
    private string m_URL = "http://letsplock.cafe24.com/api/upload_image.jsp";
    private string deviceInfo;

    private void Start()
    {
       deviceInfo = SystemInfo.deviceUniqueIdentifier;
    }

    void brickSave()
    {
        editText.SetActive(true);
        okButton.SetActive(true);
        ListPanel.SetActive(false); // 저장, 취소 시 다시 활성화
        ListSpriteDown.SetActive(false); // 저장, 취소 시 다시 활성화
        ListSprite.SetActive(false); // 저장, 취소 시 다시 활성화
        HomeBt.SetActive(false); // 저장 취소 시 다시 활성화
        BackupBt.SetActive(false); // 저장 취소 시 다시 활성화
        SaveBt.SetActive(false); // 저장 취소 시 다시 활성화
        saveData = "";
        foreach (Bricks _brick in BrickManager.brickList)
        {
            saveData += _brick.location.x + "_" + _brick.location.y + "_" + _brick.location.z + ":"
                + _brick.rotation.x + "_" + _brick.rotation.y + "_" + _brick.rotation.z + "_" + _brick.rotation.w + ":"
                + _brick.color + ":"
                + _brick.type + "&";
        } // saveData format = x_y_z:x_y_z_w:color:type&....
        saveData = saveData.Substring(0, saveData.Length - 1);
    }

    IEnumerator UploadPNG()
    {
        yield return new WaitForEndOfFrame();
        int width = Screen.width;
        int height = Screen.height;
        //var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        //tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        //tex.Apply();

        int margin = (int)width / 5;
        Texture2D tmpTex = new Texture2D(width - (2 * margin), height, TextureFormat.RGB24, false);
        tmpTex.ReadPixels(new Rect(margin, 0, width - (2 * margin), height), 0, 0);
        tmpTex.Apply();
        byte[] bytes = tmpTex.EncodeToPNG();
        //tmpAtlas.spriteMaterial.mainTexture = tmpTex;
        //tmpAtlas.spriteMaterial.mainTexture.name = saveTitle;
        UISprite sprite = tmpGameObj.GetComponent<UISprite>();
        //sprite.atlas.texture = tmpTex;
        //sprite.material.mainTexture = tmpTex;
        sprite.atlas = saveListAtlas;
        //tmpAtlas.GetComponent<UIAtlas>().spriteMaterial.mainTexture = tmpTex;
        //File.WriteAllBytes(Application.temporaryCachePath + "/" + saveTitle + ".png", bytes);
        //tmpTex.filterMode = FilterMode.Trilinear;
        //tmpTex.LoadImage(bytes);
        //Sprite sprite = Sprite.Create(tmpTex, new Rect(0, 0, tmpTex.width, tmpTex.height), new Vector2(0.5f, 0.5f));
        //sprite.atlas = saveListAtlas;
        //sprite.spriteName = saveTitle;
        Destroy(tmpTex);

        WWWForm form = new WWWForm();
        form.AddField("title", saveTitle);
        form.AddField("description", "test description");
        form.AddField("block_data", saveData);
        form.AddField("device", deviceInfo);
        form.AddBinaryData("fileUpload", bytes, "screenShot.png", "image/png");

        WWW w = new WWW(m_URL, form);
        yield return w;
        // check for errors
        if (w.error == null)
        {
            Debug.Log("WWW Ok!: " + w.text);
        }
        else
        {
            Debug.Log("WWW Error: " + w.error);
        }
    }

    void onSubmit()
    {
        saveTitle = editText.GetComponent<UIInput>().label.text;

        //tmp.Add("title", saveTitle);
        //tmp.Add("image_url", "22");
        //tmp.Add("description", "33");
        //tmp.Add("block_data", saveData);

        //POST("http://letsplock.cafe24.com/api/upload_title.jsp", tmp);
        StartCoroutine(UploadPNG());

        BinaryFormatter bin = new BinaryFormatter();
        if (SaveFileExist(saveTitle)) // 동일한 이름의 파일이 존재하면 삭제 (덮어쓰기 저장을 위해)
            DeleteSaveFile(saveTitle);
        sb.AppendFormat("{0}{1}{2}{3}", Application.persistentDataPath, '/', saveTitle, ".dat");
        FileStream file = File.Create(sb.ToString());

        sb.Length = 0;
        MyBrickList.myBrickinfo saveBrick = new MyBrickList.myBrickinfo(saveTitle, saveData);
        bin.Serialize(file, saveBrick); // 직렬화
        file.Close();

        editText.SetActive(false);
        okButton.SetActive(false);
        ListPanel.SetActive(true);
        ListSpriteDown.SetActive(true);
        ListSprite.SetActive(true);
        HomeBt.SetActive(true);
        BackupBt.SetActive(true);
        SaveBt.SetActive(true);
    }

    public static bool SaveFileExist(string name)
    {
        return File.Exists(GetSavePath(name));
    }

    private static string GetSavePath(string name)
    {
        return Path.Combine(Application.persistentDataPath, name + ".dat");
    }

    public static bool DeleteSaveFile(string name)
    {
        try
        {
            File.Delete(GetSavePath(name));
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}
