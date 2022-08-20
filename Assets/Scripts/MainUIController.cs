using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ������UI���������룺������¼�����ȼ�¼��������ɫ�л�
/// </summary>
public class MainUIController : MonoBehaviour
{
    //������
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

    //���ԣ�
    #region
    public int score = 0;
    public int length = 0;
    public Image backgroundImage;
    private Color tempColor;
    public bool isPause;
    public Sprite[] pauseSprites;
    public bool hasBorder = true;
    #endregion

    //���ã�
    #region
    public Text msgText;
    public Text scoreText;
    public Text lengthText;
    public Image pauseImage;

    #endregion

    //��ʼ������
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


    //Update����
    #region
    private void Update()
    {
        switch(score / 100)
        {
            //����Ե�����Ŀ�����ӷ�����������һ���׶�
            case 0:
            case 1:
            case 2:
                break;
            case 3:
            case 4:
                ColorUtility.TryParseHtmlString("#CCEEFFFF", out tempColor);
                backgroundImage.color = new Color();
                msgText.text = "�� �� " + 2;
                break;
            case 5:
            case 6:
                ColorUtility.TryParseHtmlString("#CCFFDBFF", out tempColor);
                backgroundImage.color = new Color();
                msgText.text = "�� �� " + 3;
                break;
            case 7:
            case 8:
                ColorUtility.TryParseHtmlString("#EBFFCCFF", out tempColor);
                backgroundImage.color = new Color();
                msgText.text = "�� �� " + 4;
                break;
            case 9:
            case 10:
                ColorUtility.TryParseHtmlString("#FFF3CCFF", out tempColor);
                backgroundImage.color = new Color();
                msgText.text = "�� �� " + 5;
                break;
            case 11:
                ColorUtility.TryParseHtmlString("#FFDACCFF", out tempColor);
                backgroundImage.color = new Color();
                msgText.text = "�� �� �� ��";
                break;
        }
    }
    #endregion

    public void UpdateUI(int score = 5,int length = 1)
    {
        this.score += score;
        this.length += length;
        scoreText.text = "�÷�:\n" + this.score;
        lengthText.text = "����:\n" + this.length;
    }

    public void Pause()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;//ʱ�䶳��
            pauseImage.sprite = pauseSprites[1];
        }
        else
        {
            Time.timeScale = 1;//ʱ�䶳��
            pauseImage.sprite = pauseSprites[0];
        }
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }
}
