using UnityEngine;

public class Explosion : MonoBehaviour
{
	public enum ExplosionType
	{
		SPHERE,
		CUBE
	}

	public float elementSize = 0.2f;

	public int elementsInRow = 5;

	public float explosionForce = 50f;

	public float explosionRadius = 4f;

	public float explosionUpward = 0.4f;

	public ExplosionType explosionElement;

	public Material material;

	private float cubesPivotDistance;

	private Vector3 cubesPivot;

	private void Start()
	{
		cubesPivotDistance = elementSize * (float)elementsInRow / 2f;
		cubesPivot = new Vector3(cubesPivotDistance, cubesPivotDistance, cubesPivotDistance);
	}

	public void Explode()
	{
		base.gameObject.SetActive(value: false);
		for (int i = 0; i < elementsInRow; i++)
		{
			for (int j = 0; j < elementsInRow; j++)
			{
				for (int k = 0; k < elementsInRow; k++)
				{
					CreatePiece(i, j, k);
				}
			}
		}
		Vector3 position = base.transform.position;
		Collider[] array = Physics.OverlapSphere(position, explosionRadius);
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			Rigidbody component = collider.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.AddExplosionForce(explosionForce, base.transform.position, explosionRadius, explosionUpward);
			}
		}
	}

	private void CreatePiece(int x, int y, int z)
	{
		GameObject gameObject = (explosionElement != ExplosionType.CUBE) ? GameObject.CreatePrimitive(PrimitiveType.Sphere) : GameObject.CreatePrimitive(PrimitiveType.Cube);
		gameObject.transform.position = base.transform.position + new Vector3(elementSize * (float)x, elementSize * (float)y, elementSize * (float)z) - cubesPivot;
		gameObject.transform.localScale = new Vector3(elementSize, elementSize, elementSize);
		gameObject.AddComponent<Rigidbody>();
		gameObject.GetComponent<Rigidbody>().mass = elementSize;
		gameObject.GetComponent<MeshRenderer>().material = material;
	}
}
