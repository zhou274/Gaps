using System.Collections;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
	public Material backgroundMainMat;

	public Material backgroundSubMat;

	public Material playerMainMat;

	public Material ground1Mat;

	public Material ground2Mat;

	public Material obstacleMat;

	public Player playerScript;

	public ColorList[] colorSets;

	private int colorIndex;

	private void Start()
	{
		colorIndex = UnityEngine.Random.Range(0, colorSets.Length - 1);
		ChangeColors();
	}

	public void ChangeColors()
	{
		StartCoroutine(ChangeColor(ground1Mat, ground1Mat.color, colorSets[colorIndex].Ground1, Time.time, 1));
		StartCoroutine(ChangeColor(ground2Mat, ground2Mat.color, colorSets[colorIndex].Ground2, Time.time, 1));
		StartCoroutine(ChangeColor(obstacleMat, obstacleMat.color, colorSets[colorIndex].Obstacle, Time.time, 1));
		StartCoroutine(ChangeColor(playerMainMat, playerMainMat.color, colorSets[colorIndex].PlayerMain, Time.time, 1));
		StartCoroutine(ChangeColor(backgroundMainMat, backgroundMainMat.color, colorSets[colorIndex].backgroundMain, Time.time, 1));
		StartCoroutine(ChangeColor(backgroundSubMat, backgroundSubMat.color, colorSets[colorIndex].backgroundSub, Time.time, 1));
		colorIndex = UnityEngine.Random.Range(0, colorSets.Length - 1);
	}

	private IEnumerator ChangeColor(Material mat, Color startColor, Color endColor, float time, int alpha)
	{
		float t = 0f;
		while (t < 1f && !playerScript.isDead)
		{
			t += Time.deltaTime;
			mat.color = Color.Lerp(startColor, endColor, t);
			Color color = mat.color;
			float r = color.r;
			Color color2 = mat.color;
			float g = color2.g;
			Color color3 = mat.color;
			mat.color = new Color(r, g, color3.b, Mathf.Lerp(1f, alpha, t));
			yield return 0;
		}
	}
}
