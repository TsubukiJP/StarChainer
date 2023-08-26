using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class StarFieldManager : MonoBehaviour
{
    public GameManager gameManager;             //ゲームマネージャー

    public Character player;                    //プレイヤー
    public Character enemy;                     //敵

    public LineRenderer lineRenderer;           //ラインレンダラー
    public RectTransform mainStarField;         //メインスターフィールド
    public RectTransform subStarField;          //サブスターフィールド

    private Star currentStar;                   //現在の始点となっているスター
    private List<Star> connectedStars = new();  //接続済みのスター

    private Star[,] playerStarField;             //プレイヤーのスターフィールド
    private Star[,] enemyStarField;              //敵のスターフィールド

    //プロパティ
    public Star CurrentStar { get => currentStar; set => currentStar = value; }
    public List<Star> ConnectedStars { get => connectedStars; set => connectedStars = value; }
    public Star[,] PlayerStarField { get => playerStarField; set => playerStarField = value; }
    public Star[,] EnemyStarField { get => enemyStarField; set => enemyStarField = value; }

    private bool isHolding = false;             //ホールド判定
    public bool isAcceptButtonPushed = false;   //確定ボタンが押されたか

    [SerializeField] private GameObject acceptButton;             //決定ボタン
    [SerializeField] private GameObject retryButton;              //リセットボタン
    [SerializeField] private GameObject errorText;                //エラーテキスト
    [SerializeField] private GameObject spellDescriptionArea;     //スペル説明エリア UI
    [SerializeField] private GameObject activeSpellArea;          //有効スペルエリア UI





    void Update()
    {
        mainStarField.GetComponent<Image>().color = new Color32(255, 255, 255, 100);    //メインフィールドを白にする

        if (gameManager.Phase == GameManager.Phases.PlayerChainPhase)   //プレイヤーチェーンフェーズ中
        {

            mainStarField.GetComponent<Image>().color = new Color32(0, 255, 255, 100);  //メインフィールドを青くする

            //---------------------------------------------------------------
            //左クリックホールド中にスター間に線を描く
            //---------------------------------------------------------------

            errorText.SetActive(false); //エラーメッセージを非表示

            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);      //マウスの座標の変換（スクリーン座標→ワールド座標）
            cursorPosition.z = 0;                                                              //z座標は0に固定
            RaycastHit2D hit = Physics2D.Raycast(cursorPosition, Vector2.zero);                //マウスの判定

            if (!isHolding && connectedStars.Count == 0)    //接続済みのスターがないとき
            {
                if (hit.collider != null && hit.collider.CompareTag("Star") && Input.GetMouseButtonDown(0))    //スターをクリックしたとき
                {
                    Star hitStar = hit.collider.gameObject.GetComponent<Star>();

                    if (hitStar.isActive == false && hitStar.owner == Star.StarOwner.Player)   //プレイヤースターをクリックしたとき
                    {
                        isHolding = true;       //ホールド状態となる
                        SetCurrentStar(hit);    //クリックしたスターを現在のスターにして、有効化、有効済みスターリストに追加
                    }
                }
            }

            if (isHolding)  //ホールド中
            {
                if (CurrentStar != null)    //現在のスターがあるなら
                {
                    LineCreate(connectedStars.Count, CurrentStar.transform.position, cursorPosition);       //現在のスターとマウスを線で結ぶ 
                }
                else
                {
                    Debug.LogWarning("currentStar is null");
                }

                if (hit.collider != null && hit.collider.CompareTag("Star"))    //カーソルがスタ―と当たったとき
                {
                    Star hitStar = hit.collider.gameObject.GetComponent<Star>();

                    if (hitStar.isActive == false && hitStar.owner == Star.StarOwner.Player)   //そのスターが非有効状態のプレイヤースターなら
                    {
                        if (CanConnectStar(hit.collider.gameObject.GetComponent<Star>()))      //線が引けるかチェック
                        {
                            LineCreate(connectedStars.Count, CurrentStar.transform.position, hit.collider.transform.position);  //スター同士を線で結ぶ
                            SetCurrentStar(hit);    //当たったスターを現在のスターにして、有効化、有効済みスターリストに追加
                        }
                    }
                }
            }

            if (isHolding && Input.GetMouseButtonUp(0) && connectedStars.Count != 0)  //マウスを離したとき
            {
                isHolding = false;  //ホールド状態解除
                Debug.Log("Not holding");

                if (lineRenderer.positionCount > 0)     //もし線が引かれていたら
                {
                    lineRenderer.positionCount--;       //スターとカーソル間の線を切る（カーソルの座標を抜く）
                }

                acceptButton.SetActive(true);           //決定ボタン       :表示
                retryButton.SetActive(true);            //リセットボタン   :表示
                spellDescriptionArea.SetActive(false);  //スペル説明UI     :非表示
                activeSpellArea.SetActive(true);        //有効スペルUI     :表示

                if (ConnectedStars.Count == 1)          //線が結ばれていない場合
                {
                    PuchResetButton();                  //そのままリセット
                }

            }
        }

    }

    //初期化
    public void Initialize()
    {
        //プレイヤー・敵の情報をGameManagerから受け取る
        player = gameManager.Player.GetComponent<Character>();      //プレイヤーをゲームマネージャーから取得
        enemy = gameManager.Enemy.GetComponent<Character>();       //敵をゲームマネージャーから取得


        lineRenderer.positionCount = 0; //線の初期化

        PlayerStarField = InitializeStarField(player.fieldSize, player);    //プレイヤーのスターフィールド
        GenerateStarFieldToMain(PlayerStarField);                           //メインフィールドに配置

        EnemyStarField = InitializeStarField(enemy.fieldSize, enemy);       //敵のスターフィールド
        GenerateStarFieldToSub(EnemyStarField);                             //サブフィールドに配置
    }


    //スターフィールドの初期化
    Star[,] InitializeStarField(Vector2Int fieldSize, Character character)
    {
        int maxX = fieldSize.x;                       //x座標最大値
        int maxY = fieldSize.y;                       //y座標最大値
        Star[,] starField = new Star[maxX, maxY];     //スターを格納する配列
        int starCount = 1;                            //スターの数（通し番号用）

        for (int y = 0; y < maxY; y++)
        {
            for(int x = 0; x < maxX; x++)
            {
                if((x + y) % 2 == 0)            //x座標とy座標の偶奇が一致する場合
                {
                    GameObject star = null;     //ゲームオブジェクトを用意   

                    if (character.characterType == CharacterType.Player)                //キャラクターがプレイヤーの場合
                    {
                        star = Instantiate(Star.playerNormalStarPrefab);                //プレイヤーのスターのプレハブを取得、インスタンス化
                    }
                    else if(character.characterType == CharacterType.Enemy)             //キャラクターが敵の場合
                    {
                        star = Instantiate(Star.enemyNormalStarPrefab);                 //敵のスターのプレハブを取得、インスタンス化
                    }
                    star.AddComponent<Star>();                                          //オブジェクトにスタークラスを追加
                    star.GetComponent<Star>().InitializeStar(x, y);                     //スターを初期化
                    star.name = character.characterType + "Star_" + starCount;          //スターに通し番号をつける
                    star.GetComponent<Star>().gridPosition.x = x;                       //格子座標xを設定
                    star.GetComponent<Star>().gridPosition.y = y;                       //格子座標yを設定

                    if (character.characterType == CharacterType.Player)                //もしキャラクターがプレイヤーなら
                    {
                        star.GetComponent<Star>().owner = Star.StarOwner.Player;        //スターの所有者をプレイヤーにする
                    }
                    else if (character.characterType == CharacterType.Player)           //もしキャラクターが敵なら
                    {
                        star.GetComponent<Star>().owner = Star.StarOwner.Enemy;         //スターの所有者を敵にする
                    }

                    starField[x, y] = star.GetComponent<Star>();                        //格子座標(x, y)にスターを配置
                    starCount++;                                                        //通し番号を増やす
                }
                else
                {
                    starField[x, y] = null;
                }
            }
        }

        return starField;
    }



    //スターフィールドの生成（メインフィールド）
    void GenerateStarFieldToMain(Star[,] starField)
    {

        float fieldWidth    = mainStarField.rect.width;     //メインスターフィールドの幅
        float fieldHeight   = mainStarField.rect.height;    //メインスターフィールドの高さ

        float topMargin     = 40.0f;                        //上の余白
        float bottomMargin  = 80.0f;                        //下の余白
        float leftMargin    = 100.0f;                       //左の余白
        float rightMargin   = 100.0f;                       //右の余白

        float horizontalSpacing   = (fieldWidth - (leftMargin + rightMargin)) / ((starField.GetLength(0) - 1) / 2);  //横方向のスターの間隔
        float verticalSpacing = (fieldHeight -  (topMargin + bottomMargin)) / ((starField.GetLength(1) - 1) / 2);    //縦方向のスターの間隔

        Vector3 starScale = new Vector3(18, 18, 18);        //スターの大きさ

        for (int y = 0; y < starField.GetLength(1); y++)
        {
            for (int x = 0; x < starField.GetLength(0); x++)
            {
                Star star = starField[x, y];

                if (star != null)
                {
                    star.transform.SetParent(mainStarField);                //メインスターフィールドを親オブジェクトに指定
                    
                    Vector3 position = new();                               //スターの座標
                    position.x = leftMargin   + horizontalSpacing * x / 2;  //スターのx座標
                    position.y = bottomMargin + verticalSpacing * y / 2;    //スターのy座標
                    position.z = -20.0f;                                    //スターのz座標

                    star.transform.localPosition = position;                //ローカル座標に変換
                    star.transform.localScale = starScale;                  //スケールの調整
                }
            }
        }

    }


    //スターフィールドの生成（サブフィールド）
    void GenerateStarFieldToSub(Star[,] starField)
    {

        float fieldWidth = subStarField.rect.width;     //サブスターフィールドの幅
        float fieldHeight = subStarField.rect.height;   //サブスターフィールドの高さ

        float topMargin = 30.0f;                        //上の余白
        float bottomMargin = 30.0f;                     //下の余白
        float leftMargin = 50.0f;                       //左の余白
        float rightMargin = 50.0f;                      //右の余白
        float horizontalSpacing = (fieldWidth - (leftMargin + rightMargin)) / ((starField.GetLength(0) - 1) / 2);  //横方向のスターの間隔
        float verticalSpacing = (fieldHeight - (topMargin + bottomMargin)) / ((starField.GetLength(1) - 1) / 2);   //縦方向のスターの間隔

        Vector3 starScale = new Vector3(10, 10, 10);    //スターの大きさ

        for (int y = 0; y < starField.GetLength(1); y++)
        {
            for (int x = 0; x < starField.GetLength(0); x++)
            {
                Star star = starField[x, y];

                if (star != null)
                {
                    star.transform.SetParent(subStarField);                 //サブスターフィールドを親オブジェクトに指定

                    Vector3 position = new();                               //スターの座標
                    position.x = leftMargin + horizontalSpacing * x / 2;    //スターのx座標
                    position.y = bottomMargin + verticalSpacing * y / 2;    //スターのy座標
                    position.z = -20.0f;                                    //スターのz座標

                    star.transform.localPosition = position;                //ローカル座標に変換
                    star.transform.localScale = starScale;                  //スケールの調整
                }
            }
        }

    }


    //当たったスターを現在のスターに設定する
    void SetCurrentStar(RaycastHit2D hit)
    {
        //当たったスターを現在のスターにして、有効化、有効済みスターリストに追加
        CurrentStar = hit.collider.gameObject.GetComponent<Star>();         //当たったスターを現在のスターに設定
        hit.collider.gameObject.GetComponent<Star>().ActivateStar();        //当たったスターを有効化
        connectedStars.Add(hit.collider.gameObject.GetComponent<Star>());   //当たったスターを接続済みのスターリストに追加

        //Debug.Log("スター" + connectedStars.Count + ":" + CurrentStar + CurrentStar.isActive);
    }


    //線の生成する
    void LineCreate(int startIndex, Vector3 start, Vector3 end) //(始点の番号, 始点の座標, 終点の座標)
    {
        if (lineRenderer.positionCount < startIndex + 1)  // 頂点数が足りない場合に追加(星2つであればカーソルと合わせて3つの頂点が必要)
        {
            lineRenderer.positionCount = startIndex + 1;
        }
        lineRenderer.SetPosition(startIndex - 1, start);    //始点の設定
        lineRenderer.SetPosition(startIndex, end);      //終点の設定
    }
    

    //スターが遠すぎないか、線が交差しないかを確認する
    bool CanConnectStar(Star targetStar)
    {

        int currentStarX = CurrentStar.gridPosition.x;  //現在のスターの格子座標x
        int currentStarY = CurrentStar.gridPosition.y;  //現在のスターの格子座標y
        int targetStarX  = targetStar.gridPosition.x;   //線を引こうとしているスターの格子座標x
        int targetStarY  = targetStar.gridPosition.y;   //線を引こうとしているスターの格子座標y
        int distance = (int)Mathf.Pow((currentStarX - targetStarX),2) + (int)Mathf.Pow((currentStarY - targetStarY),2);     //2スター間の距離の2乗
        //Debug.Log(distance);

        if (distance == 2 || distance == 4) //距離の2乗が2か4の場合 
        {
            if (ConnectedStars.Count == 1)   //星が1つのみの場合trueを返す
            {
                return true;
            }

            //線が交差していないか確認
            for (var i = 1; i < ConnectedStars.Count; i++)   //スター1→スター2、スター2→スター3、...、スターn-1→スターnの線にスターn→スターn+1の線が交差しないかを確認
            {
                Vector3 starA = ConnectedStars[i - 1].GetComponent<Transform>().position;   //判定する線の始点の座標A
                Vector3 starB = ConnectedStars[i].GetComponent<Transform>().position;       //判定する線の終点の座標B
                Vector3 starC = CurrentStar.GetComponent<Transform>().position;             //現在のスターの座標C
                Vector3 starD = targetStar.GetComponent<Transform>().position;              //線を引こうとしているスターの座標D

                float ta = (starD.x - starC.x) * (starA.y - starC.y) - (starD.y - starC.y) * (starA.x - starC.x); //直線CDの式にstarAの座標を代入
                float tb = (starD.x - starC.x) * (starB.y - starC.y) - (starD.y - starC.y) * (starB.x - starC.x); //直線CDの式にstarBの座標を代入
                float tc = (starB.x - starA.x) * (starC.y - starA.y) - (starB.y - starA.y) * (starC.x - starA.x); //直線ABの式にstarCの座標を代入
                float td = (starB.x - starA.x) * (starD.y - starA.y) - (starB.y - starA.y) * (starD.x - starA.x); //直線ABの式にstarDの座標を代入

                //交差する条件はta * tb < 0 かつ　tc * td < 0

                if (!(ta * tb < 0 && tc * td < 0)) //交差しない場合
                {
                    if (i == ConnectedStars.Count - 1) //すべての既存の線について交差していない場合
                    {
                        return true; //trueを返す
                    }
                    else //まだすべての既存の線について確認していない場合
                    {
                        continue;   //繰り返しを続ける
                    }
                }
                else //交差する場合
                {
                    break; //繰り返し文を抜け出してfalseを返す
                }

            }
        }
        else
        {
            Debug.Log("The star is too far.");
            errorText.GetComponent<TextMeshProUGUI>().SetText("スターが遠すぎます");    //エラーメッセージ
            errorText.SetActive(true);                                                  //エラーメッセージを表示
            return false;
        }

        Debug.Log("Intersected");
        errorText.GetComponent<TextMeshProUGUI>().SetText("ラインが交差しています");    //エラーメッセージ
        errorText.SetActive(true);                                                      //エラーメッセージを表示
        return false;
    }


    //決定ボタンを押したとき
    public void PushAcceptButton()
    {
        isAcceptButtonPushed = true;
    }

    //リセットボタンを押したとき
    public void PuchResetButton()
    {
        foreach(var star in connectedStars) //接続されていたスターを無効化
        {
            star.DeactivateStar();
        }

        ConnectedStars.Clear();                 //接続したスターをクリア
        CurrentStar = null;                     //現在のスターをクリア
        lineRenderer.positionCount = 0;         //線をクリア

        acceptButton.SetActive(false);          //決定ボタン       :非表示
        retryButton.SetActive(false);           //リセットボタン   :非表示
        spellDescriptionArea.SetActive(true);   //スペル説明UI     :表示
        activeSpellArea.SetActive(false);       //有効スペルUI     :非表示

        Debug.Log("Reset");
    }





}
