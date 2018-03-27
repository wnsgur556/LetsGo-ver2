using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ViewButton : MonoBehaviour {
    Camera mainCam = null;
    void Start()
    {
        mainCam = Camera.main;
        mainCam.GetComponentInParent<ControlView>().enabled = false;
    }

    void ViewControl()
    {
        GameObject NewLabel = GameObject.Find("VandPButtonLabel");
        NewLabel.GetComponent<UILabel>().text = "ViewMode";
        NewLabel.GetComponent<UILabel>().color = new Color(0f, 0f, 0f);
        mainCam.GetComponent<moveBlocks>().enabled = false;
        mainCam.GetComponentInParent<ControlView>().enabled = true;
        GameObject Create = GameObject.Find("BlockCreate");
        Create.GetComponent<BlockCreate>().enabled = false;
    }
}
