using UnityEngine;
using System.Collections;

public class moveBlocks : MonoBehaviour {
    Camera mainCam = null;
    private GameObject target;
    private GameObject tmp;
    private bool mouseState;
    private Vector3 lastMousePos;
    private bool Menu, Delete = true;
    private bool ColorSel = false;
    private BlockInfo blockinfo;
    private ScreenView sv;
    void Start ()
    {
        mainCam = Camera.main;
        blockinfo = GameObject.Find("Main Camera").GetComponent<BlockInfo>();
        sv = GameObject.Find("Main Camera").GetComponent<ScreenView>();
    }
	
	void Update () {
        if (UICamera.hoveredObject == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                target = getClickedObject();
                if (target != null)
                {
                    mouseState = true;
                    Menu = true;
                    Delete = true;
                    if (ColorSel)
                    {
                        ColorSel = false;
                    }
                    target.GetComponent<BoxCollider>().isTrigger = true;
                }
                else if (target == null)
                {
                    Menu = false;
                    Delete = false;
                    ColorSel = false;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mouseState = false;
                if (target != null)
                {
                    target.GetComponent<BoxCollider>().isTrigger = false;
                    target.transform.position = target.transform.parent.transform.position;
                }
                else if (target == null)
                {
                    Menu = false;
                    Delete = false;
                    ColorSel = false;
                }
            }

            if (mouseState)
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                Vector3 movedPos;
                if (Physics.Raycast(ray, out hit, 1000f) && lastMousePos != Input.mousePosition)
                {
                    if (hit.collider.tag == "Lego")
                    {
                        movedPos = new Vector3(Mathf.RoundToInt(hit.point.x), 0, Mathf.RoundToInt(hit.point.z));
                    }
                    else
                    {
                        movedPos = new Vector3(Mathf.RoundToInt(hit.point.x), hit.collider.gameObject.transform.position.y + hit.collider.gameObject.GetComponent<BoxCollider>().size.z + 0.01f, Mathf.RoundToInt(hit.point.z));
                    }
                    if (target.GetComponent<CheckCollision>().isCollide)
                    {
                        target.transform.position = movedPos + new Vector3(0, 0, target.GetComponent<CheckCollision>().height);
                        target.transform.parent.position = target.transform.position + new Vector3(0, target.GetComponent<CheckCollision>().height, -target.GetComponent<CheckCollision>().height);
                        if (target.tag == "Block")
                        {
                            tmp = target.transform.parent.gameObject;
                        }
                    }
                    else
                    {
                        target.transform.position = movedPos;
                        target.transform.parent.position = movedPos;
                        if (target.tag == "Block")
                        {
                            tmp = target.transform.parent.gameObject;
                        }
                    }
                }
            }
        }
    }
    void LateUpdate()
    {
        lastMousePos = Input.mousePosition;
    }
    private GameObject getClickedObject()
    {
        RaycastHit hit;
        GameObject target = null;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit) && hit.collider.tag.Equals("Block"))
            target = hit.collider.gameObject;
        return target;
    }
    void OnGUI()
    {
        GUIStyle boxstyle = GUI.skin.GetStyle("box");
        GUIStyle buttonstyle = GUI.skin.GetStyle("button");
        boxstyle.fontSize = (int) (sv.width / 20.57f);
        buttonstyle.fontSize = (int) (sv.width / 41.14f);
        if (Menu)
        {
            if (Delete)
            {
                GUI.Box(new Rect(sv.width / 51.43f, sv.height / 1.28f, sv.width / 1.82f, sv.height / 12.8f), "Menu");
                if (GUI.Button(new Rect(sv.width / 51.43f, sv.height / 1.22f, sv.width / 9.6f, sv.height / 25.6f), "Right"))
                {
                    target.transform.Rotate(0f, 0f, 90f);
                    tmp.transform.Rotate(0f, 0f, 90f);
                }
                if (GUI.Button(new Rect(sv.width / 7.66f, sv.height / 1.22f, sv.width / 9.6f, sv.height / 25.6f), "Left"))
                {
                    target.transform.Rotate(0f, 0f, -90f);
                    tmp.transform.Rotate(0f, 0f, -90f);
                }
                if (GUI.Button(new Rect(sv.width / 4.138f, sv.height / 1.22f, sv.width / 9.6f, sv.height / 25.6f), "Color"))
                {
                    Menu = false;
                    ColorSel = true;
                }
                if (GUI.Button(new Rect(sv.width / 2.835f, sv.height / 1.22f, sv.width / 9.6f, sv.height / 25.6f), "Delete"))
                {
                    Destroy(target);
                    Destroy(tmp);
                    Delete = false;
                    if (tmp.name == "Block1(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("12"), 3);
                    }
                    else if (tmp.name == "Block2(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("1"), 2);
                    }
                    else if (tmp.name == "Block3(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("4"), 2);
                    }
                    else if (tmp.name == "Block4(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("2"), 2);
                    }
                    else if (tmp.name == "Block5(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("7"), 2);
                    }
                    else if (tmp.name == "Block6(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("8"), 2);
                    }
                    else if (tmp.name == "Block7(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("5"), 2);
                    }
                    else if (tmp.name == "Block8(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("11"), 3);
                    }
                    else if (tmp.name == "Block9(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("3"), 2);
                    }
                    else if (tmp.name == "Block10(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("6"), 2);
                    }
                    else if (tmp.name == "Block11(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("9"), 2);
                    }
                    else if (tmp.name == "Block12(Clone)")
                    {
                        blockinfo.BlockNameInfo = blockinfo.BlockNameInfo.Remove(blockinfo.BlockNameInfo.IndexOf("10"), 3);
                    }
                }
                if (GUI.Button(new Rect(sv.width / 2.155f, sv.height / 1.22f, sv.width / 9.6f, sv.height / 25.6f), "Exit"))
                {
                    Menu = false;
                }
            }
        }
        if (ColorSel)
        {
            GUI.Box(new Rect(sv.width / 51.43f, sv.height / 1.28f, sv.width / 1.82f, sv.height / 12.8f), "Color");
            if (GUI.Button(new Rect(sv.width / 51.43f, sv.height / 1.22f, sv.width / 9.6f, sv.height / 25.6f), "Gray"))
            {
                Material Gray = Resources.Load("BlockColor", typeof(Material)) as Material;
                target.GetComponent<Renderer>().material = Gray;
                tmp.GetComponent<Renderer>().material = Gray;
                Menu = false;
            }
            if (GUI.Button(new Rect(sv.width / 7.66f, sv.height / 1.22f, sv.width / 9.6f, sv.height / 25.6f), "Brown"))
            {
                Material Brown = Resources.Load("BlockColor2(Brown)", typeof(Material)) as Material;
                target.GetComponent<Renderer>().material = Brown;
                tmp.GetComponent<Renderer>().material = Brown;
                Menu = false;
            }
            if (GUI.Button(new Rect(sv.width / 4.138f, sv.height / 1.22f, sv.width / 9.6f, sv.height / 25.6f), "Black"))
            {
                Material Black = Resources.Load("BlockColor4(Black)", typeof(Material)) as Material;
                target.GetComponent<Renderer>().material = Black;
                tmp.GetComponent<Renderer>().material = Black;
                Menu = false;
            }
            if (GUI.Button(new Rect(sv.width / 2.835f, sv.height / 1.22f, sv.width / 9.6f, sv.height / 25.6f), "Ivory"))
            {
                Material Ivory = Resources.Load("BlockColor3(Ivory)", typeof(Material)) as Material;
                target.GetComponent<Renderer>().material = Ivory;
                tmp.GetComponent<Renderer>().material = Ivory;
                Menu = false;
            }
            if (GUI.Button(new Rect(sv.width / 2.155f, sv.height / 1.22f, sv.width / 9.6f, sv.height / 25.6f), "Back"))
            {
                ColorSel = false;
                Menu = true;
            }
        }
    }
}
