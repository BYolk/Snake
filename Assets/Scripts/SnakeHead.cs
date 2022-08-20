using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ��ͷ�ű�
/// ��Ҫ��������⣺
///     ��ͷ�ڲ�ͬ�����ϵĳ���ͬ����Ҫ����ת���ʱ����ͷҲ��ת���ı��ߵ�localRotation����Ҫһ����Ԫ�أ�����Ҫ��ŷ����ת��Ϊ��Ԫ��
///     ��ͷ˲��ת�����⣺��ͷ�ڳ����ߵ�ʱ����������¼���ͷ���������£���������ӵĻ��������������ӣ���Ϸ��������Ҫ��Update���������������ж�
/// </summary>
public class SnakeHead : MonoBehaviour
{
    //����
    #region
    //��ͷ�ƶ�����step���ƶ�x��y���룻��ͷλ��headPos����ͷMove�����ظ�����ʱ��/�ٶ�velocity
    public int step;
    private int x;
    private int y;
    private Vector3 headPos;
    private float velocity = 0.25f;
    public List<Transform> bodyList = new List<Transform>();//���ڴ�����ɵ�����
    public Sprite[] bodySprites = new Sprite[2];//�ߵ�����������ͼƬ������һ�����飬������ż������ȡ����ͼƬ
    private bool isDie = false;
    #endregion

    //���ã�
    #region
    public GameObject bodyPrefab;
    private Transform canvas;
    public GameObject dieEffect;
    public AudioClip eatClip;
    public AudioClip dieClip;
    #endregion

    //��ʼ������
    #region
    private void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;

