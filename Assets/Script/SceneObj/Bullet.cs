using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	/* 受傷觸發架構
	 * 整體思路:將傷害量(bullet)帶至受傷方(target),又或是受傷方做偵測此碰撞是否有傷害,不論何種皆需做對象的判定
	 * 1.傷害計算是寫在受傷方身上
	 * 2.一對一(單體技能)思路:
	 * bullet打至不同對象時呼叫各對象的傷害計算函式,但需要把傷害計算函式單獨拉出成一個component(script)
	 * (因為需統一名稱,如不單獨拉出只能先行定義路徑,經由碰撞對象再寫code去訂要送傷害給誰...就變成一對象一路徑,十對象十路徑...囧)
	 * 在碰撞時直接抓取該對象的傷害計算script並輸入傷害量
	 * 3.一對多(AOE技能)思路:
	 * 確認範圍內對象,送出傷害量給各個對象(回到一對一)
	 */
	public float bltDamage;
	private Hunter hunter;
	private Wolf wolf;

	void Start () {
		hunter = GameObject.Find("Hunter").GetComponent<Hunter>();
		wolf = GameObject.Find("Wolf").GetComponent<Wolf>();
		Destroy (gameObject, 2.5f);
	}
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D coll){
		string id = coll.gameObject.name;
		if(id == "Hunter"){
			hunter.Hurt (bltDamage);
			hunter.lastAtkWeapon = "Gun";
		}
		if(id == "Wolf"){
			wolf.Hurt(bltDamage);
		}
		Debug.Log ("bullet hit = " + id);
		Debug.Log ("hit by " + coll.name);
		Destroy(gameObject);
	}
	void OnCollisionEnter2D(Collision2D coll) {


	}
}
