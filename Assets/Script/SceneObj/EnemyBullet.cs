using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

	public Player player;
	void Start () {
		player = GameObject.Find("Allie").GetComponent<Player>();
		Destroy(gameObject,5);
	}
	void Update () {
	
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.tag == "Player"){
			player.Hurt(20f);
			Destroy(gameObject);
		}
		Destroy(gameObject);
	}
}
