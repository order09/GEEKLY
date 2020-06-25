using UnityEngine;
using System.Collections;

public class Archer : MonoBehaviour
{
    //private Transform enemyarcher;
    //private Transform enemycavalry;
    //private Transform enemyknight;

    // ステータス
    //private float hp;
    //private float atk;
    private float range;
    private float speed;
    public int cost;

    // 位置、角度調整用
    private Vector2 move;
    //private Vector2 scale;

    // 状態
    enum archer_state
    {
        ARCHER_MOVE = 0,
        ARCHER_COMBAT,
        ARCHER_DEATH
    }

    // 状態変数
    archer_state archerstate;

    // animator変更
    Animator anim;

    // 間隔
    float time;
    // archerで最も近いオブジェクト
    public GameObject Aobj;
    // cavalyで最も近いオブジェクト
    public GameObject Cobj;
    // knightで最も近いオブジェクト
    public GameObject Kobj;

    // 最も近いオブジェクトの指定(serch)
    GameObject target = null;

    // 最も近いオブジェクトの距離(comparison)
    public float nearobj;

    void Start()
    {
        //enemyarcher = GameObject.FindWithTag("EnemyArcher").transform;
        //enemycavalry = GameObject.FindWithTag("EnemyCavalry").transform;
        //enemyknight = GameObject.FindWithTag("EnemyKnight").transform;

        // ステータス表
        //hp = 10.0f;
        //atk = 10.0f;
        range = 1.5f;
        speed = 0.3f;
        cost = 30;
        archerstate = archer_state.ARCHER_MOVE;

        // 移動,倍率調整用
        move = new Vector2(0.1f, 0.0f).normalized;
        //scale = new Vector2(1.0f, 1.0f).normalized;
        //transform.localScale = scale;

        // animator取得
        anim = GetComponent<Animator>();
        anim.SetBool("Combat", false);
    }

    void Update()
    {
        //経過時間を取得
        time += Time.deltaTime;

        if (time >= 1.0f)
        {
            // 各オブジェクトで最も近かったオブジェクトの取得
            Aobj = serch(gameObject, "EnemyArcher");
            Cobj = serch(gameObject, "EnemyCavalry");
            Kobj = serch(gameObject, "EnemyKnight");

            //経過時間を初期化
            time = 0;
        }

        // 最も近いオブジェクトの判定
        nearobj = comparison(Aobj, Cobj, Kobj);

        // 距離による状態変化
        if (nearobj <= range)
        {
            archerstate = archer_state.ARCHER_COMBAT;
        }
        else
        {
            archerstate = archer_state.ARCHER_MOVE;
        }

        // 範囲外に出ると死亡状態に変更
        if (transform.position.x >= 17.5f)
        {
            archerstate = archer_state.ARCHER_DEATH;
        }

        // 状態による行動変更
        if (archerstate == archer_state.ARCHER_MOVE)
        {
            // スピードを代入する
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            anim.SetBool("Combat", false);              // 移動モーションに切り替え
        }
        else if (archerstate == archer_state.ARCHER_COMBAT)
        {
            anim.SetBool("Combat", true);               // 戦闘モーションに切り替え
        }
        else if (archerstate == archer_state.ARCHER_DEATH)
        {
            Destroy(gameObject);
        }
    }

    // 最も近いオブジェクトの取得
    GameObject serch(GameObject player, string tag)
    {
        float dis = 0.0f;
        float neardis = 0.0f;
        //GameObject target = null;

        // objにタグ指定されたGameObjectの代入
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
        {
            // 代入されたGameObjectのpositionと自身(player)のpositionの計算
            dis = Vector2.Distance(obj.transform.position, player.transform.position);

            // 代入されたGameObjectのpositionと自身(player)のpositionと重なっているまたは計算後より値が大きければ代入
            if (neardis == 0 || neardis > dis)
            {
                neardis = dis;
                target = obj;
            }
        }
        return target;
    }

    // 最も近いenemyの比較
    float comparison(GameObject Aobj, GameObject Bobj, GameObject Cobj)
    {
        // 最も近いenemy
        float near;
        // Knightとの距離を代入
        float eatok = Vector2.Distance(Aobj.transform.position, transform.position);
        float ebtok = Vector2.Distance(Bobj.transform.position, transform.position);
        float ectok = Vector2.Distance(Cobj.transform.position, transform.position);

        // 比較開始
        if (eatok < ebtok)
        {
            near = eatok;
        }
        else
        {
            near = ebtok;
        }

        if (near > ectok)
        {
            near = ectok;
        }

        // 最も近い距離の代入
        return near;
    }
}
