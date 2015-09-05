using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {

	private Transform player;
	public bool Following;
	void Start () {
		player = GameObject.Find("Allie").GetComponent<Transform>();
		GM gm = GetComponent<GM>();
	}
	void Update () {
		if(Following){
			Follow();
		}
	}
	public void Follow(){
		if(player){
			transform.position = new Vector3(player.position.x,transform.position.y,transform.position.z);
		}else{
			Application.LoadLevel(0);
		}
	}
}
