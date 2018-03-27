using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayButton : MonoBehaviour {
    Camera mainCam = null;
    void Start()
    {
        mainCam = Camera.main;
    }
	void PlayControl () {
        GameObject NewLabel = GameObject.Find("VandPButtonLabel");
        NewLabel.GetComponent<UILabel>().text = "PlayMode";
        NewLabel.GetComponent<UILabel>().color = new Color(0f, 0f, 0f);
        mainCam.GetComponent<moveBlocks>().enabled = true;
        mainCam.GetComponentInParent<ControlView>().enabled = false;
        GameObject Create = GameObject.Find("BlockCreate");
        Create.GetComponent<BlockCreate>().enabled = true;
    }
}
