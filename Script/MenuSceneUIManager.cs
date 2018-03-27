using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneUIManager : MonoBehaviour {
    public float width, height;
    public GameObject MainBackground, SetMode, Logo, FreeMode, MissionMode, MyList;
    public GameObject SetBg, LogoBg, FreeBg, MissionBg, ListBg;

    private Vector3 buttonSize;
    void Start () {
        /*width = Screen.width;
        height = Screen.height;
        MainBackground.transform.localScale = new Vector3(width, height, 0f);
        SetMode.GetComponent<BoxCollider>().size = new Vector3(width * 0.057f, height * 0.091f, 0f);
        SetBg.transform.localScale = new Vector3(width * 0.057f, height * 0.091f, 0f);
        LogoBg.transform.localScale = new Vector3(width * 0.295f, height * 0.5625f, 0f);
        buttonSize = new Vector3(width * 0.3846f, height * 0.1875f, 0f);
        FreeMode.GetComponent<BoxCollider>().size = buttonSize;
        FreeBg.transform.localScale = buttonSize;
        MissionMode.GetComponent<BoxCollider>().size = buttonSize;
        MissionBg.transform.localScale = buttonSize;
        MyList.GetComponent<BoxCollider>().size = buttonSize;
        ListBg.transform.localScale = buttonSize;*/
    }
}
