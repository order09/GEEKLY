using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour
{
    public GameObject EnemyPre;       // prefabの呼び出し用

    private float ENEMY_MAX = 15;     // 召喚数
    private float DOWN_COUNT = 6;     // 倒された限界値
    private float interval = 5;       // 召喚までの時間
    private float timer;              // 時間加算用

    public float count = 0;                  // 召喚用

    void Start()
    {
        // 初期化の際に出現
        Spawn();
    }

    void Update()
    {
        // 時間の加算
        timer += Time.deltaTime;

        // timerがinterval以下で実行
        if (timer >= interval)
        {
            if (count <= ENEMY_MAX - DOWN_COUNT)
            {
                Spawn();        // スポーン実行
            }
            timer = 0;      // タイマーの初期化
        }
    }

    void Spawn()
    {
        // ENEMY_MAX分enemyの召喚
        for (; count < ENEMY_MAX; count++)
        {
            // Randomでplayerの位置から
            float x = Random.Range( -45.0f, 45.0f);
            float z = Random.Range( -45.0f, 45.0f);

            // 値をposに代入
            Vector3 pos = new Vector3(x, 4.0f, z);

            // 出現
            GameObject.Instantiate(EnemyPre, pos, Quaternion.identity);
        }
    }
}
