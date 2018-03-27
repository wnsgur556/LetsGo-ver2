using UnityEngine;

public class BrickCreate : MonoBehaviour
{
    public static GameObject[] brickIcons;

    public GameObject[] prefabs;
    public GameObject Grid, List;
    public Camera uiCam;

    private GameObject tmpPrefab, target, btObj;
    private Transform btObjNum;
    private Vector3 mousePos, listPos, oneSize, otherSize;
    private Vector4 listClipRange;
    private UIPanel listUIPanel;
    private UIDraggablePanel listUIDragPanel;
    private UIGrid gridUI;
    private string objName;
    private string[] tmp;
    private int count, startNum;
    private bool create = false;

    void Start()
    {
        settingList();
    }

    void Update()
    {
        target = GetClickedObject(); // 클릭한 오브젝트 이름
        if (brickIcons.Length <= 6)  // 버튼이 6개 이하면 Drag X
        {
            List.transform.localPosition = listPos;
            listUIPanel.clipRange = listClipRange;
            listUIDragPanel.enabled = false;
            if (startNum >= 6 && List.GetComponent<SpringPanel>() != null)
                List.GetComponent<SpringPanel>().enabled = false;
        }
        else
            listUIDragPanel.enabled = true;
        if (tmpPrefab != null && create)
        {
            bricksCreate(tmpPrefab);
        }
    }

    void createClick()
    {
        GlobalEffectSound.instance.playSound2();
        if (target != null)
        {
            if (tmpPrefab != null && tmpPrefab.activeSelf == false)
            {
                Destroy(tmpPrefab);
            }
            tmp = target.name.Split('('); // tmp[0] = 4]2x2, tmp[1] = Red)...
            tmp[1] = tmp[1].Substring(0, tmp[1].LastIndexOf(')'));
            for (int i = 0; i < prefabs.Length; i++)
            {
                if (prefabs[i].gameObject.name == tmp[0])
                {
                    tmpPrefab = Instantiate(prefabs[i], transform.position, Quaternion.identity) as GameObject;
                    tmpPrefab.SetActive(false);
                    create = true;
                    break;
                }
                else
                    continue;
            }
        }
        else
            return;
    }

    void bricksCreate(GameObject tmpPrefab)
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount == 1)
        {
            mousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0) || Input.touchCount == 1)
        {
            if (mousePos == Input.mousePosition)
            {
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && create)
                {
                    if (UICamera.hoveredObject == null)
                    {
                        if (hit.collider.gameObject.CompareTag("Field"))
                        {
                            tmpPrefab.transform.position = new Vector3((7.9f) * Mathf.Round(hit.point.x / 7.9f),
                                0, (7.9f) * Mathf.Round(hit.point.z / 7.9f));
                            tmpPrefab.SetActive(true);
                            tmpPrefab.GetComponent<MeshRenderer>().material = BrickManager.getColor(tmp[1]);
                            BrickManager.createBricks.Push(tmpPrefab); // Push
                            create = false;
                            subBricksNumber();
                        }
                        /*else if (hit.collider.gameObject.CompareTag("Brick"))
                        {
                            tmpPrefab.transform.position = new Vector3((7.9f) * Mathf.Round(hit.point.x / 7.9f),
                                moveCol.transform.position.y + moveCol.size.y, (7.9f) * Mathf.Round(hit.point.z / 7.9f));
                            tmpPrefab.SetActive(true);
                        } 바로 위에 쌓아올릴 때 */
                        else if (hit.collider.gameObject.CompareTag("OutSide") && tmpPrefab.activeSelf == false)
                            Destroy(tmpPrefab);
                    }
                }
            }
        }
    }

    void subBricksNumber()
    {
        objName = tmpPrefab.gameObject.name.ToString().Replace("(Clone)", '(' + tmp[1] + ')');
        btObj = GameObject.Find(objName); // 부모 찾기
        btObjNum = btObj.transform.FindChild("Number"); // 자식 찾기
        if (btObjNum.GetComponent<UISprite>().spriteName == "1")
        {
            btObj.SetActive(false);
            gridUI.Reposition();
            brickIcons = GameObject.FindGameObjectsWithTag("BrickIcon");
        }
        else
        {
            if (btObjNum.GetComponent<UISprite>().spriteName == "2")
            {
                btObjNum.transform.localScale = oneSize; // '1' 크기 조정
                btObjNum.GetComponent<UISprite>().spriteName = "1";
            }
            else
            {
                count = int.Parse(btObjNum.GetComponent<UISprite>().spriteName);
                btObjNum.GetComponent<UISprite>().spriteName = (--count).ToString();
                btObjNum.transform.localScale = otherSize; // 원래대로
            }
        }
    }
    private GameObject GetClickedObject()
    {
        RaycastHit hit;
        GameObject target = null;
        Ray ray = uiCam.ScreenPointToRay(Input.mousePosition);
        if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }

    private void settingList()
    {
        oneSize = new Vector3(0.08f, 0.2f, 0f);
        otherSize = new Vector3(0.12f, 0.2f, 0f);
        listPos = new Vector3(-55f, -128f, 1f);
        listClipRange = new Vector4(60f, 0f, 780f, 50f);
        listUIPanel = List.GetComponent<UIPanel>();
        listUIDragPanel = List.GetComponent<UIDraggablePanel>();
        gridUI = Grid.GetComponent<UIGrid>();
        brickIcons = GameObject.FindGameObjectsWithTag("BrickIcon");
        startNum = brickIcons.Length;
    }

    /*private Color ToColor(string color) // String -> Color
    {
        return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
    }*/
}