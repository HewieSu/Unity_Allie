using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	//hp
	public float hp = 100f;
	public float currHp;
	//move
	public bool canMove = true;
	private float moveCD;
	private float w;
	private float h;
	public float walkSpeed = 10f;
	public float RunSpeed = 20f;
	private bool facingRight = true;
	public float jump;
	private Rigidbody2D rigi;
	//atk
	public Rigidbody2D bullet;
	public GameObject sword;
	public float swordHit;
	public float bulletSpeed;
	public Transform firePoint;
	//hurt
	private bool getHurt = false;
	//groundedCheck
	private bool grounded;
	public Transform groundCheckPoint;
	public LayerMask whatIsGround;
	private float groundCheckRadius = 0.2f;
	//anim
	private Animator animator;
	void Start () {
		currHp = hp;
		rigi = GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
	}	
	void Update () {
		Move();
		Bullet();
		Att ();
		GroundedCheck();
	}
	//移動
	void Move(){
		if(canMove){
			//run
			if(Input.GetKey(KeyCode.LeftShift)){
				w = RunSpeed;
			}else{
				w = walkSpeed;
			}
			animator.SetBool("Run",Input.GetKey(KeyCode.LeftShift));
			//walk
			h = Input.GetAxis("Horizontal");
			transform.Translate(Vector3.right * Time.deltaTime * w * h);
			animator.SetFloat("Horizontal",Mathf.Abs(h));
			//jump
			if(Input.GetKeyDown("z") && grounded){
				//rigidbody2D.velocity = new Vector2(0,jump);
				Debug.Log("jump");
				//GetComponent<Rigidbody2D>().AddForce(transform.up * jump);
				rigi.AddForce(transform.up * jump);
			}
			//判斷是否轉向
			if(h > 0 && !facingRight){
				Flip();
			}else if(h < 0 && facingRight){
				Flip();
			}
			moveCD = 1f;
		}else if(getHurt){
			//移動關閉後如果受傷開始倒數
			Debug.Log("cant move");
			moveCD -= Time.deltaTime;
			if(moveCD <= 0){
				getHurt = false;
				canMove = true;
			}
		}
		//鎖住移動後h值也不會更新,故在animator多設一個canMove
		animator.SetBool ("CanMove",canMove);
	}
	//轉向
	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	//遠攻
	void Bullet(){
		if(Input.GetKeyDown("s")){
			Rigidbody2D clone = Instantiate(bullet,firePoint.position,bullet.transform.rotation) as Rigidbody2D;
			if(facingRight){
				clone.velocity = transform.TransformDirection(Vector3.right * bulletSpeed);
			}else{
				clone.velocity = transform.TransformDirection(Vector3.right * -bulletSpeed);
			}
		}
	}
	//近攻
	void Att(){
		if(Input.GetKey("x")){
			sword.SetActive(true);
		}else{
			sword.SetActive(false);
		}
	}
	//受傷
	public void Hurt(float hit){
		canMove = false;
		getHurt = true;
		GetComponent<Rigidbody2D>().AddForce(new Vector2(-400,200));
		currHp -= hit;
		Debug.Log("Allie's Hp=" + currHp);
	}
	//地面偵測(for Jump)
	void GroundedCheck(){
		//OverlapCircle:偵測是否有碰撞體在指定圓形內(中心點,半徑,過濾器(對何物做碰撞偵測))
		grounded = Physics2D.OverlapCircle(groundCheckPoint.position,groundCheckRadius,whatIsGround);
		animator.SetBool("Grounded",grounded);
	}
	//碰撞輔助線
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(groundCheckPoint.position,groundCheckRadius);
		//Gizmos.color = Color.red;
		//Gizmos.DrawWireSphere(dieCheck.position,dieRadius);
	}
}
