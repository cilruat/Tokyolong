using UnityEngine;
using System.Collections;

namespace BSK
{
//This script is used to work with transform component in any cases. Within this game it is used to animate UI text. Such as balls or score count perform scale "pushes" when you have a goal.
public class Transformer : MonoBehaviour {
	
	private float p_lerpTime, r_lerpTime, s_lerpTime;
	private float p_currentLerpTime, r_currentLerpTime, s_currentLerpTime;
	private bool needMove, needRotate, needScale, reverse, pingPong;
	private Vector3 fromPos, toPos, fromScale, toScale;
	private int s_Count;
	private int s_currentCount;
	private Quaternion toRot;
	
	public void Move(Vector3 targetPos, float time){
		p_lerpTime = time;
		fromPos = transform.position;
		toPos = targetPos;
		needMove = true;
		p_currentLerpTime = 0;
	}
	
	public void Rotate(Vector3 rotAngle, float time){
		if(!needRotate)
			toRot = transform.rotation;
		toRot.eulerAngles += rotAngle;
		needRotate = true;
		r_lerpTime = time;
		r_currentLerpTime = 0;
	}
	
	public void Scale(Vector3 targetScale, float time, bool pingPong){
		s_lerpTime = time;
		fromScale = transform.localScale;
		toScale = targetScale;
		needScale = true;
		reverse = false;
		this.pingPong = pingPong;
		s_currentLerpTime = 0;
	}
	
	public void ScaleImpulse(Vector3 targetScale, float time, int count){
		s_lerpTime = time;
		fromScale = transform.localScale;
		toScale = targetScale;
		needScale = true;
		reverse = false;
		s_Count = count;
		s_currentCount = 0;
		s_currentLerpTime = 0;
	}
	
	public void StopMove(){
		needMove = false;
	}
	
	void FixedUpdate () {
		if(needMove)
			updatePosition();
		if(needRotate)
			updateRotation();
		if(needScale)
			updateScale();
	}
	
	void updatePosition(){
		p_currentLerpTime += Time.deltaTime;
		if(p_currentLerpTime > p_lerpTime)
			p_currentLerpTime = p_lerpTime;
		
		float t = p_currentLerpTime/p_lerpTime;
		t = Mathf.Sin(t * Mathf.PI * 0.5f);
		transform.position = Vector3.Lerp(fromPos, toPos, t);
		if(t >= 1)
			needMove = false;
			
	}
	
	void updateRotation(){
		r_currentLerpTime += Time.deltaTime;
		if(r_currentLerpTime > r_lerpTime)
			r_currentLerpTime = r_lerpTime;
			
		float t = r_currentLerpTime/r_lerpTime;
		t = Mathf.Sin(t * Mathf.PI * 0.5f);
		
		transform.rotation = Quaternion.Slerp(transform.rotation, toRot, t);
		if(t == 1)
			needRotate = false;
	}
	
	void updateScale(){
		s_currentLerpTime += !reverse ? Time.deltaTime : -Time.deltaTime;
		if(s_currentLerpTime > s_lerpTime)
			s_currentLerpTime = s_lerpTime;
		
		float t = s_currentLerpTime/s_lerpTime;

		transform.localScale = Vector3.Lerp(fromScale, toScale, t);
		if(t >= 1 || t <= 0) {
			if(t <= 0)
				s_currentCount += 1;
			if(pingPong || s_currentCount < s_Count){
				reverse = !reverse;
			} else {
				needScale = false;
			}
		}
			
	}
}
}