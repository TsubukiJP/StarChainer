using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public FightAreaManager fightAreaManager;
    public CharacterManager characterManager;
    public StarFieldManager starFieldManager;

    private GameObject player;                  //プレイヤー
    private GameObject enemy;                   //現在の敵

    public const int MAXSTAGE = 6;              //ステージ数
    private int currentStageNumber = 1;         //現在のステージ

    public enum TurnType { Player, Enemy };     //どちらのターンか（ターンの表[プレイヤー]・裏[敵]）
    private TurnType turn;                       //ターン
    private int turnNumber;                      //ターン数

    public SpellDataBase spellDataBase;                                    //スペルデータベース
    public CharacterDataBase characterDataBase;                            //キャラクターデータベース
    public List<int> enemyIDList = new List<int> { 1, 2, 3, 4, 5, 6 };     //ステージごとの敵のIDリスト


    
    //フェーズ
    public enum Phases
    {
        StartPhase,             //ステージ開始時のフェーズ（敵出現の演出、フィールド・手札の初期化など）
        PlayerChainPhase,       //プレイヤーが星を繋いでスペルを決めるフェーズ
        PlayerAttackPhase,      //スペルを発動し、敵にダメージを与えたりするフェーズ
        EnemyChainPhase,        //敵が星を繋ぐフェーズ
        EnemyAttackPhase,       //敵がスペルを発動するフェーズ
        WinnersShop,            //勝利した場合のショップ
        Result,                 //敗北した場合のリザルト
    }

    private Phases phase;

    //プロパティ
    public GameObject Player { get => player; set => player = value; }
    public GameObject Enemy { get => enemy; set => enemy = value; }
    public int CurrentStageNumber { get => currentStageNumber; set => currentStageNumber = value; }
    public Phases Phase { get => phase; set => phase = value; }
    public TurnType Turn { get => turn; set => turn = value; }
    public int TurnNumber { get => turnNumber; set => turnNumber = value; }

    void Awake()
    {
        phase = Phases.StartPhase;  //スタートフェーズ
        
        Player = characterManager.InstantiateCharacter(characterDataBase.characterList[0]); //プレイヤー生成
        Enemy = characterManager.InstantiateCharacter(characterDataBase.characterList[1]);  //敵生成
        
        fightAreaManager.ArrangementCharacter(player);  //プレイヤー配置
        fightAreaManager.ArrangementCharacter(enemy);   //敵配置
    }
    
    void Start()
    {

        fightAreaManager.RefreshStatusUI(Player);       //プレイヤーのステータスをセット
        fightAreaManager.RefreshStatusUI(Enemy);        //敵のステータスをセット
        starFieldManager.Initialize();                  //スターフィールドの初期化

        StartCoroutine("Battle");                       //コルーチン開始
    }

    public IEnumerator Battle()
    {
        while (true)
        {
            yield return null;
            Debug.Log(phase);
            switch (phase)
            {
                case Phases.StartPhase:     //スタートフェーズ
                    
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

                    phase = Phases.PlayerChainPhase;    //プレーヤーのチェーンフェーズへ
                    
                    break;

                case Phases.PlayerChainPhase:   //プレイヤーがスターを繋げるフェーズ

                    turn = TurnType.Player;                 //プレイヤーのターンにする
                    turnNumber++;                           //ターン数を1増やす
                    fightAreaManager.RefreshTurnIndicator(Turn, TurnNumber);    //ターンインジケーターを更新

                    //星を変えるメソッド

                    yield return new WaitUntil(() => starFieldManager.isAcceptButtonPushed);

                    starFieldManager.isAcceptButtonPushed = false;
                    phase = Phases.PlayerAttackPhase;        //プレイヤーのアタックフェーズへ

                    break;

                case Phases.PlayerAttackPhase:  //プレイヤーの攻撃フェーズ

                    yield return new WaitForSeconds(1.0f);

                    starFieldManager.PuchResetButton();     //スターフィールドをリセット
                    phase = Phases.EnemyChainPhase;         //敵のチェーンフェーズへ

                    break;

                case Phases.EnemyChainPhase:    //敵がスターを繋げるフェーズ

                    turn = TurnType.Enemy;                  //敵のターンにする
                    fightAreaManager.RefreshTurnIndicator(Turn, TurnNumber);    //ターンインジケーターを更新

                    //星を変えるメソッド

                    yield return new WaitForSeconds(1.0f);

                    phase = Phases.EnemyAttackPhase;        //敵のアタックフェーズへ

                    break;

                case Phases.EnemyAttackPhase:   //敵の攻撃フェーズ

                    yield return new WaitForSeconds(1.0f);

                    phase = Phases.PlayerChainPhase;        //プレイヤーのチェーンフェーズへ戻る

                    break;
                
            }

        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public Character GetEnemy()
    {
        Character currentEnemy = null;

        foreach (CharacterData enemy in characterDataBase.characterList)    //すべての敵について検索
        {
            if (enemy.characterID == CurrentStageNumber)                //もし現在のステージ番号と同じIDをもつ敵がいたら
            {
                //currentEnemy = enemy;                                   //現在のステージの敵に設定
            }
        }

        return currentEnemy;    //設定した敵を返す
    }

}
