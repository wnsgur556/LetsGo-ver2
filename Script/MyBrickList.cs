using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public class MyBrickList : MonoBehaviour {
    public GameObject mainPanel, pagePrefab, menuButton;
    public GameObject deleteBox, confirmBox, cancelBox, Left, Right, leftNum, rightNum;
    public Camera uiCam;

    private List<FileStream> fileList;
    private List<myBrickinfo> brickInfoList;
    private List<GameObject> pageList, buttonList;
    private Vector3 pagePosition, pageScale, btFirstPos, btSecondPos, btThirdPos, oneSize, otherSize;
    private int btCount, pageCount, i, j, k, n, currentPage, tmpNum = 0, index = 0;
    private BoxCollider[] currentBts;
    private StringBuilder sb = new StringBuilder();
    private UISprite leftNumUISprite, rightNumUISprite;

    [Serializable]
    public class myBrickinfo
    {
        public string title;
        public string blockData;
        //public byte[] screenShot;
        public myBrickinfo(string title, string blockData)
        {
            this.title = title;
            this.blockData = blockData;
            //this.screenShot = screenShot;
        }
        public myBrickinfo() { }
    }

    void Start () {
        settingList();
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                pageList.Clear();
                buttonList.Clear();
            }
        }

        if (StaticVarInMyList.onBox)
        {
            currentPage = Int32.Parse(leftNumUISprite.spriteName); // 보고 있는 Page 번호
            currentBts = pageList[currentPage - 1].gameObject.GetComponentsInChildren<BoxCollider>(); // 현재 Page 자식들의 BoxCollider 컴포넌트 배열
            for (k = 0; k < currentBts.Length; k++)
            {
                currentBts[k].enabled = false;
            }
            Left.GetComponent<BoxCollider>().enabled = false; // Page 좌측 이동 버튼 비활성화
            Right.GetComponent<BoxCollider>().enabled = false; // Page 우측 이동 버튼 비활성화
            StaticVarInMyList.onBox = false;
        }

    }

    private void settingList()
    {
        oneSize = new Vector3(8f, 11.8f, 0f);
        otherSize = new Vector3(14f, 11.8f, 0f);
        StaticVarInMyList.onBox = false;
        StaticVarInMyList.deleteObjName = "";
        btFirstPos = new Vector3(-105f, -10f, 0f);
        btSecondPos = new Vector3(105f, -10f, 0f);
        btThirdPos = new Vector3(315f, -10f, 0f);
        leftNumUISprite = leftNum.GetComponent<UISprite>();
        rightNumUISprite = rightNum.GetComponent<UISprite>();
        pagePosition = new Vector3(-105f, 0f, 0f);
        pageScale = new Vector3(1f, 1f, 1f);
        btCount = fileNumber(Application.persistentDataPath); // 저장한 파일 개수
        while (true)
        {
            if (btCount <= 3 * i) // 한 Page에 3개의 Button
            {
                pageCount = i;
                break;
            }
            i++;
        }

        pageList = new List<GameObject>(pageCount);
        buttonList = new List<GameObject>(btCount);
        brickInfoList = getMyBrickInfoList(Application.persistentDataPath); // 해당 경로에 있는 파일들 DeSerializable

        currentPage = Int32.Parse(leftNumUISprite.spriteName); // 보고 있는 Page 번호 (초기값 1)
        for (i = 0; i < pageCount; i++)
        {
            GameObject pageObj = Instantiate(pagePrefab, pagePosition, Quaternion.identity, mainPanel.transform) as GameObject; // Page 생성
            pageObj.transform.localPosition = pagePosition;
            pageObj.transform.localScale = pageScale;
            sb.AppendFormat("{0}{1}", "Page", (i + 1).ToString());
            pageObj.name = sb.ToString();
            pageList.Add(pageObj);
            for (j = 0; j < 3; j++)
            {
                GameObject buttonObj = pageList[i].transform.FindChild((j + 1).ToString()).gameObject;
                buttonObj.SetActive(true);
                buttonObj.GetComponent<BrickListDelete>().uiCam = uiCam;
                buttonObj.GetComponent<BrickListDelete>().deleteBox = deleteBox;
                buttonObj.GetComponent<BrickListDelete>().confirmBox = confirmBox;
                buttonObj.GetComponent<BrickListDelete>().cancelBox = cancelBox;
                buttonObj.name = brickInfoList[tmpNum].title;
                StartCoroutine(loadTexture(tmpNum, buttonObj)); // Image Load
                buttonList.Add(buttonObj);
                tmpNum++;
                if (tmpNum == btCount)
                {
                    int noObjNum = btCount % 3; // 0 일때는 그냥 break
                    if (noObjNum == 1)
                    {
                        Destroy(pageList[i].transform.FindChild(2.ToString()).gameObject);
                        Destroy(pageList[i].transform.FindChild(3.ToString()).gameObject);
                    }
                    else if (noObjNum == 2)
                    {
                        Destroy(pageList[i].transform.FindChild(3.ToString()).gameObject);
                    }
                    break;
                }
            }
            sb.Length = 0;
        }

        /* MyList Page Setting */
        if (pageList.Count >= 2)
        {
            leftNumUISprite.spriteName = "1"; // 첫 페이지 Number
            rightNumUISprite.spriteName = pageCount.ToString(); // 끝 페이지 Number
            leftNum.transform.localScale = oneSize;
            for (int i = 1; i < pageCount; i++)
            {
                pageList[i].SetActive(false);
            }
        }
        else
        {
            leftNumUISprite.spriteName = "1"; // 첫 페이지 Number
            rightNumUISprite.spriteName = "1"; // 끝 페이지 Number
            rightNum.transform.localScale = oneSize;
            leftNum.transform.localScale = oneSize;
        }
    }

    void inMyListLeftMove()
    {
        GlobalEffectSound.instance.playSound2();
        if (leftNumUISprite.spriteName == "1")
        {
            return;
        }
        else
        {
            pageList[index].SetActive(false);
            pageList[--index].SetActive(true);
            leftNumUISprite.spriteName = (index + 1).ToString();
        }
        if (leftNumUISprite.spriteName == "1")
            leftNum.transform.localScale = oneSize;
        else
            leftNum.transform.localScale = otherSize;
    }

    void inMyListRightMove()
    {
        GlobalEffectSound.instance.playSound2();
        if (leftNumUISprite.spriteName == pageCount.ToString())
        {
            return;
        }
        else
        {
            pageList[index].SetActive(false);
            pageList[++index].SetActive(true);
            leftNumUISprite.spriteName = (index + 1).ToString();
        }
        if (leftNumUISprite.spriteName == "1")
            leftNum.transform.localScale = oneSize;
        else
            leftNum.transform.localScale = otherSize;
    }

    private int fileNumber(string dirPath)
    {
        DirectoryInfo dir = new DirectoryInfo(dirPath);
        return dir.GetFiles().Length;
    }

    private List<myBrickinfo> getMyBrickInfoList(string dirPath)
    {
        DirectoryInfo dir = new DirectoryInfo(dirPath);
        FileInfo[] fileInfo = dir.GetFiles();
        myBrickinfo[] tmpBrickInfoArray = new myBrickinfo[btCount];
        fileInfo = getOrderedFileArray(fileInfo); // 수정 날짜 순 정렬
        BinaryFormatter bf = new BinaryFormatter();
        for (int m = 0; m < btCount; m++)
        {
            fileInfo[m].Name.Replace(".dat", ""); // file 이름 변경
            FileStream tmpFile = fileInfo[m].Open(FileMode.Open); // FileInfo -> FileStream
            tmpBrickInfoArray[m] = (myBrickinfo)bf.Deserialize(tmpFile); // FileStream -> myBrickinfo
            tmpFile.Close();
        }
        List<myBrickinfo> tmpBrickInfoList = new List<myBrickinfo>();
        tmpBrickInfoList.AddRange(tmpBrickInfoArray); // Array -> List 변환
        return tmpBrickInfoList;
    }

    private FileInfo[] getOrderedFileArray(FileInfo[] fileArray) // bubble sort
    {
        FileInfo temp;
        for (int a = 0; a < fileArray.Length; a++)
        {
            for (int b = a + 1; b < fileArray.Length; b++)
            {
                if (DateTime.Compare(fileArray[a].LastWriteTime, fileArray[b].LastWriteTime) < 0)
                {
                    temp = fileArray[a];
                    fileArray[a] = fileArray[b];
                    fileArray[b] = temp;
                }
            }
        }
        return fileArray;
    }

    void goDelete() // 삭제 시작
    {
        StaticVarInMyList.second = 0; // 초기화

        deleteBox.SetActive(false);
        confirmBox.SetActive(false);
        cancelBox.SetActive(false);

        for (k = 0; k < currentBts.Length; k++)
        {
            currentBts[k].enabled = true;
        }

        /* 삭제 작업 */

        /* 삭제할 버튼 오브젝트의 위치 기억 */
        GameObject deleteObj = GameObject.Find(StaticVarInMyList.deleteObjName); // 삭제할 GameObject
        float x = deleteObj.transform.localPosition.x; // 삭제할 GameObject의 x 좌표
        int okCount;

        /* 위치에 따라 삭제 방식이 다름 */
        if (x == -105) // First
        {
            okCount = (currentPage - 1) * 3 + 1;
            if (currentPage == pageCount) // 마지막 Page
            {
                int tmpPage = currentPage - 1;
                int moveCount = 0;
                for (int tmpButton = tmpPage * 3 + 1; tmpButton != btCount; tmpButton++)
                {
                    if (moveCount == 0)
                        buttonList[tmpButton].transform.localPosition = btFirstPos;
                    else
                        buttonList[tmpButton].transform.localPosition = btSecondPos;
                    moveCount++;
                }
            }
            else
            {
                for (int tmpPage = currentPage - 1; tmpPage < pageCount; tmpPage++) // 현재 보고 있는 페이지 번호부터 마지막 페이지까지
                {
                    /* 위치 변경 */
                    int moveCount = 0;
                    if (okCount == btCount)
                        break;
                    for (int tmpButton = tmpPage * 3 + 1; tmpButton < tmpPage * 3 + 4; tmpButton++)
                    {
                        if (okCount == btCount)
                            break;
                        if (moveCount % 3 == 0)
                        {
                            buttonList[tmpButton].transform.localPosition = btFirstPos;
                            okCount++;
                        }
                        else if (moveCount % 3 == 1)
                        {
                            buttonList[tmpButton].transform.localPosition = btSecondPos;
                            okCount++;
                        }
                        else
                        {
                            buttonList[tmpButton].transform.SetParent(pageList[tmpPage].transform); // 다음 Page 자식을 현재 보고 있는 Page의 자식으로 이동
                            buttonList[tmpButton].transform.localPosition = btThirdPos;
                            okCount++;
                        }
                        moveCount++;
                    }
                }
            }
        }
        else if (x == 105) // Second
        {
            okCount = (currentPage - 1) * 3 + 2;
            if (currentPage == pageCount) // 마지막 Page
            {
                int tmpPage = currentPage - 1;
                for (int tmpButton = tmpPage * 3 + 2; tmpButton != btCount; tmpButton++)
                {
                    buttonList[tmpButton].transform.localPosition = btSecondPos;
                }
            }
            else
            {
                for (int tmpPage = currentPage - 1; tmpPage < pageCount; tmpPage++) // 현재 보고 있는 페이지 번호부터 마지막 페이지까지
                {
                    /* 위치 변경 */
                    int moveCount = 0;
                    if (okCount == btCount)
                        break;
                    for (int tmpButton = tmpPage * 3 + 2; tmpButton < tmpPage * 3 + 5; tmpButton++)
                    {
                        if (okCount == btCount)
                            break;
                        if (moveCount % 3 == 0)
                        {
                            buttonList[tmpButton].transform.localPosition = btSecondPos;
                            okCount++;
                        }
                        else if (moveCount % 3 == 1)
                        {
                            buttonList[tmpButton].transform.SetParent(pageList[tmpPage].transform); // 다음 Page 자식을 현재 보고 있는 Page의 자식으로 이동
                            buttonList[tmpButton].transform.localPosition = btThirdPos;
                            okCount++;
                        }
                        else
                        {
                            buttonList[tmpButton].transform.localPosition = btFirstPos;
                            okCount++;
                        }
                        moveCount++;
                    }
                }
            }
        }
        else if (x == 315) // Third
        {
            okCount = (currentPage - 1) * 3 + 3;
            for (int tmpPage = currentPage - 1; tmpPage < pageCount; tmpPage++) // 현재 보고 있는 페이지 번호부터 마지막 페이지까지
            {
                /* 위치 변경 */
                int moveCount = 0;
                if (okCount == btCount)
                    break;
                for (int tmpButton = tmpPage * 3 + 3; tmpButton < tmpPage * 3 + 6; tmpButton++)
                {
                    if (tmpButton == btCount)
                        break;
                    if (moveCount % 3 == 0)
                    {
                        buttonList[tmpButton].transform.SetParent(pageList[tmpPage].transform); // 다음 Page 자식을 현재 보고 있는 Page의 자식으로 이동
                        buttonList[tmpButton].transform.localPosition = btThirdPos;
                        okCount++;
                    }
                    else if (moveCount % 3 == 1)
                    {
                        buttonList[tmpButton].transform.localPosition = btFirstPos;
                        okCount++;
                    }
                    else
                    {
                        buttonList[tmpButton].transform.localPosition = btSecondPos;
                        okCount++;
                    }
                    moveCount++;
                }
            }
        }

        /* Button 삭제 */
        buttonList.Remove(deleteObj);
        Destroy(deleteObj);
        btCount--;

        /* File 삭제 */
        BrickSave.DeleteSaveFile(StaticVarInMyList.deleteObjName);
        //fileName.Remove(StaticVarInMyList.deleteObjName);

        if (btCount % 3 == 0) // 버튼의 개수가 3으로 나누어 떨어지면 마지막 Page 삭제
        {
            if (currentPage == pageCount && currentPage != 1)
                pageList[--index].SetActive(true);
            GameObject deletePage = pageList[pageCount - 1];
            pageList.Remove(deletePage);
            Destroy(deletePage);
            if (leftNumUISprite.spriteName == rightNumUISprite.spriteName)
            {
                if (rightNumUISprite.spriteName == "1")
                {
                    leftNum.transform.localScale = oneSize;
                    rightNum.transform.localScale = oneSize;
                }
                else
                {
                    leftNumUISprite.spriteName = (pageCount - 1).ToString();
                    rightNumUISprite.spriteName = (pageCount - 1).ToString();
                    if (rightNumUISprite.spriteName == "1")
                    {
                        leftNum.transform.localScale = oneSize;
                        rightNum.transform.localScale = oneSize;
                    }
                    else
                    {
                        leftNum.transform.localScale = otherSize;
                        rightNum.transform.localScale = otherSize;
                    }
                }
            }
            else
            {
                rightNumUISprite.spriteName = (pageCount - 1).ToString();
                if (rightNumUISprite.spriteName == "1")
                    rightNum.transform.localScale = oneSize;
                else
                    rightNum.transform.localScale = otherSize;
            }
            pageCount--;
        }

        Left.GetComponent<BoxCollider>().enabled = true; // Page 좌측 이동 버튼 활성화
        Right.GetComponent<BoxCollider>().enabled = true; // Page 우측 이동 버튼 활성화
        StaticVarInMyList.deleteObjName = ""; // 초기화
    }

    void noDelete() // 삭제 취소
    {
        StaticVarInMyList.second = 0; // 초기화

        deleteBox.SetActive(false);
        confirmBox.SetActive(false);
        cancelBox.SetActive(false);

        for (k = 0; k < currentBts.Length; k++)
        {
            currentBts[k].enabled = true;
        }

        Left.GetComponent<BoxCollider>().enabled = true; // Page 좌측 이동 버튼 활성화
        Right.GetComponent<BoxCollider>().enabled = true; // Page 우측 이동 버튼 활성화
        StaticVarInMyList.deleteObjName = ""; // 초기화
    }

    IEnumerator loadTexture(int number, GameObject button)
    {
        yield return new WaitForEndOfFrame();
        int width = Screen.width;
        int height = Screen.height;
        int margin = (int)width / 5;
        Texture2D texture = new Texture2D(width - (2 * margin), height, TextureFormat.RGB24, false);
        texture.filterMode = FilterMode.Trilinear;
        //texture.LoadImage(brickInfoList[number].screenShot);
        GameObject btBackGround = button.transform.FindChild("Background" + (j + 1)).gameObject;
        btBackGround.GetComponent<UITexture>().material.mainTexture = texture;
        //btBackGround.GetComponent<UISprite>().spriteName = 
    }
}
