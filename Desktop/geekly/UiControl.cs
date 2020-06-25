using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiControl : MonoBehaviour
{

    //スクロール用オブジェクト
    private Transform sc;

    //UIボタン用オブジェクト
    public GameObject nond;

    // 速度
    public float speed;
    float upSpeed;

    //フラグ
    bool lFlag;
    bool rFlag;
    bool dFlag;
    public bool mFlag;

    //UIボタン非表示フラグ
    bool nFlag;

    // hp減少用
    Image image;
    Image enemyimage;
    bool a;

    // 雇用状態フラグ
    public bool oneFlag;
    public bool twoFlag;
    public bool threeFlag;

    // 雇用状態変更
    public bool fflag;

    //イメージの位置座標
    public Transform pop;

    //リザルトフラグ
    bool rfrag;

    //クリックカウント
    public int ctr;

    //移動制限用
    float CP;

    void Start()
    {
        sc = GameObject.Find("Main Camera").GetComponent<UiControl>().transform;
        pop = GameObject.Find("Menu_BG").GetComponent<RectTransform>();

        speed = 3.5f;
        upSpeed = 10.0f;

        lFlag = false;
        rFlag = false;
        dFlag = false;
        nFlag = false;
        mFlag = false;

        // 切り出し位置取得
        image = GameObject.Find("Hp").GetComponent<Image>();
        enemyimage = GameObject.Find("EnemyHp").GetComponent<Image>();
        a = false;

        // 雇用状態の初期化
        oneFlag = false;
        twoFlag = false;
        threeFlag = false;

        // 雇用状態
        fflag = false;

        //UI非表示
        nond.gameObject.SetActive(false);

        //カウント初期化
        ctr = 0;
    }

    void Update()
    {
        //カメラのposition
        CP = sc.transform.position.x;

        if (CP <= 12.75)
        {
            if (Input.GetKey(KeyCode.D) || rFlag == true)
            {
                sc.Translate(Vector2.right * speed * Time.deltaTime);
            }
        }
        if (CP >= 0.05f)
        {
            if (Input.GetKey(KeyCode.A) || lFlag == true)
            {
                sc.Translate(Vector2.left * speed * Time.deltaTime);
            }
        }

        if (Input.GetKey(KeyCode.R) || dFlag == true)
        {
            sc.position = new Vector3(0.0f, 0.0f, -10.0f);
            dFlag = false;
        }

        //resultFlagから値を取得
        rfrag = GameObject.Find("S_Result").GetComponent<Result>().resultFlag;

        if (rfrag == false)
        {
            //UIBer移動
            if (mFlag == true)
            {
                //ウィンドウ展開
                if (pop.transform.position.y < 0f)//移動制限
                {
                    pop.Translate(Vector2.up * upSpeed);
                    if (pop.transform.position.y > 0f)
                    {
                        pop.transform.position = new Vector2(0.0f, 0.0f);
                    }
                }

                nond.gameObject.SetActive(true);

                nFlag = true;
                //if (nFlag)
                //{
                //    nond.gameObject.SetActive(true);
                //}
            }

            if (mFlag == false && nFlag == true)
            {
                nFlag = false;
                nond.gameObject.SetActive(false);

                //ウィンドウ縮小
                if (pop.transform.position.y > -84.0f)
                {
                    pop.Translate(Vector2.down * upSpeed);
                    if (pop.transform.position.y > -84.0f)
                    {
                        pop.transform.position = new Vector2(0.0f, -84.0f);
                    }
                }
            }
        }

        // hp減少
        if (!a)
        {
            image.fillAmount = image.fillAmount - 0.003f * Time.deltaTime;
            enemyimage.fillAmount = enemyimage.fillAmount - 0.003f * Time.deltaTime;
            //a = true;
        }

        // enemyhp減少

        // 雇用状態の変更
        fflag = GameObject.Find("Hire").GetComponent<Hire>().fFlag;

        if (fflag)
        {
            oneFlag = false;
            twoFlag = false;
            threeFlag = false;
            fflag = false;
        }
    }

    public void RightDown()
    {
        rFlag = true;
        //sc.position += new Vector3(5.0f, 0.0f, 0.0f) * Time.deltaTime;
    }
    public void RightUp()
    {
        rFlag = false;
    }

    public void LeftDown()
    {
        lFlag = true;
        //sc.position += new Vector3(-5.0f, 0.0f, 0.0f) * Time.deltaTime;
    }

    public void LeftUp()
    {
        lFlag = false;
    }

    public void defo()
    {
        dFlag = true;
    }

    public void Menu()
    {
        mFlag = true;
        ctr++;
        if (!mFlag && (ctr % 2) == 1)      //奇数
        {
            mFlag = true;
        }
        else if (mFlag && (ctr % 2) == 0)//偶数
        {
            mFlag = false;
        }
    }

    // UIボタンによる雇用状態の変更
    public void OneDown()
    {
        oneFlag = true;
        twoFlag = false;
        threeFlag = false;
    }

    public void TwoDown()
    {
        oneFlag = false;
        twoFlag = true;
        threeFlag = false;
    }

    public void ThreeDown()
    {
        oneFlag = false;
        twoFlag = false;
        threeFlag = true;
    }
}
