using UnityEngine;
using System.Collections;

public class Enemy02 : MonoBehaviour {

	private Player player;
	public float HP = 70f;
	public float currHp;
	public Rigidbody2D enemyBullet;
	public float bulletSpeed;
	public Transform firePoint;
	private bool fire = true;
	public float fireDis;
	public float fireCD;
	private float time;

	void Start () {
		player = GameObject.Find("Allie").GetComponent<Player>();
		currHp = HP;
		time = fireCD;
	}
	void Update () {
		if(time <= 0 ){
			fire = true;
		}

		time -= Time.deltaTime;

		if(fire){
			Fire();
		}
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.tag == "Player"){
			player.Hurt(10f);
		}
		if (coll.collider.name == "Sword"){
			Hurt(30f);	
		}
	}
	void Fire(){
		fire = false;
		time = fireCD;
		Rigidbody2D clone = Instantiate(enemyBullet,firePoint.position,enemyBullet.transform.rotation) as Rigidbody2D;
		clone.velocity = transform.TransformDirection(Vector3.left * bulletSpeed);
	}
	//受傷&Die
	public void Hurt(float hit){
		currHp -= hit;
		Debug.Log("moster02 = " + currHp);
		if(currHp <= 0){
			Destroy(gameObject);
		}
	}
}
