using UnityEngine;
using System.Collections;

public class ControlView : MonoBehaviour
{

    public float cameraSpeed = 1f;
    public float wheelSpeed = 5f;
    Vector3 mouseGap;
    public float distance = 10f;
    public bool invert = true;
    Camera mainCam;
    private bool moveIt;
    private Vector3 targetPos;

    void Start()
    {
        mainCam = Camera.main;
        mouseGap.x = 10f;
        this.transform.rotation = Quaternion.Euler(mouseGap);
    }

    void Update()
    {
        if (UICamera.hoveredObject == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f) && hit.collider.tag == "Block")
                {
                    targetPos = hit.collider.transform.position;
                    moveIt = true;
                }
            }
            if (moveIt)
            {
                mainCam.transform.parent.position = Vector3.Lerp(mainCam.transform.parent.position, targetPos, Time.deltaTime * 5f);
                float distance = Vector3.Distance(mainCam.transform.parent.position, targetPos);
                if (distance < 0.01f)
                    moveIt = false;
            }
        }
    }
    void LateUpdate()
    {
        Transform cam = Camera.main.transform;

        Vector3 tmp = this.transform.forward * -1;
        if (UICamera.hoveredObject == null)
        {
            if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                float touchDelta = 0.0f;
                Vector2 prevDist = new Vector2(0, 0);
                Vector2 curDist = new Vector2(0, 0);
                curDist = Input.GetTouch(0).position - Input.GetTouch(1).position;
                prevDist = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));
                touchDelta = curDist.magnitude - prevDist.magnitude;
                distance -= touchDelta;
                if (distance > 0)
                    tmp *= distance;
                else
                    distance = 0;
                cam.position = tmp + this.transform.position;
            }
            else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                mouseGap.y += Input.GetAxis("Mouse X") * cameraSpeed;
                if (invert)
                    mouseGap.x += Input.GetAxis("Mouse Y") * cameraSpeed;
                else
                    mouseGap.x -= Input.GetAxis("Mouse Y") * cameraSpeed;

                mouseGap.x = Mathf.Clamp(mouseGap.x, -50f, 50f);

                if (mouseGap.x >= 0)
                {
                    this.transform.rotation = Quaternion.Euler(mouseGap);
                }
                else if (mouseGap.x < 0)
                {
                    mouseGap.x = 0.1f;
                    this.transform.rotation = Quaternion.Euler(mouseGap);
                }
            }
        }
    }
}
