using UnityEngine;
using System.Collections;

public class ResetCancel : MonoBehaviour {
    void Cancel()
    {
        GameObject ResetBox = GameObject.Find("ResetConfirm");
        ResetBox.SetActive(false);
    }
}
