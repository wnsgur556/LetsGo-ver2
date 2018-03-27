using UnityEngine;
using System.Collections;

public class PageMove : MonoBehaviour {
    public GameObject leftNum, rightNum;

    private GameObject[] Panels;
    private UISprite leftNumUISprite, rightNumUISprite;
    private Vector3 oneSize, otherSize;
    private int count = 0, index = 0;
    void Start()
    {
        Panels = GameObject.FindGameObjectsWithTag("Panel");
        oneSize = new Vector3(8f, 11.8f, 0f);
        otherSize = new Vector3(14f, 11.8f, 0f);
        leftNumUISprite = leftNum.GetComponent<UISprite>();
        rightNumUISprite = rightNum.GetComponent<UISprite>();
        if (Panels.Length >= 2)
        {
            count = Panels.Length;
            Sort(); // 정렬
            leftNumUISprite.spriteName = "1"; // 첫 페이지 Number
            rightNumUISprite.spriteName = count.ToString(); // 끝 페이지 Number
            leftNum.transform.localScale = oneSize;
            for (int i = 1; i < count; i++)
            {
                Panels[i].SetActive(false);
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
    void LeftMove()
    {
        GlobalEffectSound.instance.playSound2();
        if (leftNumUISprite.spriteName == "1")
        {
            return;
        }
        else
        {
            Panels[index].SetActive(false);
            Panels[--index].SetActive(true);
            leftNumUISprite.spriteName = (index + 1).ToString();
        }
        if (leftNumUISprite.spriteName == "1")
            leftNum.transform.localScale = oneSize;
        else
            leftNum.transform.localScale = otherSize;
    }
    void RightMove()
    {
        GlobalEffectSound.instance.playSound2();
        if (leftNumUISprite.spriteName == count.ToString())
        {
            return;
        }
        else
        {
            Panels[index].SetActive(false);
            Panels[++index].SetActive(true);
            leftNumUISprite.spriteName = (index + 1).ToString();
        }
        if (leftNumUISprite.spriteName == "1")
            leftNum.transform.localScale = oneSize;
        else
            leftNum.transform.localScale = otherSize;
    }

    void Sort() // Page 정렬
    {
        int[] numArray = new int[count];
        int temp, length;
        GameObject tempObj;
        for (int i = 0; i < count; i++)
        {
            length = Panels[i].name.Length;
            numArray[i] = int.Parse(Panels[i].name.ToString().Substring(4, length - 4));
        }
        for (int i = 0; i < count; i++)
        {
            if (i == count - 1)
                break;
            for (int j = i + 1; j < count; j++)
            {
                if (numArray[i] > numArray[j])
                {
                    temp = numArray[i]; tempObj = Panels[i];
                    numArray[i] = numArray[j]; Panels[i] = Panels[j];
                    numArray[j] = temp; Panels[j] = tempObj;
                }
            }
        }
    }
}
