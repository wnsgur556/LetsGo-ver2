using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrickListDelete : MonoBehaviour {
    public Camera uiCam;
    public GameObject deleteBox, confirmBox, cancelBox;

    private GameObject target;

    void Start()
    {
        target = null;
        StaticVarInMyList.second = 0;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0) // Press...
        {
            target = GetClickedObject();
            if (target != null)
            {
                if (target.name != "Left" && target.name != "Right" && target.name != "ConfirmBox" && target.name != "CancelBox") // List 목록 누를때만 시간 계산
                {
                    StaticVarInMyList.second += Time.deltaTime; // 누르고 있는 시간 계산
                    if (StaticVarInMyList.second >= 1.0)  // 1.0초 동안 누르고 있었다면
                    {
                        delete();
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) || Input.touchCount == 0) // Up!
        {
            if (StaticVarInMyList.second > 0 && StaticVarInMyList.second < 1.0) // 1.0초 동안 누르고 있지 않았다면
            {
                brickLoad(); // Load 실행
            }
        }
    }

    void delete()
    {
        StaticVarInMyList.deleteObjName = target.name;
        deleteBox.SetActive(true);
        confirmBox.SetActive(true);
        cancelBox.SetActive(true);
        StaticVarInMyList.onBox = true;
    }

    void brickLoad()
    {
        StaticVarInMyList.second = 0; // 초기화
        SimulationManager.loadData = target.name; // 클릭한 GameObject 이름 전달
        GlobalEffectSound.instance.playSound2();
        MainScreenView.lastScene.Push("MyListMenu_Scene");
        SceneManager.LoadScene("Simulation_Scene");
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
