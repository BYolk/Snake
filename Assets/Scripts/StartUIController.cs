using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 开始场景UI设置
/// </summary>
public class StartUIController : MonoBehaviour
{
    //属性
    #region

    #endregion

    //引用
    #region
    public Text lastText;
    public Text bestText;
    public Toggle blue;
    public Toggle yellow;
    public Toggle border;
    public Toggle nonBorder;
    #endregion

    //初始化方法
    #region
    private void Awake()
    {
        lastText.text = "上次：长度" + PlayerPrefs.GetInt("lastLength", 0) + ",分数" + PlayerPrefs.GetInt("lastScore", 0);
        bestText.text = "最好：长度" + PlayerPrefs.GetInt("bestLength", 0) + ",分数" + PlayerPrefs.GetInt("bestScore", 0);
    }
    //读取按钮选择
    private void Start()
    {
        //如果蓝色蛇被选中
        //PlayerPrefs.GetString("snakeHead","snakeHead01") == "snakeHead01":如果注册表中没有snakeHead，那么返回snakeHead01，如果有snakeHead，那么久返回该值，忽略 ,"snakeHead01")，并将该值与snakeHead01进行判断 == "snakeHead01"
        if (PlayerPrefs.GetString("snakeHead","snakeHead01") == "snakeHead01")
        {
            blue.isOn = true;
            PlayerPrefs.SetString("snakeHead", "snakeHead01");
            PlayerPrefs.SetString("snakeBody01", "snakeBody0101");
            PlayerPrefs.SetString("snakeBody02", "snakeBody0102");
        }
        else
        {
            yellow.isOn = true;
            PlayerPrefs.SetString("snakeHead", "snakeHead02");
            PlayerPrefs.SetString("snakeBody01", "snakeBody0201");
            PlayerPrefs.SetString("snakeBody02", "snakeBody0202");
        }

        //边界模式判断
        if(PlayerPrefs.GetInt("border", 1) == 1)
        {
            border.isOn = true;
            PlayerPrefs.SetInt("border", 1);
        }
        else
        {
            nonBorder.isOn = true;
            PlayerPrefs.SetInt("border", 0);
        }
    }
    #endregion

    //按钮方法
    #region
    //开始按钮
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


    //皮肤按钮：
    //isOn表示Blue蓝色皮肤这个单选框是否被选中的状态
    public void BlueSelected(bool  isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("snakeHead", "snakeHead01");
            PlayerPrefs.SetString("snakeBody01", "snakeBody0101");
            PlayerPrefs.SetString("snakeBody02", "snakeBody0102");
        }
    }
    public void YellowSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("snakeHead", "snakeHead02");
            PlayerPrefs.SetString("snakeBody01", "snakeBody0201");
            PlayerPrefs.SetString("snakeBody02", "snakeBody0202");
        }
    }

    //模式按钮：
    public void BorderSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border",1);
        }
    }

    public void NonBorderSelected(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 0);
        }
    }
    #endregion

}
