using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �����ʳ��ű�
/// ����������������ʼλ��������16���ͻ������ұߵ�ǽ������ʳ�����������ʼλ���������ɵ�λ��ֻ��(30,0,0),(60,0,0)����(450,0,0)��15���ط�
/// ͬ������ֻ��(-30, 0, 0),(-60, 0, 0)����(-270, 0, 0)��9���ط�
/// ����ֻ��(0,30,0),(0, 60, 0)����(0, 330, 0)��11���ط�
/// ����ֻ��(0,-30,0),(0, -60, 0)����(0, -330, 0)��11���ط�
/// ������ͷ����λ��(0,0,0)һ���ط�
/// ����λ����ʳ�����������ɵ�λ��:x��������25������y������23��
/// ��һ��ʳ�ﱻ�Ե�֮���������һ��ʳ��
/// </summary>
public class IceCream : MonoBehaviour
{
    //������
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

    //���ԣ�
    #region
    public int xRightLimit = 15;//����ֻ����15��
    public int xLeftLimit = 9;//����ֻ����9��
    public int yLimit = 11;//���¶�ֻ����11��
    public Sprite[] iceCreamSprites;//������б���ܾ��������

    #endregion

    //���ã�
    #region
    public GameObject iceCreamPrefab;
    public Transform iceCreamHolder;
    public GameObject rewardPrefab;
    #endregion

    //��ʼ��������
    #region
    private void Awake()
    {
        _instance = this;    
    }

    private void Start()
    {
        iceCreamHolder = GameObject.Find("IceCreamHolder").transform;
        CreateIceCream(false);//��һ�����ɲ���Ҫ����
    }
    #endregion


    /// <summary>
    /// ���ɱ����跽��
    /// </summary>
    /// <param name="isReward">�˴����ɱ������Ƿ�Ҫ��һ������Ŀ��</param>
    public void CreateIceCream(bool isReward)
    {
        //���ɱ���ܶ���
        GameObject iceCream = Instantiate(iceCreamPrefab);
        iceCream.transform.SetParent(iceCreamHolder, false);//false��ʾ����Canvas�Զ�ΪiceCream��������Scale

        //���ѡ������ͼƬ
        int index = Random.Range(0, iceCreamSprites.Length);
        iceCream.GetComponent<Image>().sprite = iceCreamSprites[index];

        //���ѡ������λ��
        int x = Random.Range(-xLeftLimit, xRightLimit);
        int y = Random.Range(-yLimit, yLimit);
        iceCream.transform.localPosition = new Vector3(x * 30, y * 30);

        if (isReward)
        {
            //���ɽ���Ŀ�����
            GameObject reward = Instantiate(rewardPrefab);
            reward.transform.SetParent(iceCreamHolder, false);//false��ʾ����Canvas�Զ�ΪiceCream��������Scale

            //���ѡ����Ŀ������λ��
            x = Random.Range(-xLeftLimit, xRightLimit);
            y = Random.Range(-yLimit, yLimit);
            reward.transform.localPosition = new Vector3(x * 30, y * 30);
        }
    }

    
}
