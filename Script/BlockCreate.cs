using UnityEngine;
using System.Collections;

public class BlockCreate : MonoBehaviour {
    public GameObject Block1, Block2, Block3, Block4, Block5, Block6, Block7, Block8, Block9, Block10, Block11, Block12;
    private GameObject BlockPrefab1, BlockPrefab2, BlockPrefab3, BlockPrefab4, BlockPrefab5, BlockPrefab6, BlockPrefab7, BlockPrefab8, BlockPrefab9, BlockPrefab10, BlockPrefab11, BlockPrefab12;
    private Vector3 mousePos, targetPos;
    private bool Move;
    private string BlockName;
    private BlockInfo blockinfo;
    private Camera mainCam = null;
    void Start()
    {
        mainCam = Camera.main;
        blockinfo = GameObject.Find("Main Camera").GetComponent<BlockInfo>();
    }
    void Update()
    {
        if (BlockPrefab1 && (BlockName == "12_"))
        {
            LegoCreate(BlockPrefab1);
        }
        if (BlockPrefab2 && (BlockName == "1_"))
        {
            LegoCreate(BlockPrefab2);
        }
        if (BlockPrefab3 && (BlockName == "4_"))
        {
            LegoCreate(BlockPrefab3);
        }
        if (BlockPrefab4 && (BlockName == "2_"))
        {
            LegoCreate(BlockPrefab4);
        }
        if (BlockPrefab5 && (BlockName == "7_"))
        {
            LegoCreate(BlockPrefab5);
        }
        if (BlockPrefab6 && (BlockName == "8_"))
        {
            LegoCreate(BlockPrefab6);
        }
        if (BlockPrefab7 && (BlockName == "5_"))
        {
            LegoCreate(BlockPrefab7);
        }
        if (BlockPrefab8 && (BlockName == "11_"))
        {
            LegoCreate(BlockPrefab8);
        }
        if (BlockPrefab9 && (BlockName == "3_"))
        {
            LegoCreate(BlockPrefab9);
        }
        if (BlockPrefab10 && (BlockName == "6_"))
        {
            LegoCreate(BlockPrefab10);
        }
        if (BlockPrefab11 && (BlockName == "9_"))
        {
            LegoCreate(BlockPrefab11);
        }
        if (BlockPrefab12 && (BlockName == "10_"))
        {
            LegoCreate(BlockPrefab12);
        }
    }
    void LegoCreate(GameObject Block) // 블럭 생성 함수
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
                if (Physics.Raycast(ray, out hit, 10000f) && Move)
                {
                    if (UICamera.hoveredObject == null)
                    {
                        if (hit.collider.gameObject.tag == "Lego")
                        {
                            targetPos = hit.point;
                            Block.transform.position = new Vector3((int)targetPos.x, targetPos.y, (int)targetPos.z);
                            Block.SetActive(true);
                            Move = false;
                            blockinfo.BlockNameInfo += BlockName;
                            mainCam.GetComponent<moveBlocks>().enabled = true;
                            //mainCam.GetComponent<TabMoveBlocks>().enabled = true;
                            //mainCam.GetComponent<TabMoveBlocks2>().enabled = true;
                        }
                        else if (hit.collider.gameObject.tag == "Block")
                        {
                            targetPos = hit.point;
                            Block.transform.position = new Vector3((int)targetPos.x, targetPos.y, (int)targetPos.z);
                            Block.SetActive(true);
                            Move = false;
                            blockinfo.BlockNameInfo += BlockName;
                            mainCam.GetComponent<moveBlocks>().enabled = true;
                            //mainCam.GetComponent<TabMoveBlocks>().enabled = true;
                            //mainCam.GetComponent<TabMoveBlocks2>().enabled = true;
                        }
                    }
                }
            }
        }
    }
    public void OnClickStart1()
    {
        BlockPrefab1 = Instantiate(Block1, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab1.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab1.SetActive(false);
        BlockName = "12_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
    public void OnClickStart2()
    {
        BlockPrefab2 = Instantiate(Block2, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab2.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab2.SetActive(false);
        BlockName = "1_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
    public void OnClickStart3()
    {
        BlockPrefab3 = Instantiate(Block3, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab3.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab3.SetActive(false);
        BlockName = "4_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
    public void OnClickStart4()
    {
        BlockPrefab4 = Instantiate(Block4, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab4.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab4.SetActive(false);
        BlockName = "2_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
    public void OnClickStart5()
    {
        BlockPrefab5 = Instantiate(Block5, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab5.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab5.SetActive(false);
        BlockName = "7_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
    public void OnClickStart6()
    {
        BlockPrefab6 = Instantiate(Block6, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab6.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab6.SetActive(false);
        BlockName = "8_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
    public void OnClickStart7()
    {
        BlockPrefab7 = Instantiate(Block7, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab7.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab7.SetActive(false);
        BlockName = "5_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
    public void OnClickStart8()
    {
        BlockPrefab8 = Instantiate(Block8, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab8.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab8.SetActive(false);
        BlockName = "11_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
    public void OnClickStart9()
    {
        BlockPrefab9 = Instantiate(Block9, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab9.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab9.SetActive(false);
        BlockName = "3_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
    public void OnClickStart10()
    {
        BlockPrefab10 = Instantiate(Block10, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab10.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab10.SetActive(false);
        BlockName = "6_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
    public void OnClickStart11()
    {
        BlockPrefab11 = Instantiate(Block11, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab11.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab11.SetActive(false);
        BlockName = "9_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
    public void OnClickStart12()
    {
        BlockPrefab12 = Instantiate(Block12, transform.position, Quaternion.identity) as GameObject;
        BlockPrefab12.transform.position = new Vector3(25.0f, 0.0f, 25.0f); // 임의의 위치
        BlockPrefab12.SetActive(false);
        BlockName = "10_";
        Move = true;
        mainCam.GetComponent<moveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks>().enabled = false;
        //mainCam.GetComponent<TabMoveBlocks2>().enabled = false;
    }
}