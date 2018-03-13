/************************************************************
How to use
	追従する側のObjectにattach.
	inspectorから、GO_Targetをset.

description
	TargetとなるGameObjectに、位置、姿勢、共に追従させるためのscript.
	
	pos
		TargetのLocal座標系で、同位置に来るよう処理。
		
	rot
		transform.rotationは、現在いる座標系でのMartix処理(つまりAction)であり、処理結果としての"姿勢"ではない。
		transform.forward/ up/ right が結果。
		
		故に、forwardについて、TargetのLocal座標系で同方向に来るよう処理。
		さて、これでforward方向は保持されるが、forward vector回りの回転はFixされていないので、姿勢が保持されない。
		(実際、forwardを法線ベクトルに持つ平面がクルクルと回転してしまう)
		
		そこで、up or rightについても、
			transform.forward	= GO_Target.transform.TransformDirection(TargetLocal_forward);
			transform.up		= GO_Target.transform.TransformDirection(TargetLocal_up);
		とすれば良いのかと思ったが、この処理だと、一番最後の処理のみを行ったのと同じになってしまうようだ。
		つまり、"transform.forward = ..."と処理した部分は、保持されない。
		
		そこで、まずはforwardが保持されるように処理を行った上で、
		予め保存されたup vectorと、forwardを揃えた後でのup vectorの成す角を算出し、
		これを打ち消すように"RotateAround"することで姿勢の保持をすることとした。

tips
	Mocapの場合、"PartsであるBoneそのもの"をtargetとして指定すること.

参考URL
	https://docs.unity3d.com/ja/540/ScriptReference/Transform.html
************************************************************/
using UnityEngine;
using System.Collections;

/************************************************************
************************************************************/

public class TraceTarget : MonoBehaviour {
	/****************************************
	****************************************/
	public GameObject GO_Target;
	
	private Vector3 TargetLocal_pos;
	private Vector3 TargetLocal_forward;
	private Vector3 TargetLocal_up;
	
	string label;
	
	/****************************************
	****************************************/
	void Start () {
		TargetLocal_pos		= GO_Target.transform.InverseTransformPoint(transform.position);
		TargetLocal_forward = GO_Target.transform.InverseTransformDirection(transform.forward);
		TargetLocal_up		= GO_Target.transform.InverseTransformDirection(transform.up);
	}
	
	// Update is called once per frame
	void Update () {
		/********************
		********************/
		transform.position	= Vector3.Lerp(transform.position, GO_Target.transform.TransformPoint(TargetLocal_pos), Time.deltaTime * 5.0f/* 200msで追いつくSpeed  */);
		transform.forward	= Vector3.Lerp(transform.forward, GO_Target.transform.TransformDirection(TargetLocal_forward), Time.deltaTime * 5.0f);
		
		float angle = Vector3.Angle(GO_Target.transform.InverseTransformDirection(transform.up), TargetLocal_up);
		transform.RotateAround(transform.position, transform.forward, -angle);
		
		/********************
		********************/
		/*
		transform.position	= GO_Target.transform.TransformPoint(TargetLocal_pos);
		
		transform.forward	= GO_Target.transform.TransformDirection(TargetLocal_forward);
		// transform.up		= GO_Target.transform.TransformDirection(TargetLocal_up); // これだと、上の行(forward)で行われた内容が上書きされてしまう.
		
		
		float angle = Vector3.Angle(GO_Target.transform.InverseTransformDirection(transform.up), TargetLocal_up);
		transform.RotateAround(transform.position, transform.forward, -angle);
		*/
		/*
		label =		string.Format("{0:0.000000} ,"	, angle);
		label +=	string.Format("{0:0.000000} ,"	, Vector3.Angle(transform.up, GO_Target.transform.TransformDirection(TargetLocal_up))); // 0 になっているか、check
		*/
	}
	
	void OnGUI()
	{
		GUI.color = Color.red;
		// GUI.Label(new Rect(15, 30, 500, 30), label);
	}
}
