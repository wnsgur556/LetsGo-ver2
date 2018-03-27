using UnityEngine;
using System.Collections;

public class BlockReset : MonoBehaviour {
    private GameObject[] Block;
    private BlockInfo blockinfo;
    void Reset()
    {
        Block = GameObject.FindGameObjectsWithTag("Block");
        for (int i=0; i<Block.Length; i++)
        {
            Destroy(Block[i]);
        }
        GameObject ResetBox = GameObject.Find("ResetConfirm");
        ResetBox.SetActive(false);
        blockinfo = GameObject.Find("Main Camera").GetComponent<BlockInfo>();
        blockinfo.BlockNameInfo = "";
    }
}
