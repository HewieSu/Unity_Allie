using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
	/*—-GM(player)(one script)—-
	 *1.TaskControl(未接/進行/完成/交付)
	 *	Task-pool
	 *	-最多可追蹤X個任務進行狀態
	 *	在此遊戲中將player state綁至各npc上
	 *	GM將儲存物品和關卡進度
	 *
	 *2.Task UI system
	 *	Dialog
	 *	-對話框模組化(內容來自TaskScript)
	 *		1.stop moving
	 * 		2.show dialog box
	 * 		3.get txt
	 * 		4.show at ui
	 *	list
	 *	-清單UI顯示(內容來自TaskControl)
	 */
public class GM : MonoBehaviour {

	//loading textAsset & dialog
	public GameObject diaLogBox;//對話UI
	public Text diaLogText;//對話文字UI
	public bool canTalk;
	TextAsset textAsset;
	string currTxt;
	string[] currStrs;
	int i = 0;//目前句數
	//string[] strs;//外部讀取用
	//character
	private Player player;
	private Hunter hunter;
	private Wolf wolf;
	//swich curr npc let GM report to,let each npc refresh the state themself
	public string currNpcSpeakTo;//curr using dialogSystem NPC;
	public int hunterState;//task state in hunter,let npc load for
	public int wolfState;//task state in wolf,let npc load for
	//item
	public bool hunterHand;
	public bool wolfWild;
	public bool BearDoll;
	public bool boneToy;
	public bool sword;
	public bool gun;
	//UI
	public Slider sliderHp;
	public Image image_Sword;
	public Image image_Gun;
	//can get both item
	public bool canGetBothItem;
	//SceneObj
	//private GameObject wallToForest;
	void Start () {
		diaLogBox = GameObject.Find("Canvas/UICamera/Dialog box");
		diaLogText = GameObject.Find("Canvas/UICamera/Dialog box/Text").GetComponent<Text>();
		player = GameObject.Find("Allie").GetComponent<Player>();
		hunter = GameObject.Find ("Hunter").GetComponent<Hunter>();
		wolf = GameObject.Find("Wolf").GetComponent<Wolf>();
		diaLogBox.SetActive(false);
		image_Sword.enabled = false;
		image_Gun.enabled = false;
		/*外部讀取
		string path = Application.dataPath; //獲取預設抓資料的路徑
		int num = path.LastIndexOf("/"); //取得最後一個"/"出現的位置
		path = path.Substring(0,num); //取從0開始共num個字元>>將路徑最後一個資料夾移除
		string url = path + "/Test.txt"; //產生新的路徑
		strs = File.ReadAllLines(url); //抓取新路徑.ReadAllLines可將其轉為string[]
		*/
	}
	void Update () {
		//Dialog ();
		OpenToForest ();
		DieAndHp ();
		ItemUI();
	}

	//讀取對話並轉成文字陣列
	public void LoadStrs(string txtName){
		//內部讀取
		if(txtName != ""){
			textAsset = Resources.Load<TextAsset>("Dialog/" + txtName);//讀取Resources資料夾
			currTxt = textAsset.text;//將txt裝進currTxt
			currStrs = currTxt.Split('/');//用指定字元將字串切割成字串陣列
		}else{
			textAsset = null;//clean data
		}
	}
	/*//更新各npc任務狀態(進度)
	void RefreshState(string npcName){
		if(npcName == "hunter"){
			hunterState += 1;
			hunter.a = !hunter.a;//switch on
		}else{
			wolfState += 1;
			wolf.a = !wolf.a;//switch on
		}
	}*/
	//對話框模組
	public bool Dialog(){ // Test
		if(textAsset){
			//按下對話鍵(c)且目前句數i<總句數
			if(Input.GetKeyDown("c") && canTalk && i < currStrs.Length){;
				player.canMove = false;
				diaLogBox.SetActive(true);
				diaLogText.text = currStrs[i];
				i++;
				return false;
			}else if(Input.GetKeyDown("c") && i == currStrs.Length){//對話完畢將完成資訊回傳至該npc
				canTalk = false;
				player.canMove = true;
				diaLogBox.SetActive(false);
				//RefreshState(currNpcSpeakTo);
				i = 0;
				return true;
			}
		}
		return false;
	}
	void DieAndHp(){
		sliderHp.value = player.currHp / player.hp;
		if(player.currHp <= 0){
			Application.LoadLevel(0);
		}
	}
	void ItemUI(){
		if(sword){
			image_Sword.enabled = true;
		}
		if(gun){
			image_Gun.enabled = true;
		}
		if(hunterHand){

		}
		if(wolfWild){
			
		}
	}
	void BackPack(){

	}
	//h01_3結束後拿到水果刀&開啟往森林之路
	void OpenToForest(){
		GameObject wallToForest = GameObject.Find("Scene/WallToForest");
		if(hunterState == 3 && wallToForest){
			sword = true;
			Destroy(wallToForest);
		}
	}
}
