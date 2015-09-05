using UnityEngine;
using System.Collections;

public class Enemy01 : MonoBehaviour {

	private Player player;
	public float HP = 50f;
	public float currHp;
	public bool atk = false;
	public GameObject atkSprite;
	private float atkTime = 4f;
	private float time;
	void Start () {
		player = GameObject.Find("Allie").GetComponent<Player>();
		Move ();
		currHp = HP;
		time = atkTime;
	}
	void Update () {
		if(time <= 2){
			atk = false;
			if(time <= 0){
				time = atkTime;
			}
		}else{
			atk = true;
		}
		time -= Time.deltaTime;

		Att();
	}
	void Move(){
		iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(7f, 1.46f, 0f), "easeType", "linear", "loopType", "pingPong", "delay", .1, "time", 3));
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.collider.tag == "Player"){
			player.Hurt(10f);
		}
		if (atk && coll.collider.name == "Sword"){
			Hurt(30f);
		}
		if(!atk && coll.collider.name == "Bullet(Clone)"){
			Hurt(10f);
		}
	}	
	//普/攻切換
	void Att(){
		if(atk){
			atkSprite.SetActive(true);
		}else{
			atkSprite.SetActive(false);
		}
	}
	//受傷&Die
	 public void Hurt(float hit){
		currHp -= hit;
		Debug.Log("moster01 = " + currHp);
		if(currHp <= 0){
			Destroy(gameObject);
		}
	}
}
