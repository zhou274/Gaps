using System.Collections;
using UnityEngine;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;
using StarkSDKSpace;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class Player : MonoBehaviour
{
	public float MoveForwardSpeed = 2f;

	public bool isDead;

	public bool hitObstacle;

	public Explosion explosion;

	public MeshRenderer meshRenderer;

	private GamePlayManager GamePlayManagerScript;

	private Rigidbody rgdBdy;

	private Vector3 movingPos;

	private Vector2 tempPos;

	private float angle;

	public GameObject currentObstacle;

    public string clickid;
    private StarkAdManager starkAdManager;

    private void Start()
	{
		Time.timeScale = 1f;
		GamePlayManagerScript = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();
		movingPos = new Vector3(0f, 0f, MoveForwardSpeed);
		rgdBdy = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (!hitObstacle)
		{
			MoveForward(movingPos);
		}
		if (Input.GetMouseButtonDown(0) && hitObstacle)
		{
			hitObstacle = false;
			Vector3 position = base.transform.parent.position;
			float x = position.x;
			Vector3 position2 = base.transform.parent.position;
			tempPos = new Vector2(x, position2.z);
			Vector2 a = tempPos;
			Vector3 position3 = base.transform.position;
			float x2 = position3.x;
			Vector3 position4 = base.transform.position;
			tempPos = a - new Vector2(x2, position4.z);
			base.transform.parent = null;
			angle = Mathf.Atan2(tempPos.y, tempPos.x);
			movingPos = new Vector3((0f - Mathf.Cos(angle)) * MoveForwardSpeed, 0f, (0f - Mathf.Sin(angle)) * MoveForwardSpeed);
			AudioManager.Instance.PlayEffects(AudioManager.Instance.shoot);
		}
	}

	private void MoveForward(Vector3 movPos)
	{
		if (GamePlayManagerScript.isStarted && !isDead)
		{
			base.transform.position += Time.deltaTime * movPos;
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "SideTrigger")
		{
			if (!isDead)
			{
				StartCoroutine(Dead());
			}
		}
		else if (other.gameObject.tag == "Obstacle" && !hitObstacle)
		{
			ObstacleHit(other.gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "LittleBall")
		{
			UnityEngine.Object.Destroy(other.gameObject);
			GamePlayManagerScript.CollectLittleBall();
			AudioManager.Instance.PlayEffects(AudioManager.Instance.littleBallCollect);
		}
	}

	private IEnumerator Dead()
	{
		//Time.timeScale = 0;
		AudioManager.Instance.PlayEffects(AudioManager.Instance.crash);
		isDead = true;
		//GameObject.Find("Main Camera").GetComponent<CameraManager>().ZoomIn();
		//禁用碰撞体
		GetComponent<SphereCollider>().enabled = false;
		//打开结束面板
		GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>().GameOver();
        ShowInterstitialAd("1lcaf5895d5l1293dc",
            () => {
                Debug.LogError("--插屏广告完成--");

            },
            (it, str) => {
                Debug.LogError("Error->" + str);
            });

        //explosion.Explode();
        meshRenderer.enabled = false;
		base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
		yield break;
	}
	public void Respawn()
	{
        ShowVideoAd("192if3b93qo6991ed0",
            (bol) => {
                if (bol)
                {

                    gameObject.SetActive(true);
                    GetComponent<SphereCollider>().enabled = true;
                    meshRenderer.enabled = true;
                    base.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>().ContinueGame();
                    ObstacleHit(currentObstacle);
                    GameObject.Find("Main Camera").GetComponent<CameraManager>().ResetCamera();
                    transform.position = currentObstacle.transform.Find("HoldPoint").position;
                    isDead = false;


                    clickid = "";
                    getClickid();
                    apiSend("game_addiction", clickid);
                    apiSend("lt_roi", clickid);


                }
                else
                {
                    StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
                }
            },
            (it, str) => {
                Debug.LogError("Error->" + str);
                //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
            });
        
    }
	public void OutOfScreen()
	{
		if (!isDead)
		{
			isDead = true;
			GetComponent<SphereCollider>().enabled = false;
			GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>().GameOver();
			AudioManager.Instance.PlayEffects(AudioManager.Instance.crash);
		}
	}

	private void ObstacleHit(GameObject obstacle)
	{
		currentObstacle = obstacle;
		rgdBdy.velocity = Vector3.zero;
		hitObstacle = true;
		base.gameObject.transform.SetParent(obstacle.transform);
		if(!isDead)
		{
            GameObject.Find("Main Camera").GetComponent<CameraManager>().LastCamera = GameObject.Find("Main Camera").transform.position;
        }
    }


    public void getClickid()
    {
        var launchOpt = StarkSDK.API.GetLaunchOptionsSync();
        if (launchOpt.Query != null)
        {
            foreach (KeyValuePair<string, string> kv in launchOpt.Query)
                if (kv.Value != null)
                {
                    Debug.Log(kv.Key + "<-参数-> " + kv.Value);
                    if (kv.Key.ToString() == "clickid")
                    {
                        clickid = kv.Value.ToString();
                    }
                }
                else
                {
                    Debug.Log(kv.Key + "<-参数-> " + "null ");
                }
        }
    }

    public void apiSend(string eventname, string clickid)
    {
        TTRequest.InnerOptions options = new TTRequest.InnerOptions();
        options.Header["content-type"] = "application/json";
        options.Method = "POST";

        JsonData data1 = new JsonData();

        data1["event_type"] = eventname;
        data1["context"] = new JsonData();
        data1["context"]["ad"] = new JsonData();
        data1["context"]["ad"]["callback"] = clickid;

        Debug.Log("<-data1-> " + data1.ToJson());

        options.Data = data1.ToJson();

        TT.Request("https://analytics.oceanengine.com/api/v2/conversion", options,
           response => { Debug.Log(response); },
           response => { Debug.Log(response); });
    }


    /// <summary>
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="closeCallBack"></param>
    /// <param name="errorCallBack"></param>
    public void ShowVideoAd(string adId, System.Action<bool> closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            starkAdManager.ShowVideoAdWithId(adId, closeCallBack, errorCallBack);
        }
    }

    /// <summary>
    /// 播放插屏广告
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="errorCallBack"></param>
    /// <param name="closeCallBack"></param>
    public void ShowInterstitialAd(string adId, System.Action closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            var mInterstitialAd = starkAdManager.CreateInterstitialAd(adId, errorCallBack, closeCallBack);
            mInterstitialAd.Load();
            mInterstitialAd.Show();
        }
    }
}
