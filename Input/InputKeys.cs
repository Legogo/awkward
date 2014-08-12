using UnityEngine;
using System.Collections;

public class InputKeys : MonoBehaviour {
	
	static protected KeyCode up = KeyCode.Z;
	static protected KeyCode US_up = KeyCode.W;
	static protected KeyCode down = KeyCode.S;
	static protected KeyCode left = KeyCode.Q;
	static protected KeyCode US_left = KeyCode.A;
	static protected KeyCode right = KeyCode.D;
	
	static protected KeyCode alt_up = KeyCode.UpArrow;
	static protected KeyCode alt_down = KeyCode.DownArrow;
	static protected KeyCode alt_left = KeyCode.LeftArrow;
	static protected KeyCode alt_right = KeyCode.RightArrow;
	
	static public bool key_up = false;
	static public bool key_down = false;
	static public bool key_left = false;
	static public bool key_right = false;
	
	static public bool key_space = false;
	static public bool key_shift = false;
	static public bool key_ctrl = false;
	static public bool key_alt = false;
  static public bool key_e = false;
	
	void Update () {
		//if(Input.GetAxis("Horizontal"))	key_right = true;
		key_left = Input.GetKey(left) || Input.GetKey(US_left) || Input.GetKey(alt_left);
		key_right = Input.GetKey(right) || Input.GetKey(alt_right);
		
		key_up = Input.GetKey(up) || Input.GetKey(US_up) || Input.GetKey(alt_up);
		key_down = Input.GetKey(down) || Input.GetKey(alt_down);
		
		key_space = Input.GetKey(KeyCode.Space);
		key_shift = Input.GetKey(KeyCode.LeftShift);
		key_ctrl = Input.GetKey(KeyCode.LeftControl);
		key_alt = Input.GetKey(KeyCode.LeftAlt);
    key_e = Input.GetKey(KeyCode.E);
	}
}
