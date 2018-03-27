using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour {

    Camera mainCam;
    GameObject target = null;
    Component targetComponent;
    bool moveCam = false;

    private void Start()
    {
        mainCam = Camera.main;
    }

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            target = getTarget();
            if (target != null) // Bricks Movement
            {
                target.GetComponent<Bricks>().movement = true; // Send message to < Script "Bricks" > 
                target.layer = LayerMask.NameToLayer("Ignore Raycast"); // Don't perceive target Bricks
                // To-do : What color on selecting..?
            }
            else // Camera Rotation
            {
                moveCam = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(target != null) // Bricks Movement End
            {
                target.GetComponent<Bricks>().movement = false; // Send message to < Script "Bricks" > 
                target.layer = LayerMask.NameToLayer("Default"); // Rollback perceiving
                target.GetComponent<Bricks>().saveChange(); // save Lotation and Rotation data
                target = null;
            }
            else // Camera Rotation End
            {
                moveCam = false;
            }
        }
	}

    public float minX = -360.0f;
    public float maxX = 360.0f;

    public float minY = -45.0f;
    public float maxY = 45.0f;

    public float sensX = 10.0f;
    public float sensY = 10.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;

    /*private void LateUpdate() // Handling About Camera Rotation
    {
        if (moveCam)
        {
            rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
            mainCam.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
    } 카메라 이동 부분 */

    GameObject getTarget()
    {
        RaycastHit hit;
        GameObject target = null;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && hit.transform.tag == "Brick")
        {
            target = hit.transform.gameObject;
        }
        return target;
    }
}
