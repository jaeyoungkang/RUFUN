namespace MoenenVoxel {

using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {


	public static Transform CameraShaker;
	private static float CameraSize;

	private Transform Player {
		get {
			return DemoStage.Main.CurrentPlayer;
		}
	}
	
	private const float gap = 1f;


	void Awake () {
		Camera.main.orthographic = true;
		CameraShaker = Camera.main.transform.parent;
		CameraSize = Camera.main.orthographicSize;
	}
	
	
	void Update () {
		// Shaker
		CameraShaker.localPosition *= -0.8f;
		Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, CameraSize, 0.15f);
		// Holder
		if (Player) {
			Vector3 pos = Player.localPosition;
			Vector2 stageSize = DemoStage.Main.MainGround.localScale;
			stageSize.x -= gap;
			stageSize.y -= gap;
			stageSize.x -= 2f * Camera.main.orthographicSize * Camera.main.aspect;
			float r = Mathf.Sin(Camera.main.transform.rotation.eulerAngles.x * Mathf.Deg2Rad);
			stageSize.y -= 2f * Camera.main.orthographicSize / r;
			stageSize.x = Mathf.Max(stageSize.x, 0f);
			pos.x = Mathf.Clamp(pos.x, -stageSize.x * 0.5f, stageSize.x * 0.5f);
			pos.z = Mathf.Clamp(pos.z, -stageSize.y * 0.5f, stageSize.y * 0.5f);
			transform.localPosition = Vector3.Lerp(transform.localPosition, pos, Time.deltaTime * 6f);
		}
	}


	public static void CameraShake () {
		CameraShaker.localPosition = Random.insideUnitCircle * 0.5f;
		DemoStage.Main.MainLight.color = new Color(0.6f, 0.3f, 0.3f);
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize * 0.8f, CameraSize * 0.7f, CameraSize);
	}


}
}
