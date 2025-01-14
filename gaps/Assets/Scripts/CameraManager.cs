using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public GameObject player;

	public float smoothTime = 0.3f;

	public float yOffset;

	public float zOffset;

	public float xOffset;

	private Vector3 velocity = Vector3.zero;

	private float posX;

	public Vector3 LastCamera;
    public void Awake()
    {
		xOffset = transform.position.x - player.transform.position.x;
    }
    private void Update()
	{
		FollowPlayer();
		Vector3 position = player.transform.position;
		float z = position.z;
		Vector3 position2 = base.transform.position;
		if (z < position2.z - 1f)
		{
			player.GetComponent<Player>().OutOfScreen();
		}
	}
	public void ResetCamera()
	{
		transform.position = LastCamera;
	}
	public void FollowPlayer()
	{
		float x = posX;
		float y = yOffset;
		Vector3 position = player.transform.position;
		Vector3 target = new Vector3(x, y, position.z - zOffset);
		float z = target.z;
		Vector3 position2 = base.transform.position;
		if (z > position2.z)
		{
			base.transform.position = Vector3.SmoothDamp(base.transform.position, target, ref velocity, smoothTime);
		}
	}

	public void ZoomIn()
	{
		smoothTime = 0.1f;
		Vector3 position = player.transform.position;
		posX = position.x;
		zOffset = 6f;
		yOffset = 8f;
	}
}
