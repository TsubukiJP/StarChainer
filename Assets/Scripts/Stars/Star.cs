using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public enum StarType { Normal, Mana, Heal, Damage }  //スターのタイプ
    public StarType type;                               //タイプ
    public enum StarOwner { Player, Enemy }             //スターの所有者
    public StarOwner owner;                             //所有者

    public bool isActive;                               //有効状態
    public bool isTypeChangeable;                       //タイプ変更可能か
    public bool isStartingPointOfSpell;                 //スペルの始点か
    public Vector2Int gridPosition;                     //スターの格子座標

    //スターのプレハブ
    public static GameObject playerNormalStarPrefab = (GameObject)Resources.Load("Prefabs/PlayerNormalStar");     //プレイヤーのスター
    public static GameObject enemyNormalStarPrefab = (GameObject)Resources.Load("Prefabs/EnemyNormalStar");      //敵のスター
    public static GameObject manaStarPrefab;                   //マナスター
    public static GameObject healStarPrefab;                   //ヒールスター
    public static GameObject DamageStarPrefab;                 //ダメージスター

    void Start()
    {

    }

    public void InitializeStar(int x, int y)            //スターの初期化
    {
        type = StarType.Normal;                         //初期状態はノーマル
        isActive = false;                               //初期状態は非アクティブ
        isTypeChangeable = true;                        //初期状態はタイプ変更可能
        gridPosition.x = x;                             //配置する格子のx座標
        gridPosition.y = y;                             //配置する格子のy座標
    }


    public void ChangeType(StarType newType)    //タイプの変更
    {
        type = newType;
    }

    public void ActivateStar()                  //スターの有効化
    {
        isActive = true;
    }

    public void DeactivateStar()                //スターの無効化
    {
        isActive = false;
    }
}
