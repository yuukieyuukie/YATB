using UnityEngine;
using System.Collections;

public class MenuCam : MonoBehaviour {
	public Transform MenuPos;
	public Transform OptionsPos;
	private float overTime = 0.5f; //カメラ移動完了にかかる時間
	
	public void OptionsScreen()
	{
		StartCoroutine (MoveToOptions());
	
	}
	public void MenuScreen()
	{
		StartCoroutine (MoveToMenu());
	
	}
	
	private IEnumerator MoveToOptions()
	{
	
		Vector3 source = MenuPos.position;
		Vector3 target = OptionsPos.position;
		float startTime = Time.time;
		while(Time.time < startTime + overTime)
		{
	
			transform.position = Vector3.Lerp(source, target, (Time.time - startTime)/overTime);
			yield return new WaitForEndOfFrame();
		}
		transform.position = target;
	}
	
	private IEnumerator MoveToMenu()
	{
		Vector3 source = OptionsPos.position;
		Vector3 target = MenuPos.position;
		float startTime = Time.time;
		while(Time.time < startTime + overTime)
		{
			transform.position = Vector3.Lerp(source, target, (Time.time - startTime)/overTime);
			yield return new WaitForEndOfFrame();
		}
		transform.position = target;
	}
	
}
