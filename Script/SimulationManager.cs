using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationManager : MonoBehaviour {
    public static string loadData;

    public GameObject[] prefabs;
    public Camera uiCam;
    public GameObject ListSpriteUp, ListSpriteDown, ListSprite;
    public Animator spriteAnim, panelAnim;

    private StringBuilder sb = new StringBuilder();
    private GameObject tmpPrefab, target;
    private Vector3 mousePos;
    private string[] tmp;
    private bool create = false, isUp = true;
    private UISprite listUISprite;

    void Start()
    {
        listUISprite = ListSprite.GetComponent<UISprite>();
        ListSpriteUp.SetActive(false);
        if (MainScreenView.lastScene.Peek() == "MyListMenu_Scene") // 전 Scene이 MyListMenu_Scene이면
        {
            if (!BrickSave.SaveFileExist(loadData)) // 파일이 존재하지 않으면
            {
                Debug.Log("존재하지 않는 파일입니다.");
            }
            else
            {
                BinaryFormatter bin = new BinaryFormatter();
                sb.AppendFormat("{0}{1}{2}{3}", Application.persistentDataPath, '/', loadData, ".dat");
                FileStream file = File.Open(sb.ToString(), FileMode.Open);
                if (file != null && file.Length > 0)
                {
                    MyBrickList.myBrickinfo data = (MyBrickList.myBrickinfo)bin.Deserialize(file); // 역직렬화
                    string[] tmpLoad = data.blockData.Split('&');
                    for (int i = 0; i < tmpLoad.Length; i++)
                    {
                        string[] property = tmpLoad[i].Split(':');
                        // 0 : Location / 1 : Rotation / 2 : Color / 3 : Type

                        string[] savedLocation = property[0].Split('_');
                        Vector3 location = new Vector3(float.Parse(savedLocation[0]), float.Parse(savedLocation[1]), float.Parse(savedLocation[2]));
                        string[] savedRocation = property[1].Split('_');
                        Quaternion rotation = new Quaternion(float.Parse(savedRocation[0]), float.Parse(savedRocation[1]), float.Parse(savedRocation[2]), float.Parse(savedRocation[3]));
                        int type = int.Parse(property[3]);

                        GameObject bricks = (GameObject)Instantiate(BrickManager.getBrick(type), location, rotation);
                        bricks.GetComponent<Renderer>().material = BrickManager.getColor(property[2]);
                    }
                }
                file.Close();
            }
        }
        sb.Length = 0;
    }

    void Update()
    {
        target = GetClickedObject(); // 클릭한 오브젝트 이름
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

    void upList()
    {
        GlobalEffectSound.instance.playSound();
        listUISprite.spriteName = "블럭모음_열림";
        spriteAnim.SetBool("isUp", !isUp);
        panelAnim.SetBool("isUp", !isUp);
        ListSpriteUp.SetActive(false);
        ListSpriteDown.SetActive(true);
        isUp = !isUp;
    }

    void downList()
    {
        GlobalEffectSound.instance.playSound();
        listUISprite.spriteName = "블럭모음_닫힘";
        spriteAnim.SetBool("isUp", !isUp);
        panelAnim.SetBool("isUp", !isUp);
        ListSpriteDown.SetActive(false);
        ListSpriteUp.SetActive(true);
        isUp = !isUp;
    }

    void homeClick()
    {
        GlobalEffectSound.instance.playSound2();
        MainScreenView.lastScene.Clear();
        BrickManager.createBricks.Clear();
        BrickManager.brickList.Clear();
        SceneManager.LoadScene("Menu_Scene");
    }

    void backupClick()
    {
        GlobalEffectSound.instance.playSound2();
        if (BrickManager.createBricks.Count == 0)
            return;
        else
        {
            GameObject backUp = BrickManager.createBricks.Pop();
            Destroy(backUp);
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
}
