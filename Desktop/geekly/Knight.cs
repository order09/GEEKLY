using UnityEngine;
using System.Collections;

public class Knight : MonoBehaviour
{
    // ステータス
    private float hp;
    private float atk;
    private float atktime;
    private float range;
    private float speed;
    public int cost;

    // 位置、角度調整用
    private Vector2 move;
    //private Vector2 scale;

    // 状態
    enum knight_state
    {
        KNIGHT_MOVE = 0,
        KNIGHT_COMBAT,
        KNIGHT_DEATH
    }

    // 状態変数
    knight_state knightstate;

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

    // 敵Hpの取得
    //public float enemyhp;

    void Start()
    {
        // ステータス表(初期化)
        hp = 10.0f;
        atk = 10.0f;
        range = 0.6f;
        speed = 0.3f;
        cost = 30;
        knightstate = knight_state.KNIGHT_MOVE;

        // 移動,倍率調整用
        move = new Vector2(0.1f, 0.0f).normalized;

        // animator取得
        anim = GetComponent<Animator>();
        anim.SetBool("Combat", false);

        // 敵のhpの代入
        //enemyhp = 
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
            knightstate = knight_state.KNIGHT_COMBAT;   // 戦闘状態に変更
        }
        else
        {
            knightstate = knight_state.KNIGHT_MOVE;     // 移動状態に変更
        }

        // 範囲外に出ると死亡状態に変更
        if (transform.position.x >= 17.5f /*|| hp <= 0f*/)
        {
            knightstate = knight_state.KNIGHT_DEATH;    // 死亡状態に変更
        }

        // 死亡状態なら削除
        if (knightstate == knight_state.KNIGHT_DEATH)
        {
            Destroy(gameObject);
        }

        // 状態による行動変更
        if (knightstate == knight_state.KNIGHT_MOVE)
        {
            // スピードを代入する
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            anim.SetBool("Combat", false);              // 移動モーションに切り替え
        }
        else if (knightstate == knight_state.KNIGHT_COMBAT)
        {
            anim.SetBool("Combat", true);               // 戦闘モーションに切り替え
        }
    }

    // 最も近いオブジェクトの取得
    GameObject serch(GameObject player, string tag)
    {
        float dis = 0.0f;
        float neardis = 0.0f;

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

    // 最も近いenemyの比較
    //GameObject comparison(GameObject Aobj, GameObject Bobj, GameObject Cobj)
    //{
    //    // 最も近いenemy
    //    float near;
    //    // Knightとの距離を代入
    //    float eatok = Vector2.Distance(Aobj.transform.position, transform.position);
    //    float ebtok = Vector2.Distance(Bobj.transform.position, transform.position);
    //    float ectok = Vector2.Distance(Cobj.transform.position, transform.position);
    //
    //    // 比較開始
    //    if(eatok < ebtok)
    //    {
    //        near = eatok;
    //        nearobj = Aobj;
    //    }
    //    else
    //    {
    //        near = ebtok;
    //        nearobj = Bobj;
    //    }
    //
    //    if(near > ectok)
    //    {
    //        near = ectok;
    //        nearobj = Cobj;
    //    }
    //
    //    // 最も近い距離の代入
    //    return nearobj;
    //}
}
