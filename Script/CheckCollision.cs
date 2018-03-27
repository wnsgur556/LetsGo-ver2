using UnityEngine;
using System.Collections;

public class CheckCollision : MonoBehaviour
{
    public bool isCollide;
    public float height;

    void Start()
    {
        height = 0f;
    }

    void OnTriggerEnter(Collider col)
    {
        height = col.gameObject.GetComponent<BoxCollider>().size.z + 0.01f;
        isCollide = true;
    }
    void OnTriggerExit(Collider coll)
     {
        height = 0f;
        isCollide = false;
     }
}