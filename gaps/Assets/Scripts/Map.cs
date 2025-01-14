using UnityEngine;

public class Map : MonoBehaviour
{
	public GameObject player;

	private readonly float mapLength = 20f;

	private void Update()
	{
		Vector3 position = player.transform.position;
		float z = position.z;
		Vector3 position2 = base.transform.position;
		if (z - position2.z > mapLength + mapLength / 4f)
		{
			Transform transform = base.transform;
			Vector3 position3 = base.transform.position;
			float x = position3.x;
			Vector3 position4 = base.transform.position;
			float y = position4.y;
			Vector3 position5 = base.transform.position;
			transform.position = new Vector3(x, y, position5.z + mapLength * 4f);
		}
	}
}
