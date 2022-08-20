using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 冰淇淋食物脚本
/// 经过测量，蛇在起始位置往右走16步就会碰到右边的墙，所以食物可以在蛇起始位置往右生成的位置只有(30,0,0),(60,0,0)……(450,0,0)共15个地方
/// 同理，往左只有(-30, 0, 0),(-60, 0, 0)……(-270, 0, 0)共9个地方
/// 往上只有(0,30,0),(0, 60, 0)……(0, 330, 0)共11个地方
/// 往下只有(0,-30,0),(0, -60, 0)……(0, -330, 0)共11个地方
/// 加上蛇头生成位置(0,0,0)一个地方
/// 以上位置是食物可以随机生成的位置:x方向上有25个，在y方向还有23个
/// 上一个食物被吃掉之后才生成下一个食物
/// </summary>
public class IceCream : MonoBehaviour
{
    //单例：
    #region
    private static IceCream _instance;
    public static IceCream Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    //属性：
    #region
    public int xRightLimit = 15;//往右只能走15步
    public int xLeftLimit = 9;//往左只能走9步
    public int yLimit = 11;//上下都只能走11步
    public Sprite[] iceCreamSprites;//存放所有冰淇淋精灵的数组

    #endregion

    //引用：
    #region
    public GameObject iceCreamPrefab;
    public Transform iceCreamHolder;
    public GameObject rewardPrefab;
    #endregion

    //初始化方法：
    #region
    private void Awake()
    {
        _instance = this;    
    }

    private void Start()
    {
        iceCreamHolder = GameObject.Find("IceCreamHolder").transform;
        CreateIceCream(false);//第一次生成不需要奖励
    }
    #endregion


    /// <summary>
    /// 生成冰激凌方法
    /// </summary>
    /// <param name="isReward">此次生成冰激凌是否要带一个奖励目标</param>
    public void CreateIceCream(bool isReward)
    {
        //生成冰淇淋对象
        GameObject iceCream = Instantiate(iceCreamPrefab);
        iceCream.transform.SetParent(iceCreamHolder, false);//false表示不让Canvas自动为iceCream对象设置Scale

        //随机选择冰淇淋图片
        int index = Random.Range(0, iceCreamSprites.Length);
        iceCream.GetComponent<Image>().sprite = iceCreamSprites[index];

        //随机选择生成位置
        int x = Random.Range(-xLeftLimit, xRightLimit);
        int y = Random.Range(-yLimit, yLimit);
        iceCream.transform.localPosition = new Vector3(x * 30, y * 30);

        if (isReward)
        {
            //生成奖励目标对象
            GameObject reward = Instantiate(rewardPrefab);
            reward.transform.SetParent(iceCreamHolder, false);//false表示不让Canvas自动为iceCream对象设置Scale

            //随机选择奖励目标生成位置
            x = Random.Range(-xLeftLimit, xRightLimit);
            y = Random.Range(-yLimit, yLimit);
            reward.transform.localPosition = new Vector3(x * 30, y * 30);
        }
    }

    
}
