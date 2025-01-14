using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
	public GameObject player;

	[Space(10f)]
	public GameObject[] obstacles;

	[Space(10f)]
	public float distanceBetweenObstacles = 8f;

	[Space(10f)]
	public GameObject doorPrefab;

	private GameObject currentObstacle;

	private GameObject newObstacle;

	private GameObject door;

	private int obstacle;

	private void Start()
	{
		GameObject original = obstacles[obstacle];
		Vector3 position = obstacles[obstacle].transform.position;
		currentObstacle = UnityEngine.Object.Instantiate(original, new Vector3(0f, position.y, distanceBetweenObstacles), Quaternion.identity);
		currentObstacle.transform.SetParent(base.transform);
		currentObstacle.transform.GetComponent<Obstacle>().SetPlayerObject(player, 0);
		Transform transform = currentObstacle.transform;
		Vector3 position2 = currentObstacle.transform.position;
		float y = position2.y;
		Vector3 position3 = currentObstacle.transform.position;
		transform.position = new Vector3(0f, y, position3.z);
		GameObject original2 = doorPrefab;
		Vector3 position4 = doorPrefab.transform.position;
		float y2 = position4.y;
		Vector3 position5 = currentObstacle.transform.position;
		door = UnityEngine.Object.Instantiate(original2, new Vector3(0f, y2, position5.z - distanceBetweenObstacles / 2f), Quaternion.identity);
	}

	private void CreateObstacle()
	{
		obstacle = UnityEngine.Random.Range(0, obstacles.Length);
		GameObject original = obstacles[obstacle];
		Vector3 position = obstacles[obstacle].transform.position;
		float y = position.y;
		Vector3 position2 = currentObstacle.transform.position;
		newObstacle = UnityEngine.Object.Instantiate(original, new Vector3(0f, y, position2.z + 8f), Quaternion.identity);
		newObstacle.transform.SetParent(base.transform);
		newObstacle.transform.GetComponent<Obstacle>().SetPlayerObject(player, 1);
		GameObject original2 = doorPrefab;
		Vector3 position3 = doorPrefab.transform.position;
		float y2 = position3.y;
		Vector3 position4 = newObstacle.transform.position;
		door = UnityEngine.Object.Instantiate(original2, new Vector3(0f, y2, position4.z - distanceBetweenObstacles / 2f), Quaternion.identity);
		Transform transform = door.transform;
		Vector3 position5 = newObstacle.transform.position;
		float x = position5.x;
		Vector3 position6 = currentObstacle.transform.position;
		float x2 = (x + position6.x) / 2f;
		Vector3 position7 = door.transform.position;
		float y3 = position7.y;
		Vector3 position8 = door.transform.position;
		transform.position = new Vector3(x2, y3, position8.z);
		currentObstacle = newObstacle;
	}

	private void Update()
	{
		Vector3 position = player.transform.position;
		float z = position.z;
		Vector3 position2 = currentObstacle.transform.position;
		if (z > position2.z - 30f)
		{
			CreateObstacle();
		}
	}
}
