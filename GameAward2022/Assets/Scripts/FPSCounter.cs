using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;

/*
 Unity -> Edit -> ProjectSettings -> Quality -> 全てのLevelsの VSync Count を "Don't VSync" へ変更
*/

/// <summary>
/// For debugging: FPS Counter
/// デバッグ用: FPS カウンタ
/// </summary>
public class FPSCounter : MonoBehaviour
{
	/// <summary>
	/// Reflect measurement results every 'EveryCalcurationTime' seconds.
	/// EveryCalcurationTime 秒ごとに計測結果を反映する
	/// </summary>
	[SerializeField, Range(0.1f, 1.0f)]
	float EveryCalcurationTime = 1.0f;

	/// <summary>
	/// FPS value
	/// </summary>
	public float Fps
	{
		get; private set;
	}

	int frameCount;
	float prevTime;
	Text text;

	void Start()
	{
		Application.targetFrameRate = 60;

		frameCount = 0;
		prevTime = 0.0f;
		Fps = 0.0f;
		text = this.gameObject.GetComponent<Text>();
	}

	void Update()
	{
		frameCount++;
		float time = Time.realtimeSinceStartup - prevTime;

		// n秒ごとに計測
		if (time >= EveryCalcurationTime)
		{
			Fps = frameCount / time;

			frameCount = 0;
			prevTime = Time.realtimeSinceStartup;
			text.text = "FPS:"+Fps.ToString();
		}
	}
}