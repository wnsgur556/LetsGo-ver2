using System.Text;
using System.Collections;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static int matchCount = 0;
    public static float dragXPoint;

    public GameObject[] animalMission, buildingMission, objectMission;
    public GameObject ListSpriteUp, ListSpriteDown, ListSprite, Grid;
    public Animator spriteAnim, panelAnim;

    private GameObject[] missionObject, brickIcon;
    private GameObject currentMission;
    private Vector3 oneSize, otherSize;
    private Transform currentIcon;
    private StringBuilder sb = new StringBuilder();
    private UISprite listUISprite;
    private string missionColor;
    private int missionClear, count;
    private bool isUp = true;

    void Start()
    {
        listUISprite = ListSprite.GetComponent<UISprite>();
        oneSize = new Vector3(0.08f, 0.2f, 0f);
        otherSize = new Vector3(0.12f, 0.2f, 0f);
        currentMission = Instantiate(animalMission[SceneMove.animalMissionNum]);
        missionObject = GameObject.FindGameObjectsWithTag("Mission"); // 미션 오브젝트들 - 동적
        brickIcon = GameObject.FindGameObjectsWithTag("BrickIcon"); // 블럭 버튼들 - 정적
        ListSpriteUp.SetActive(false);
        Sort(missionObject); Sort(brickIcon); // 정렬
        for (int i = 0; i < brickIcon.Length; i++)
            brickIcon[i].SetActive(false);
        for (int i = 0; i < missionObject.Length; i++)
        {
            for (int j = 0; j < brickIcon.Length; j++)
            {
                missionColor = missionObject[i].GetComponent<MeshRenderer>().material.name.Replace(" (Alpha) (Instance)", "");
                sb.AppendFormat("{0}{1}{2}{3}", missionObject[i].gameObject.name, '(', missionColor, ')');
                if (sb.ToString() == brickIcon[j].gameObject.name)
                {
                    brickIcon[j].SetActive(true);
                    currentIcon = brickIcon[j].transform.FindChild("Number");
                    if (currentIcon.GetComponent<UISprite>().spriteName == "x")
                    {
                        currentIcon.GetComponent<UISprite>().spriteName = "1";
                        currentIcon.transform.localScale = oneSize; // 크기 줄이기
                        break;
                    }
                    else
                    {
                        count = int.Parse(currentIcon.GetComponent<UISprite>().spriteName);
                        currentIcon.GetComponent<UISprite>().spriteName = (++count).ToString();
                        currentIcon.transform.localScale = otherSize; // 원래대로
                    }
                    break;
                }
                sb.Length = 0; // clear
            }
        }
        Grid.GetComponent<UIGrid>().Reposition();
        missionClear = currentMission.transform.childCount;
    }

    void Update()
    {
        if (missionClear != 0 && matchCount == missionClear)
        {
            GlobalEffectSound.instance.playMissionClear();
            missionClear = 0;
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

    void Sort(GameObject[] tmp)
    {
        int[] numArray = new int[tmp.Length];
        string[] split;
        int temp;
        GameObject tempObj;
        for (int i = 0; i < tmp.Length; i++)
        {
            split = tmp[i].gameObject.name.Split(']');
            numArray[i] = int.Parse(split[0]);
        }
        for (int i = 0; i < tmp.Length; i++)
        {
            if (i == tmp.Length - 1)
                break;
            for (int j = i + 1; j < tmp.Length; j++)
            {
                if (numArray[i] > numArray[j])
                {
                    temp = numArray[i]; tempObj = tmp[i];
                    numArray[i] = numArray[j]; tmp[i] = tmp[j];
                    numArray[j] = temp; tmp[j] = tempObj;
                }
            }
        }
    }

    IEnumerator waitSound()
    {
        yield return new WaitForSeconds(10.0f);
    }
}