using UnityEngine;
using System.Collections;


public class Hunter2 : MonoBehaviour {

	private Player player;
	public float triggerDis;//觸發距離
	private GameObject talkbox;//對話圖示(小白框)
	private GM gm;
	bool a = true;//進出檢查>只讓load在進出時執行

	void Start () {
		gm = GameObject.Find("Main Camera").GetComponent<GM>();
		player = GameObject.Find("Allie").GetComponent<Player>();
		talkbox = GameObject.Find("Hunter2/TalkBox");
		talkbox.SetActive(false);
	}

	void Update () {
		TalkActive();
	}
	void TalkActive(){//傳送目前對話進度至GM，離開範圍時清空
		bool inside = Mathf.Abs(transform.position.x - player.transform.position.x) <= triggerDis;
		//進入觸發範圍
		if(inside && a){
			a = !a;
			talkbox.SetActive(true);
			gm.canTalk = true;
			gm.LoadStrs("test2");//傳送文檔名稱
		}else if(!inside && !a){
			a = !a;
			talkbox.SetActive(false);
			gm.canTalk = false;
			gm.LoadStrs("");
		}
	}
	//觸發範圍繪製
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, triggerDis);
	}

}
