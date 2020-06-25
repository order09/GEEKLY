using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hire : MonoBehaviour
{
    // 雇用オブジェクト取得用
    public GameObject archer;
    public GameObject cavalry;
    public GameObject knight;

    // 雇用資金オブジェクト
    private int archercost;
    private int cavalrycost;
    private int knightcost;

    // 雇用状態
    public enum hire_state
    {
        NOSELECT_FLAG = 0,
        ARCHER_FLAG,
        CAVALRY_FLAG,
        KNIGHT_FLAG,
    }

    // 雇用選択管理
    hire_state hirestate;

    bool aFlag;
    bool cFlag;
    bool kFlag;

    // 雇用手持ち初期資金
    public int cost;

    // 位置、角度の調整用
    private Vector2 pos;
    private Quaternion rot;

    // mouseの位置取得
    public Vector2 mouse;

    // UiControlの書き換え
    public bool fFlag;

    //召喚用SE
    public AudioClip Summon;
    AudioSource audioSource;

    void Start()
    {
        // 初期生成位置
        pos = new Vector2(-6.5f, 0.0f);
        rot = Quaternion.identity;

        // 手持ち初期資金の設定
        cost = 300;

        // 雇用費用の設定
        archercost = archer.GetComponent<Archer>().cost;
        cavalrycost = cavalry.GetComponent<Cavalry>().cost;
        knightcost = knight.GetComponent<Knight>().cost;

        // 兵士の選択(選択されていない状態)
        hirestate = hire_state.NOSELECT_FLAG;

        // 雇用状態初期化
        aFlag = false;
        cFlag = false;
        kFlag = false;

        // UiControlのoneFlag,twoFlag,threeFlag変更初期化
        fFlag = false;

        //召喚用SE
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        // Enemyが倒れたら資金の追加
        //if()
        // 資金の追加
        //cost = cost 

        // mouseの位置取得
        mouse = /*Input.mousePosition;*/Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 雇用資金より雇用費安いかどうか
        if (cost >= archercost ||
            cost >= cavalrycost ||
            cost >= knightcost)
        {
            // 雇用範囲内であれば生成
            if (mouse.y >= -3.6f && mouse.y <= 2.1f &&
                mouse.x >= -3.8f && mouse.x <= -2.5f)
            {
                // 雇用生成
                if (Input.GetMouseButtonDown(0) && Input.mousePosition.y >= 85.0f)
                {

                    Spawn();
                    GetComponent<AudioSource>().PlayOneShot(Summon);
                }
            }
        }

        // UiControlの雇用状態の取得...(失敗)取得してくるがキー番号での雇用状態変更が不可能になる
        // 理由...雇用状態はUiControlで変更しているためキー番号でのfalse化ができないかつ毎フレームtrueを代入しているため変わらない
        aFlag = GameObject.Find("S_UI").GetComponent<UiControl>().oneFlag;
        cFlag = GameObject.Find("S_UI").GetComponent<UiControl>().twoFlag;
        kFlag = GameObject.Find("S_UI").GetComponent<UiControl>().threeFlag;

        // UiControl書き換えかつ初期化用
        fFlag = GameObject.Find("S_UI").GetComponent<UiControl>().fflag;

        // 雇用状態選択
        if (Input.GetKeyDown(KeyCode.Alpha1) || aFlag == true)        // 数字(1)
        {
            hirestate = hire_state.ARCHER_FLAG;
            fFlag = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || cFlag == true)       // 数字(2)
        {
            hirestate = hire_state.CAVALRY_FLAG;
            fFlag = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || kFlag == true)       // 数字(3)
        {
            hirestate = hire_state.KNIGHT_FLAG;
            fFlag = true;
        }
    }

    // 状態変化による雇用
    void Spawn()
    {       
        switch(hirestate)
        {
            case hire_state.ARCHER_FLAG:
                {
                    GameObject.Instantiate(archer, mouse, rot);
                    cost = cost - archercost;
                    break;
                };
            case hire_state.CAVALRY_FLAG:
                {
                    GameObject.Instantiate(cavalry, mouse, rot);
                    cost = cost - cavalrycost;
                    break;
                };
            case hire_state.KNIGHT_FLAG:
                {
                    GameObject.Instantiate(knight, mouse, rot);
                    cost = cost - knightcost;
                    break;
                };
            case hire_state.NOSELECT_FLAG:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}