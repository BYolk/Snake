using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 蛇头脚本
/// 需要解决的问题：
///     蛇头在不同方向上的朝向不同，需要在蛇转向的时候将蛇头也旋转，改变蛇的localRotation，需要一个四元素，所以要将欧拉角转化为四元素
///     蛇头瞬间转向问题：蛇头在朝上走的时候，如果按朝下键蛇头会立马往下，如果有身子的话就立刻碰到身子，游戏结束。需要在Update方法里面做额外判断
/// </summary>
public class SnakeHead : MonoBehaviour
{
    //属性
    #region
    //蛇头移动步数step；移动x，y距离；蛇头位置headPos；蛇头Move方法重复调用时间/速度velocity
    public int step;
    private int x;
    private int y;
    private Vector3 headPos;
    private float velocity = 0.25f;
    public List<Transform> bodyList = new List<Transform>();//用于存放生成的蛇身
    public Sprite[] bodySprites = new Sprite[2];//蛇的身体有两种图片，创建一个数组，根据奇偶性来获取蛇身图片
    private bool isDie = false;
    #endregion

    //引用：
    #region
    public GameObject bodyPrefab;
    private Transform canvas;
    public GameObject dieEffect;
    public AudioClip eatClip;
    public AudioClip dieClip;
    #endregion

    //初始化方法
    #region
    private void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;

