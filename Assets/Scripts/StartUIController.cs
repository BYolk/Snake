using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ��ʼ����UI����
/// </summary>
public class StartUIController : MonoBehaviour
{
    //����
    #region

    #endregion

    //����
    #region
    public Text lastText;
    public Text bestText;
    public Toggle blue;
    public Toggle yellow;
    public Toggle border;
    public Toggle nonBorder;
    #endregion

    //��ʼ������
    #region
    private void Awake()
    {
        lastText.text = "�ϴΣ�����" + PlayerPrefs.GetInt("lastLength", 0) + ",����" + PlayerPrefs.GetInt("lastScore", 0);
        bestText.text = "��ã�����" + PlayerPrefs.GetInt("bestLength", 0) + ",����" + PlayerPrefs.GetInt("bestScore", 0);
    }
    //��ȡ��ťѡ��
    private void Start()
    {
        //�����ɫ�߱�ѡ��
        //PlayerPrefs.GetString("snakeHead","snakeHead01") == "snakeHead01":���ע�����û��snakeHead����ô����snakeHead01�������snakeHead����ô�÷��ظ�ֵ������ ,"snakeHead01")��������ֵ��snakeHead01�����ж� == "snakeHead01"
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

        //�߽�ģʽ�ж�
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

    //��ť����
    #region
    //��ʼ��ť
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


    //Ƥ����ť��
    //isOn��ʾBlue��ɫƤ�������ѡ���Ƿ�ѡ�е�״̬
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

    //ģʽ��ť��
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
