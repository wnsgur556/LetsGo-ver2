using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour {
    public static int animalMissionNum, buildingMissionNum, objectMissionNum;
    public Camera uiCam;

    private GameObject target, btObject, Grid;
    private Transform btObjectNum;
    private string objectName;

    void freeClick()
    {
        MainScreenView.lastScene.Push("Menu_Scene");
        GlobalEffectSound.instance.playSound();
        SceneManager.LoadScene("Simulation_Scene");
    }

    void missionClick()
    {
        MainScreenView.lastScene.Push("Menu_Scene");
        GlobalEffectSound.instance.playSound();
        SceneManager.LoadScene("MissionMenu_Scene");
    }

    void myListClick()
    {
        MainScreenView.lastScene.Push("Menu_Scene");
        GlobalEffectSound.instance.playSound();
        SceneManager.LoadScene("MyListMenu_Scene");
    }

    void setClick()
    {
        if (GameObject.Find("FreeMode"))
        {
            MainScreenView.lastScene.Push("Menu_Scene");
        }
        else if (GameObject.Find("AnimalButton"))
        {
            MainScreenView.lastScene.Push("MissionMenu_Scene");
        }
        GlobalEffectSound.instance.playSound();
        SceneManager.LoadScene("Set_Scene");
    }

    void backClick()
    {
        GlobalEffectSound.instance.playSound();
        SceneManager.LoadScene(MainScreenView.lastScene.Pop());
    }

    void animalClick()
    {
        MainScreenView.lastScene.Push("MissionMenu_Scene");
        GlobalEffectSound.instance.playSound();
        SceneManager.LoadScene("AnimalMenu_Scene");
    }

    void buildingClick()
    {
        MainScreenView.lastScene.Push("MissionMenu_Scene");
        GlobalEffectSound.instance.playSound();
        SceneManager.LoadScene("BuildingMenu_Scene");
    }

    void objectClick()
    {
        MainScreenView.lastScene.Push("MissionMenu_Scene");
        GlobalEffectSound.instance.playSound();
        SceneManager.LoadScene("ObjectMenu_Scene");
    }

    void missionStartClick()
    {
        target = GetClickedObject(); // 미션모드 선택한 오브젝트 이름 얻기
        if (GameObject.Find("AnimalMenu"))
        {
            if (target.name == "Fox") animalMissionNum = 0;
            else if (target.name == "Koala") animalMissionNum = 1;
            else if (target.name == "Pig") animalMissionNum = 2;
            else if (target.name == "Bull") animalMissionNum = 3;
            else if (target.name == "Crab") animalMissionNum = 4;
            else if (target.name == "Chick") animalMissionNum = 5;
            else if (target.name == "Tiger") animalMissionNum = 6;
            else if (target.name == "Whale") animalMissionNum = 7;
            else if (target.name == "Lemur") animalMissionNum = 8;
            else if (target.name == "Zebra") animalMissionNum = 9;
            else if (target.name == "Hedgehog") animalMissionNum = 10;
            else if (target.name == "Hippo") animalMissionNum = 11;
            MainScreenView.lastScene.Push("AnimalMenu_Scene");
        }
        else if (GameObject.Find("BuildingMenu"))
        {
            MainScreenView.lastScene.Push("BuildingMenu_Scene");
        }
        else if (GameObject.Find("ObjectMenu"))
        {
            MainScreenView.lastScene.Push("ObjectMenu_Scene");
        }
        GlobalEffectSound.instance.playSound();
        SceneManager.LoadScene("MissionMode_Scene");
    }

    void homeClick()
    {
        GlobalEffectSound.instance.playSound2();
        MainScreenView.lastScene.Clear();
        BrickManager.createBricks.Clear();
        BrickManager.brickList.Clear();
        MissionManager.matchCount = 0;
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
            addBricksNumber(backUp);
            Destroy(backUp);
        }
    }

    /*void brickLoadClick()
    {
        target = GetClickedObject(); // 클릭한 GameObject
        SimulationManager.loadData = target.name; // 클릭한 GameObject 이름 전달
        GlobalEffectSound.instance.playSound2();
        MainScreenView.lastScene.Push("MyListMenu_Scene");
        SceneManager.LoadScene("Simulation_Scene");
    }*/

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

    void addBricksNumber(GameObject inListBrick) // When 'backUpButton' Click
    {
        Grid = GameObject.Find("Grid");
        string inListBrickColor = inListBrick.GetComponent<MeshRenderer>().material.name.Replace(" (Instance)", "");
        objectName = inListBrick.gameObject.name.ToString().Replace("(Clone)", '(' + inListBrickColor + ')');
        btObject = GameObject.Find(objectName); // 부모 찾기
        if (btObject == null) // inListBrick에 해당하는 버튼이 setActive(false) 라면
        {
            Grid.transform.FindChild(objectName).gameObject.SetActive(true);
            Grid.GetComponent<UIGrid>().Reposition();
            BrickCreate.brickIcons = GameObject.FindGameObjectsWithTag("BrickIcon"); // static 변수로 버튼 개수 갱신
        }
        else
        {
            btObjectNum = btObject.transform.FindChild("Number"); // 자식 찾기
            int count = int.Parse(btObjectNum.GetComponent<UISprite>().spriteName);
            btObjectNum.GetComponent<UISprite>().spriteName = (++count).ToString();
            btObjectNum.transform.localScale = new Vector3(0.12f, 0.2f, 0f); // 원래대로
        }
        if (MissionManager.matchCount > 0)
            MissionManager.matchCount--;
    }
}
