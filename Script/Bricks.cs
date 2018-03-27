using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour {
    public Vector3 location;
    public Quaternion rotation;
    public string color; 
    public int type;
    public bool movement = false;

    private bool checkCollision = false;
    private bool moveUp = false;
    private BoxCollider sideCol = null;
    private List<BoxCollider> bottomCol = new List<BoxCollider>();
    private Vector3 lastMousePos;
    private Vector3 lastPos;
    private List<Bricks> missionCol = new List<Bricks>();
    private bool match = false;

    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        location = this.transform.position;
        rotation = this.transform.rotation;

        color = this.transform.GetComponent<Renderer>().material.name;
        string[] tmp = color.Split(' '); // ex) Material Format : Red (Instance) ...
        color = tmp[0];

        tmp = name.Split('('); // ex) Name Format : 2x2(Clone) or 2x2 ...
        string typeName = tmp[0];
        type = getBrickType(typeName);
        
        BrickManager.brickList.Add(this);
    }

    private int getBrickType(string name)
    {
        switch (name)
        {
            case "1]1x1": return 0;
            case "2]2x1": return 1;
            case "3]2x1_3": return 2;
            case "4]2x2": return 3;
            case "5]2x6": return 4;
            case "6]2x8": return 5;
            case "7]4x1": return 6;
            case "8]4x1_2": return 7;
            case "9]4x2": return 8;
            case "10]4x2_2": return 9;
            case "11]4x4_2": return 10;
            case "12]4x8_2": return 11;
            default: return -1; // Matching Fail
        }
    }

    private void FixedUpdate()  // Handling About Moving Bricks
    {
        if (missionCol.Count != 0 && !match)
        {
            foreach (Bricks goal in missionCol)
            {
                if (goal.location == this.transform.position)
                {
                    if (goal.type == this.type)
                    {
                        if (goal.color.Equals(this.color))
                        {
                            GlobalEffectSound.instance.playBrickClear();
                            match = true;
                            MissionManager.matchCount++;
                            // To-do : Rotation...
                        }
                    }
                }
            }
        }

        if (movement && !match)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                float posX, posY, posZ;
                posX = (7.9f) * Mathf.Round(hit.point.x / 7.9f);
                posZ = (7.9f) * Mathf.Round(hit.point.z / 7.9f);

                if (sideCol == null)
                {
                    posY = hit.point.y + (this.GetComponent<BoxCollider>().size.y / 2) + 1;
                }
                else
                {
                    posY = sideCol.transform.position.y + sideCol.size.y;
                }

                if (this.transform.position != new Vector3(posX, posY, posZ))
                    this.transform.position = new Vector3(posX, posY, posZ);
            }
        }
    }

    public void saveChange()
    {
        location = this.transform.position;
        rotation = this.transform.rotation;
        string[] tmp = this.transform.GetComponent<Renderer>().material.name.Split(' ');
        color = tmp[0];
    }

    void OnDestroy()
    {
        BrickManager.brickList.Remove(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            if (this.transform.position.y <= other.transform.position.y) // Collision on side (Bricks move up)
            {
                if (sideCol == null || sideCol.transform.position.y < other.transform.position.y)
                    sideCol = other.GetComponent<BoxCollider>();
            }
            else if (this.transform.position.y > other.transform.position.y)
            {
                if (this.GetComponent<BoxCollider>().size.y < 9) // Half Bricks
                {
                    if (sideCol == null || sideCol.transform.position.y < other.transform.position.y)
                        sideCol = other.GetComponent<BoxCollider>();
                }
                else // Another Bricks, Collision on bottom
                {
                    bottomCol.Add(other.GetComponent<BoxCollider>());
                }
            }
        }
        else if (other.CompareTag("Mission"))
        {
            missionCol.Add(other.GetComponent<Bricks>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Brick"))
        {
            if (sideCol == other.GetComponent<BoxCollider>())
            {
                sideCol = null;
            }
            else if (bottomCol.Contains(other.GetComponent<BoxCollider>()))
            {
                bottomCol.Remove(other.GetComponent<BoxCollider>());
            }
        }
        else if (other.CompareTag("Mission"))
        {
            missionCol.Remove(other.GetComponent<Bricks>());
        }
    }
}