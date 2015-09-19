using UnityEngine;
using System.Collections;

/* —-NPC—-(two script)
 * 
 * 1.NPC script(該NPC主程式)
 * -視觸發條件顯示該npc可接任務
 * -載入所接任務
 * 
 * 2.Task script(該任務主程式)
 * -load 該任務進度
 * -狀態改變時回報GM
 * -提供/回收 道具or獎勵
 * 
 * 在此遊戲中把npc & task做合併
 */
public class Hunter : MonoBehaviour {

	private Player player;
	private GM gm;
	private Wolf wolf;
	//setting
	public float hp;
	private float currHp;
	//lastAtkBy
	public string lastAtkWeapon;
	//flip
	private bool facingRight;
	//talkAcitve
	public float triggerDis;//觸發距離
	private GameObject talkbox;//對話圖示(小白框)
	public bool a = true;//避免重複刷新開關1.進入該npc範圍2.對話結束時 刷新一次
	//taskState
	public string currStateStr;//currstate string,change in npc
	public string lastStateStr;
	//Dialog switch
	bool DialogSwitch;
	bool AcptSwitch;//限定目前npc使用acpt(),防止其他npc呼叫對話系統,一個以上任務在同個npc則會有bug
	//AtkAI
	public float walkSpeed;
	public float nearAtkDis;
	public float cdTime;
	float currTime;
	Vector2 target;
	GameObject sword;
	bool getPlayerLoc;
	bool countDownOver;
	bool fightOn;

	void Start () {
		gm = GameObject.Find("Main Camera").GetComponent<GM>();
		player = GameObject.Find("Allie").GetComponent<Player>();
		wolf = GameObject.Find("Wolf").GetComponent<Wolf>();
		talkbox = GameObject.Find("Hunter/TalkBox");
		sword = GameObject.Find("Hunter/HunterSword");
		sword.SetActive (false);
		talkbox.SetActive(false);
		currHp = hp;
		currTime = cdTime;
	}

	void Update () {
		Acpt ();
		TalkActive();
		FilpSwitch ();
		AtkAI ();
	}
	//傳送目前對話進度至GM，離開範圍時清空
	void TalkActive(){
		bool inside = Mathf.Abs(transform.position.x - player.transform.position.x) <= triggerDis;
		//進入範圍時載入
		if(inside && a && gm.fighting == false){
			TaskState();
			//載入不為null(目前有對話)時
			if(currStateStr != null){
				talkbox.SetActive(true);
				gm.canTalk = true;
				gm.LoadStrs(currStateStr);//傳送文檔名稱
				Debug.Log(currStateStr + "  is loaded");
			}else{//目前無對話時自動進行清除
				talkbox.SetActive(false);
				gm.canTalk = false;
				gm.LoadStrs("");
			}
		}else if(!inside && !a){//沒進入範圍且a為false(防止影響其他npc)時自動進行清除,trun switch A on
			talkbox.SetActive(false);
			gm.canTalk = false;
			gm.LoadStrs("");
			a = true;
			AcptSwitch = false;
		}
	}
	//把主控轉回npc,點擊一次執行一次對話系統
	void Acpt(){
		if(Input.GetKeyDown("c") && AcptSwitch){
			gm.talking = true;
			//執行對話系統
			DialogSwitch = gm.Dialog();
			//當對話結束
			if(DialogSwitch){
				Debug.Log("dialog over");
				a =!a;
				AcptSwitch = false;
				lastStateStr = currStateStr;
				if(currStateStr == "h01"){
					gm.sword = true;
					gm.OpenToForest();
				}
				if(currStateStr == "h02"){
					Destroy(gameObject);
				}
				if(currStateStr == "h03"){
					gm.gun = true;
				}
				gm.talking = false;
			}
		}
	}
	//設定各任務階段npc所要loading的對話內容
	//設定目前對話npc(讓GM知道要回傳完成訊息的npc)
	void TaskState(){
		//switch off
		a = !a;
		AcptSwitch = true;
		if(currHp > 0){
			if(wolf){
				if(gm.suggest){
					currStateStr = "h03";
				}else{
					currStateStr = "h01";
				}
			}else{
				currStateStr = "h04";
			}
		}
		if(currStateStr == lastStateStr){
			currStateStr = null;
		}
	}
	//觸發範圍繪製
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, triggerDis);
		Gizmos.DrawWireSphere(transform.position, nearAtkDis);
	}
	void FilpSwitch(){
		if(Input.GetKeyDown("c") && AcptSwitch && player.transform.position.x > transform.position.x && !facingRight){
			Flip();
		}else if(Input.GetKeyDown("c") && AcptSwitch && player.transform.position.x < transform.position.x && facingRight){
			Flip();
		}
	}
	//轉向
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	//觸發戰鬥偵測
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.name == "Sword" || coll.name == "Bullet(Clone)" && !fightOn){
			gm.BattleModeSwitch(gameObject);
			fightOn = true;
		}
	}
	public void Hurt(float damage){
		currHp -= damage;
		Debug.Log ("hunter's HP = " + currHp);
		if(currHp <= 0){
			gm.BattleModeSwitch(gameObject);
			fightOn = false;
			gm.hunterHand = true;
			Debug.Log("get hunter's hand");
			currStateStr = "h02";
			if(lastAtkWeapon == "Gun"){
				gm.canGetBothItem = true;
			}
		}
	}
	void AtkAI(){
		if(fightOn){
			//CD clock
			if (currTime >= 0) {
				countDownOver = false;
				currTime -= Time.deltaTime;
			} else {
				countDownOver = true;
			}
			//decide act
			if(countDownOver){
				Move();
			}
		}
	}
	void GetPlayerLoc(){
		//get player position
		if(!getPlayerLoc){
			//flip or not
			if(transform.position.x - player.transform.position.x < 0 && !facingRight){
				nearAtkDis *= -1;
				Flip();
			}
			if(transform.position.x - player.transform.position.x > 0 && facingRight){
				Flip();
			}
			target = new Vector2 (player.transform.position.x , transform.position.y);
			getPlayerLoc = true;
		}
	}
	void Move(){
		GetPlayerLoc ();
		sword.SetActive (true);
		float Speed = walkSpeed * Time.deltaTime;
		transform.position = Vector2.MoveTowards (new Vector2(transform.position.x,transform.position.y) ,target,Speed);
		//when act is over
		if(transform.position.x == target.x){
			sword.SetActive(false);
			//reset cd
			currTime = cdTime;
			getPlayerLoc = false;
		}
	}
	void AISword(){

	}
	void AIGun(){

	}
	void AIBomb(){

	}
}
