using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightAreaManager : MonoBehaviour
{


    public RectTransform fightArea;                         //ファイトエリア
    public RectTransform statusArea;                        //ステータスエリア

    [SerializeField] private GameObject turnMarker;         //ターンマーカー（ターンがプレイヤー・敵かに応じて色を変化）
    [SerializeField] private RectTransform turnIndicator;   //ターンインジケーター
    [SerializeField] private GameObject turnText;           //ターン数テキスト

    [SerializeField] private GameObject playerHpText;       //プレイヤーのHPテキスト
    [SerializeField] private GameObject playerMpText;       //プレイヤーのMPテキスト
    [SerializeField] private GameObject playerCpText;       //プレイヤーのCPテキスト
    [SerializeField] private GameObject enemyHpText;        //敵のHPテキスト
    [SerializeField] private GameObject enemyMpText;        //敵のMPテキスト
    [SerializeField] private GameObject enemyCpText;        //敵のCPテキスト


    //キャラクターをインスタンス化して配置する
    public void ArrangementCharacter(GameObject characterGO)
    {

        characterGO.transform.SetParent(fightArea, false);      //ファイトエリアにキャラクターを配置（スケールは依存しない）

        if (characterGO.GetComponent<Character>().characterType == CharacterType.Player)      //プレイヤーの場合
        {
            Vector3 position = new Vector3(150, -100, 0);       //キャラクターの位置

            characterGO.transform.localPosition = position;
        }
        else if(characterGO.GetComponent<Character>().characterType == CharacterType.Enemy)   //敵の場合
        {
            Vector3 position = new Vector3(900, -100, 0);       //キャラクターの位置
            characterGO.transform.localPosition = position;
        }

    }

    //ステータスエリアのステータスを更新
    public void RefreshStatusUI(GameObject characterGO)
    {
        Character character = characterGO.GetComponent<Character>();

        if(character.characterType == CharacterType.Player)         //プレイヤーの場合
        {
            // (現在のHP)/(最大HP)という風に表示する
            playerHpText.GetComponent<TextMeshProUGUI>().SetText(character.hp.ToString() + "/" + character.maxHp.ToString());  //HPをセット
            playerMpText.GetComponent<TextMeshProUGUI>().SetText(character.mp.ToString() + "/" + character.maxMp.ToString());  //MPをセット
            playerCpText.GetComponent<TextMeshProUGUI>().SetText(character.cp.ToString() + "/" + character.maxCp.ToString());  //CPをセット
        }
        else if (character.characterType == CharacterType.Enemy)    //敵の場合
        {
            enemyHpText.GetComponent<TextMeshProUGUI>().SetText(character.hp.ToString() + "/" + character.maxHp.ToString());  //HPをセット
            enemyMpText.GetComponent<TextMeshProUGUI>().SetText(character.mp.ToString() + "/" + character.maxMp.ToString());  //MPをセット
            enemyCpText.GetComponent<TextMeshProUGUI>().SetText(character.cp.ToString() + "/" + character.maxCp.ToString());  //CPをセット
        }
    }

    //ターンをステータスエリアに反映する
    public void RefreshTurnIndicator(GameManager.TurnType turn, int turnNumber)
    {
        turnText.GetComponent<TextMeshProUGUI>().SetText(turnNumber.ToString());    //ターン数をセット

        if (turn == GameManager.TurnType.Player) //プレイヤーのターンなら
        {
            turnMarker.GetComponent<Image>().color = new Color32(0, 0, 255, 180);  //色を青にする
            turnIndicator.rotation = Quaternion.Euler(0, 0, 0); //インジケータ―は左向き
        }
        else if(turn == GameManager.TurnType.Enemy)
        {
            turnMarker.GetComponent<Image>().color = new Color32(255, 0, 0, 180);  //色を赤にする
            turnIndicator.rotation = Quaternion.Euler(0, 0, 180); //インジケータ―は右向き
        }
    }

}
