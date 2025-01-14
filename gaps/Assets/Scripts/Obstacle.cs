using System;
using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	[Space(10f)]
	public float rotationSpeedMin;

	public float rotationSpeedMax;

	[Space(10f)]
	public float randomXMin;

	public float randomXMax;

	[Space(10f)]
	[Range(0f, 12f)]
	public int maxNumberOfSpawnedLittleBalls;

	public GameObject littleBallPrefab;

	public float littleBallDistanceFromCenter;

	private GameObject playerObj;

	private GameObject tempLittleBall;

	private GameObject obstacle;

	private float rotationSpeedY;

	private bool changed;

	public Transform HoldPoint;

	private void Start()
	{
		obstacle = base.gameObject.transform.GetChild(0).gameObject;
		InitObstacle();
	}

	public void SetPlayerObject(GameObject po, int index)
	{
		playerObj = po;
		if (index == 0)
		{
			randomXMax = 0f;
			randomXMin = 0f;
		}
	}

	private void Update()
	{
		obstacle.transform.Rotate(0f, rotationSpeedY, 0f);
		Vector3 position = base.transform.position;
		float z = position.z;
		Vector3 position2 = playerObj.transform.position;
		if (z < position2.z - 10f)
		{
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					//UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			//UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void InitObstacle()
	{
		Transform transform = base.transform;
		float x = UnityEngine.Random.Range(randomXMin, randomXMax);
		Vector3 position = base.transform.position;
		float y = position.y;
		Vector3 position2 = base.transform.position;
		transform.position = new Vector3(x, y, position2.z);
		if (UnityEngine.Random.Range(0, 100) < 50)
		{
			rotationSpeedY = UnityEngine.Random.Range(rotationSpeedMin, rotationSpeedMax);
		}
		else
		{
			rotationSpeedY = UnityEngine.Random.Range(0f - rotationSpeedMax, 0f - rotationSpeedMin);
		}
		if (maxNumberOfSpawnedLittleBalls > 0)
		{
			maxNumberOfSpawnedLittleBalls = UnityEngine.Random.Range(1, maxNumberOfSpawnedLittleBalls);
			for (int i = 0; i <= maxNumberOfSpawnedLittleBalls; i++)
			{
				tempLittleBall = UnityEngine.Object.Instantiate(littleBallPrefab, base.transform);
				tempLittleBall.transform.eulerAngles = new Vector3(0f, 360 / maxNumberOfSpawnedLittleBalls * i, 0f);
				Transform transform2 = tempLittleBall.transform;
				Vector3 position3 = base.transform.position;
				float x2 = position3.x + littleBallDistanceFromCenter * Mathf.Sin((float)Math.PI / 180f * (float)(360 / maxNumberOfSpawnedLittleBalls) * (float)i);
				Vector3 position4 = base.transform.position;
				transform2.position = new Vector3(x2, 0.8f, position4.z + littleBallDistanceFromCenter * Mathf.Cos((float)Math.PI / 180f * (float)(360 / maxNumberOfSpawnedLittleBalls) * (float)i));
			}
		}
	}
}
