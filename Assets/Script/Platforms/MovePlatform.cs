using UnityEngine;
using System.Collections;

public class MovePlatform : BasePlatform {

	public MovePlatform(PlatformType type) {
		this.m_PlatformType = type;
	}
	
	void Awake() {
		init();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		switch (this.m_Direction) {
			case Direction.TOP:
				transform.position += Vector3.up * i_MoveSpeed * Time.deltaTime;
				if(transform.position.y >= f_EndPosition_Y) {
					changeDirection(Direction.DOWN);
				}
				break;
			case Direction.DOWN:
				transform.position -= Vector3.up * i_MoveSpeed * Time.deltaTime;
				if(transform.position.y <= f_StartPosition_Y) {
					changeDirection(Direction.TOP);
				}
				break;
			case Direction.LEFT:
				break;
			case Direction.RIGHT:
				break;
		}
	}
	
	// Change Direction
	public void changeDirection(Direction newDirection) {
		this.m_Direction = newDirection;
	}
}
