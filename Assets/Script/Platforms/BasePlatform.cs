using UnityEngine;
using System.Collections;

public class BasePlatform : MonoBehaviour {

	public enum Direction {
		TOP, DOWN, RIGHT, LEFT
	}
	public enum PlatformType {
		MOVE, ROTATION
	}

	protected void init() {
		f_StartPosition_X = transform.position.x;
		f_StartPosition_Y = transform.position.y;
		f_EndPosition_X = f_StartPosition_X + f_MoveDistance;
		f_EndPosition_Y = f_StartPosition_Y + f_MoveDistance;
	}

	// Main Parameters (Default)
	public float i_MoveSpeed = 5; // Move Speed
	public Direction m_Direction; // Direction
	public PlatformType m_PlatformType { get; set; } // Platform Type
	public float f_MoveDistance = 2; // 

	protected float f_StartPosition_X;
	protected float f_StartPosition_Y;
	protected float f_EndPosition_X;
	protected float f_EndPosition_Y;

	// Control Parameters
	public bool isShow;
	public bool isEntity;

}