        //��ʼ����ͷ��������ͷ���������Start����Toggle����
        //ͨ��ResourceRequest.Load(string path)������Դ��path����Ҫ��Resources/�Լ��ļ���չ��
        //GetString("SnakeHead", "SnakeHead02"):���ض�Ӧ�ļ�ֵ�����û�У���SnakeHead02����
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("snakeHead", "snakeHead02"));
        bodySprites[0] = Resources.Load<Sprite>(PlayerPrefs.GetString("snakeBody", "snakeBody0201"));
        bodySprites[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("snakeBody", "snakeBody0202"));
    }
    private void Start()
    {
        //�������ظ������ĸ�������ִ������������ʱ����һ�ε������������ÿ������ʱ�����һ���������
        InvokeRepeating("Move", 0, velocity);
        //��Ϸһ��ʼ��ͷ�����ұ���
        gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);
        x = step;
        y = 0;
    }
    #endregion

    //update����
    #region
    private void Update()
    {
        //�����������ѡ����Ϸ�ٶ����������¿ո񲻷ż��٣�̧��ո�ȡ������
        if (Input.GetKeyDown(KeyCode.Space) && MainUIController.Instance.isPause == false && isDie == false)
        {
            //InvokeRepeating("Move", 0, velocity);����һ�����ã���ʹ��;�޸�VelocityҲ���ܶԸ÷�����Ӱ��,��Ҫ��ȡ��Invoke���޸���ֵ������������Invoke
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity - 0.15f);
        }
        if (Input.GetKeyUp(KeyCode.Space) && MainUIController.Instance.isPause == false && isDie == false)
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, velocity);
        }



        //��ͷ�ܹ�����W�������ߵ������ǣ���ͷ��ʱ��ת̬����������
        if (Input.GetKey(KeyCode.W) && y != -step && MainUIController.Instance.isPause == false && isDie == false)
        {
            //������ͷ�ڲ�ͬ�����ϵĳ�����Ҫһ����Ԫ������ת��ͷ������Quaternion��Eulerŷ����������ŷ����ת��Ϊ��Ԫ��
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

    //�ƶ�����ʳ���ɳ�����������
    #region
    /// <summary>
    /// ��ͷ�ƶ�����
    /// </summary>
    private void Move()
    {
        //��¼��ͷ�˶�ǰλ��
        headPos = gameObject.transform.localPosition;
        gameObject.transform.localPosition = new Vector3(headPos.x + x,headPos.y + y,headPos.z);
        if(bodyList.Count > 0)
        {
            //��һ���ƶ�����������ͬɫ���һ��
            //bodyList.Last().localPosition = headPos;//�����һ������ŵ�ͷ�ƶ�ǰ��λ��
            //bodyList.Insert(0, bodyList.Last());//��Ϊ���һ���ƶ�����һ�������Խ����һ��Ԫ�ز��뵽��һ��
            //bodyList.RemoveAt(bodyList.Count - 1);//��ԭ�ȵ����һ���������������棩���Ƴ���
            
            //�ڶ����ƶ�������ÿ�������ƶ���ǰһ������λ��
            for(int i = bodyList.Count -2; i >= 0; i--)
            {
                bodyList[i + 1].localPosition = bodyList[i].localPosition;//
            }
            bodyList[0].localPosition = headPos;
        }
    }


    /// <summary>
    /// ������������ͷ��ʳ�﷽��
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ҳ����д�����collision.tag == "IceCream"
        if (collision.gameObject.CompareTag("IceCream"))
        {
            //����ʳ�����ʳ��÷֣������µı�����
            Destroy(collision.gameObject);
            MainUIController.Instance.UpdateUI();
            Grow();
            //������һ�������裬����ﵽĳ�����ʣ�������һ������Ŀ��
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
            //����б߽磬�����߽�����
            if (MainUIController.Instance.hasBorder)
            {
                Die();
            }
            else
            {
                //ײ���߽�:Ϊ�˱����һ�鴫�͵���һ�ߺ���һ��Ҳ����ײ�����ִ��ͻ�������Ҫ�ڴ��͹�ȥ������һ����+30/-30��
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
    /// �Ե�ʳ�ﳤ���巽��
    /// </summary>
    private void Grow()
    {
        AudioSource.PlayClipAtPoint(eatClip, Vector3.zero);
        int index = (bodyList.Count % 2 == 0) ? 0 : 1;//������ż��ѡ������ͼƬ
        GameObject body = Instantiate(bodyPrefab,new Vector3(2000,2000,0),Quaternion.identity);//�����ɵ�ʳ����ڿ������ĵط������ƶ������Move������ȥ����λ�ã�����ֱ�����������ϲ����������bug
        body.GetComponent<Image>().sprite = bodySprites[index];//ѡ������ͼƬ
        body.transform.SetParent(canvas, false);//false��ʾ���Զ�������������
        bodyList.Add(body.transform);
    }

    private void Die()
    {
        AudioSource.PlayClipAtPoint(dieClip, Vector3.zero);
        //���������ƶ�
        CancelInvoke();
        isDie = true;
        //����Ч��
        Vector3 dieEffectPos = new Vector3(0, 0, -5);
        Instantiate(dieEffect, dieEffectPos,transform.rotation);//�������٣���Ϊ�ܿ�����¼��س���


        //��¼���³����������
        //PlayerPrefs��������window��ע����ڣ��ü�ֵ�Ա��棬��߲����Ǽ������ұ��Ǽ�ֵ
        PlayerPrefs.SetInt("lastLength", MainUIController.Instance.length);
        PlayerPrefs.SetInt("lastScore", MainUIController.Instance.score);
        
        //�ж��Ƿ�Ϊ��ѳ����������
        //PlayerPrefs.GetInt("bestScore",0):�������û�б����bestScore����ô�᷵��0
        if (PlayerPrefs.GetInt("bestScore",0) < MainUIController.Instance.score)
        {
            PlayerPrefs.SetInt("bestLength", MainUIController.Instance.length);
            PlayerPrefs.SetInt("bestScore", MainUIController.Instance.score);
        }
        
        StartCoroutine(GameOver(1.5f));
    }

    IEnumerator GameOver(float time)
    {
        //�ȴ�timeʱ���
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(1);
    }
    #endregion
}
