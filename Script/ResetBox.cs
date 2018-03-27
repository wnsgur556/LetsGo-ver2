using UnityEngine;
using System.Collections;

public class ResetBox : MonoBehaviour {
    public GameObject Reset;
    void ResetConfirm()
    {
        Reset.SetActive(true);
    }
}
