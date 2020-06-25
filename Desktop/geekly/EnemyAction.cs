using UnityEngine;
using System.Collections;
//using UnityEditor;                                // enumの動的割り当て

//[CustomEditor(typeof(EnemyAction))]               // enumの動的割り当て

public class EnemyAction : MonoBehaviour {

    // 速度、回転速度など
    private float       speed           = 1.5f;     // 速度
    private float       rotationSmooth  = 2.0f;     // 回転

    // 徘徊用
    private Vector3     TagePos;                    // 値の入れ物
    private float       chengeTarget    = 40.0f;    // この値より小さければ変更

    // 周回用
    public float        angle           = 360f;     //一秒当たりの回転角度

    // 情報取得用
    private Transform   player;                     // player情報取得

    // 時間経過用
    private float       interval        = 3;        // 移動間隔
    private float       timer;                      // 時間加算用

    // 敵の状態
    public enum Enemy_state
    {
        STATE_PRY = 0,                              // 詮索状態
        STATE_TRACKING,                             // 追跡状態
        STATE_COMBAT,                               // 戦闘状態
    };

    public Enemy_state  enemy_state;                // inspectorにある状態の入れ物
    
    public int          move;

    //モーション
    private Animator    animator;

    public bool EAttack;
    public int HP;
    private CountControl counter;

    // 範囲外の値
    float area0 = 48f;   // 上右
    float area1 = -48f;   // 下左

    // 範囲外に出た数
    public float AreaEnemy = 0;

    // 攻撃状態か判定用
    public Player playerstate;

    // enemyが攻撃された際の停止管理用
    private float counttimer;
    private bool flag = true;

    private void Start()
    {
        // プレイヤーの位置を取得
        // playerのタグの付いている値の取得、見つからなければnull
        // ゲームオブジェクトにアタッチされている Transform （読み取り専用） 
        player = GameObject.FindWithTag("Player").transform;

        // Randomで取得した値を代入
        TagePos = GetRandomPosition();

        animator = GetComponent<Animator>();

        EAttack = false;
        HP = 100;
        counter = GameObject.FindWithTag("Counter").GetComponent<CountControl>();   //コンポーネント設定
    }

    private void Update()
    {
        //時間が止まっているときは通さない
        if (Time.timeScale == 0.0f)
            return;

        AnimatorStateInfo animInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        //現在のアニメーションが指定の名前なら
        if (animInfo.shortNameHash == Animator.StringToHash("Attack"))
        {
            EAttack = true;
        }
        else
        {
            EAttack = false;
        }

        // 距離による追跡
        Vector3 Enemypos = transform.position;
        Vector3 Playerpos = player.transform.position;
        float dis = Vector3.Distance(Enemypos, Playerpos);

        // 目標地点との距離が小さければ、次のランダムな目標地点を設定する
        float sqrDistanceToTarget = Vector3.SqrMagnitude(transform.position - TagePos);
        if (sqrDistanceToTarget < chengeTarget)
        {
            TagePos = GetRandomPosition();
        }

        // 距離による状態変化
        if (dis <= 8.0f && dis > 3.5f)
        {
            enemy_state = Enemy_state.STATE_TRACKING;       // 追跡状態
        }
        else if (dis <= 3.5f)
        {
            enemy_state = Enemy_state.STATE_COMBAT;         // 戦闘状態
        }
        else
        {
            enemy_state = Enemy_state.STATE_PRY;            // 詮索状態
        }

        GetComponent<Animator>().SetBool("Attack", false);
        GetComponent<Animator>().SetBool("Damage", false);

        // playerの状態の取得用
        playerstate = GameObject.FindWithTag("Player").GetComponent<Player>();

        // 状態の変化による行動変化
        switch (enemy_state)
        {
            // 索敵状態
            case Enemy_state.STATE_PRY:
                {
                    // 目標地点の方向を向く
                    Quaternion targetRotation = Quaternion.LookRotation(TagePos - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmooth);

                    // 前方に前進
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    break;
                };
            // 追跡状態
            case Enemy_state.STATE_TRACKING:
                {
                    // player方向を向く
                    Quaternion TagetRot = Quaternion.LookRotation(player.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, TagetRot, Time.deltaTime * rotationSmooth);

                    // 前方に前進
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    break;
                }
            // 戦闘状態
            case Enemy_state.STATE_COMBAT:
                {
                    // player方向を向く
                    Quaternion TagetRot = Quaternion.LookRotation(player.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, TagetRot, Time.deltaTime * rotationSmooth);

                    // 時間の加算
                    timer += Time.deltaTime;

                    if (timer >= interval)
                    {
                        // minを含みmaxを含まず0~3の値の生成
                        move = Random.Range(0, 3);
                        timer = 0;
                    }

                    // 一応完成(動きが微妙) 
                    switch (move)
                    {
                        // 左移動
                        case 0:
                            transform.Translate(Vector3.left * speed * Time.deltaTime);
                            break;
                        // 右移動
                        case 1:
                            transform.Translate(Vector3.right * speed * Time.deltaTime);
                            break;
                        // 攻撃
                        case 2:
                            if (dis >= 1.0f)
                            {
                                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                                //animator(Attack)がfalseで1.5より近いとき
                                if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) == false && dis <= 1.5f)
                                {
                                    GetComponent<Animator>().SetBool("Attack", true);   //Attackをtrueに
                                }
                            }
                            if ((animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) == true && playerstate.playermode == Player.PLAYER.PLAYER_ATTACK)
                            {
                                //GetComponent<Animator>().SetBool("Attack", false);   //Attackをfalseに
                                GetComponent<Animator>().SetBool("Damage", true);   //Attackをfalseに
                                move = Random.Range(0, 3);
                            }
                            break;
                        case 3:
                            flag = false;
                            counttimer += Time.deltaTime;
                            float stop = 3f;
                            if (counttimer >= stop && flag == false)
                            {
                                transform.Translate(Vector3.zero);
                                counttimer = 0;
                                flag = true;
                            }
                            break;
                    }
                }
                break;
        }

        if (HP <= 0)
        {
            GameObject.Find("EnemyControl").GetComponent<EnemyControl>().count--;
            counter.count += 1;
            Destroy(this.gameObject);
        }

        // 敵のエリア外での生成&移動を破棄(未完成)
        if (area0 <= transform.position.x ||
            area1 >= transform.position.x ||
            area0 <= transform.position.z ||
            area1 >= transform.position.z)
        {
            Destroy(gameObject);
            AreaEnemy++;
        }
    }

    // Randomで値を取得
    public Vector3 GetRandomPosition()
    {
        float RanSizeX = Random.Range(-45.0f, 45.0f);
        float RanSizeZ = Random.Range(-45.0f, 45.0f);
        return new Vector3(RanSizeX, 0.0f, RanSizeZ);
    }

    // トリガーとの接触時に呼ばれるコールバック
    void OnTriggerEnter(Collider hit) //当たった時
    {
        // 接触対象はEnemyタグですか？
        if (hit.CompareTag("Player"))
        {
            if (this.EAttack == true)
            {
                Player player = GameObject.FindWithTag("Player").GetComponent<Player>();   //コンポーネント設定
                player.HP = player.HP - 1;
            }
        }
    }
}
