using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

	private Transform player;
	void Start () {
		player = GameObject.Find("Allie").GetComponent<Transform>();
		GM gm = GetComponent<GM>();
	}
	void Update () {
		Follow();
	}
	void Follow(){
		if(player){
			transform.position = new Vector3(player.position.x,transform.position.y,transform.position.z);
		}else{
			Application.LoadLevel(0);
		}
	}
}