        //初始化蛇头和蛇身，蛇头和蛇身根据Start面板的Toggle来定
        //通过ResourceRequest.Load(string path)加载资源，path不需要加Resources/以及文件扩展名
        //GetString("SnakeHead", "SnakeHead02"):加载对应的键值，如果没有，用SnakeHead02代替
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("snakeHead", "snakeHead02"));
        bodySprites[0] = Resources.Load<Sprite>(PlayerPrefs.GetString("snakeBody", "snakeBody0201"));
        bodySprites[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("snakeBody", "snakeBody0202"));
    }
    private void Start()
    {
        //参数：重复调用哪个方法；执行这条语句多少时间后第一次调用这个方法；每隔多少时间调用一次这个方法
        InvokeRepeating("Move", 0, velocity);
        //游戏一开始蛇头就往右边走
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);
        x = step;
        y = 0;
    }
    #endregion

    //update方法
    #region
    private void Update()
    {
        //给予玩家自由选择游戏速度条件：按下空格不放加速，抬起空格取消加速
        if (Input.GetKeyDown(KeyCode.Space) && MainUIController.Instance.isPause == false && isDie == false)
        {
            //InvokeRepeating("Move", 0, velocity);方法一旦调用，即使中途修改Velocity也不能对该方法有影响,需要先取消Invoke，修改数值后再重新启用Invoke
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity - 0.15f);
        }
        if (Input.GetKeyUp(KeyCode.Space) && MainUIController.Instance.isPause == false && isDie == false)
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity);
        }



        //蛇头能够按下W键向上走的条件是：蛇头此时的转态不是向下走
        if (Input.GetKey(KeyCode.W) && y != -step && MainUIController.Instance.isPause == false && isDie == false)
        {
            //设置蛇头在不同方向上的朝向：需要一个四元素来旋转蛇头，调用Quaternion的Euler欧拉方法，将欧拉角转化为四元素
            gameObject.transform.localRotation = Quaternion.Euler(0,0,0);
            x = 0;
            y = step;
        }
        if (Input.GetKey(KeyCode.S) && y != step && MainUIController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
            x = 0;
            y = -step;
        }
        if (Input.GetKey(KeyCode.A) && x != step && MainUIController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
            x = -step;
            y = 0;
        }
        if (Input.GetKey(KeyCode.D) && x != -step && MainUIController.Instance.isPause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);
            x = step;
            y = 0;
        }
    }
    #endregion

    //移动，进食，成长，死亡方法
    #region
    /// <summary>
    /// 蛇头移动方法
    /// </summary>
    private void Move()
    {
        //记录蛇头运动前位置
        headPos = gameObject.transform.localPosition;
        gameObject.transform.localPosition = new Vector3(headPos.x + x,headPos.y + y,headPos.z);
        if(bodyList.Count > 0)
        {
            //第一种移动方法：会让同色黏在一起
            //bodyList.Last().localPosition = headPos;//将最后一个身体放到头移动前的位置
            //bodyList.Insert(0, bodyList.Last());//因为最后一个移动到第一个，所以将最后一个元素插入到第一个
            //bodyList.RemoveAt(bodyList.Count - 1);//将原先的最后一个（还在数组里面）给移除了
            
            //第二种移动方法：每个蛇身都移动到前一个蛇身位置
            for(int i = bodyList.Count -2; i >= 0; i--)
            {
                bodyList[i + 1].localPosition = bodyList[i].localPosition;//
            }
            bodyList[0].localPosition = headPos;
        }
    }


    /// <summary>
    /// 触发器触发蛇头吃食物方法
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //也可以写成这个collision.tag == "IceCream"
        if (collision.gameObject.CompareTag("IceCream"))
        {
            //碰到食物，销毁食物，得分，生成新的冰激凌
            Destroy(collision.gameObject);
            MainUIController.Instance.UpdateUI();
            Grow();
            //先生成一个冰激凌，如果达到某个概率，再生成一个奖励目标
            IceCream.Instance.CreateIceCream((Random.Range(1, 100) < 20) ? true : false);      
        }
        else if (collision.gameObject.CompareTag("Reward"))
        {
            Destroy(collision.gameObject);
            MainUIController.Instance.UpdateUI(Random.Range(5,15) * 10);
            Grow();
        }
        else if (collision.gameObject.CompareTag("Body"))
        {
            Die();
        }
        else
        {
            //如果有边界，碰到边界死亡
            if (MainUIController.Instance.hasBorder)
            {
                Die();
            }
            else
            {
                //撞到边界:为了避免从一遍传送到另一边后（另一边也有碰撞器）又传送回来，需要在传送过去后再走一步（+30/-30）
                switch (collision.gameObject.name)
                {
                    case "UpWall":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                        break;
                    case "DownWall":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 30, transform.localPosition.z);
                        break;
                    case "RightWall":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 240, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "LeftWall":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 180, transform.localPosition.y, transform.localPosition.z);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 吃到食物长身体方法
    /// </summary>
    private void Grow()
    {
        AudioSource.PlayClipAtPoint(eatClip, Vector3.zero);
        int index = (bodyList.Count % 2 == 0) ? 0 : 1;//根据奇偶性选择身体图片
        GameObject body = Instantiate(bodyPrefab,new Vector3(2000,2000,0),Quaternion.identity);//将生成的食物放在看不到的地方，在移动身体的Move方法内去排序位置，避免直接生在身体上产生各种奇怪bug
        body.GetComponent<Image>().sprite = bodySprites[index];//选择蛇身图片
        body.transform.SetParent(canvas, false);//false表示不自动更改世界坐标
        bodyList.Add(body.transform);
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(dieClip, Vector3.zero);
        //死亡不能移动
        CancelInvoke();
        isDie = true;
        //播放效果
        Vector3 dieEffectPos = new Vector3(0, 0, -5);
        Instantiate(dieEffect, dieEffectPos,transform.rotation);//不用销毁，因为很快就重新加载场景


        //记录最新长度与分数：
        //PlayerPrefs：保存在window的注册表内，用键值对保存，左边参数是键名，右边是键值
        PlayerPrefs.SetInt("lastLength", MainUIController.Instance.length);
        PlayerPrefs.SetInt("lastScore", MainUIController.Instance.score);
        
        //判断是否为最佳长度与分数：
        //PlayerPrefs.GetInt("bestScore",0):如果从来没有保存过bestScore，那么会返回0
        if (PlayerPrefs.GetInt("bestScore",0) < MainUIController.Instance.score)
        {
            PlayerPrefs.SetInt("bestLength", MainUIController.Instance.length);
            PlayerPrefs.SetInt("bestScore", MainUIController.Instance.score);
        }
        
        StartCoroutine(GameOver(1.5f));
    }

    IEnumerator GameOver(float time)
    {
        //等待time时间后
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(1);
    }
    #endregion
}
