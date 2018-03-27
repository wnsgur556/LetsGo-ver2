using UnityEngine;
using System.Collections;

public class TopicColor : MonoBehaviour {
	void Start () {
        GameObject Topic = GameObject.Find("Topic");
        Topic.GetComponent<UILabel>().color = Color.gray;
    }
}
