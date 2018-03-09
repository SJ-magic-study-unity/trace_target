/************************************************************
How to use
	追従する側のObjectにattach.
	inspectorから、GO_Targetをset.

description
	TargetとなるGameObjectに、位置、姿勢、共に追従させるためのscript.
	
		private Vector3 pos;
		private Vector3 forward;
	は、それぞれ、GO_TargetのLocal座標系でのposとforward vectorを保持する.
		InverseTransformPoint()
		InverseTransformDirection()
	は、それぞれ、Global -> Localの変換関数.
	
	Update()では、posとforwardに保持されたGO_TargetのLocal座標系値を
	Global値に戻し、transformにset.

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
	
	private Vector3 pos;
	private Vector3 forward;
	
	
	/****************************************
	****************************************/
	void Start () {
		pos = GO_Target.transform.InverseTransformPoint(transform.position);
		forward = GO_Target.transform.InverseTransformDirection(transform.forward);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position	= Vector3.Lerp(transform.position, GO_Target.transform.TransformPoint(pos), Time.deltaTime * 5.0f/* 200msで追いつくSpeed  */);
		transform.forward	= Vector3.Lerp(transform.forward, GO_Target.transform.TransformDirection(forward), Time.deltaTime * 5.0f);
		
		/*
		transform.position	= GO_Target.transform.TransformPoint(pos);
		transform.forward	= GO_Target.transform.TransformDirection(relative);
		*/
	}
}
