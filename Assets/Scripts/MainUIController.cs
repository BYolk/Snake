using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 主场景UI控制器代码：分数记录，长度记录，背景颜色切换
/// </summary>
public class MainUIController : MonoBehaviour
{
    //单例：
    #region
    private static MainUIController _instance;
    public static MainUIController Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    //属性：
    #region
    public int score = 0;
    public int length = 0;
    public Image backgroundImage;
    private Color tempColor;
    public bool isPause;
    public Sprite[] pauseSprites;
    public bool hasBorder = true;
    #endregion

    //引用：
    #region
    public Text msgText;
    public Text scoreText;
    public Text lengthText;
    public Image pauseImage;

    #endregion

    //初始化方法
    #region 
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        if(PlayerPrefs.GetInt("border",1) == 0)
        {
            hasBorder = false;
            foreach(Transform t in backgroundImage.gameObject.transform)
            {
                t.gameObject.GetComponent<Image>().enabled = false;
            }
        }
    }
    #endregion


    //Update方法
    #region
    private void Update()
    {
        switch(score / 100)
        {
            //避免吃到奖励目标增加分数过大跳过一个阶段
            case 0:
            case 1:
            case 2:
                break;
            case 3:
            case 4:
                ColorUtility.TryParseHtmlString("#CCEEFFFF", out tempColor);
                backgroundImage.color = new Color();
                msgText.text = "阶 段 " + 2;
                break;
            case 5:
            case 6:
                ColorUtility.TryParseHtmlString("#CCFFDBFF", out tempColor);
                backgroundImage.color = new Color();
                msgText.text = "阶 段 " + 3;
                break;
            case 7:
            case 8:
                ColorUtility.TryParseHtmlString("#EBFFCCFF", out tempColor);
                backgroundImage.color = new Color();
                msgText.text = "阶 段 " + 4;
                break;
            case 9:
            case 10:
                ColorUtility.TryParseHtmlString("#FFF3CCFF", out tempColor);
                backgroundImage.color = new Color();
                msgText.text = "阶 段 " + 5;
                break;
            case 11:
                ColorUtility.TryParseHtmlString("#FFDACCFF", out tempColor);
                backgroundImage.color = new Color();
                msgText.text = "无 尽 阶 段";
                break;
        }
    }
    #endregion

    public void UpdateUI(int score = 5,int length = 1)
    {
        this.score += score;
        this.length += length;
        scoreText.text = "得分:\n" + this.score;
        lengthText.text = "长度:\n" + this.length;
    }

    public void Pause()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;//时间冻结
            pauseImage.sprite = pauseSprites[1];
        }
        else
        {
            Time.timeScale = 1;//时间冻结
            pauseImage.sprite = pauseSprites[0];
        }
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }
}
