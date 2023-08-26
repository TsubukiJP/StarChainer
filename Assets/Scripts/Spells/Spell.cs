using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell : ScriptableObject
{
    public string spellName;                                //スペル名
    public List<Vector2Int> spellPattern = new();           //スペルのパターン（線のベクトル）
    [HideInInspector] public Vector2 startPoint = new();    //スペルの始点（格子座標）
    public int mpCost;                                      //マナコスト
    [HideInInspector] public int cpCost;                    //チェーンコスト
    public bool isRotatable;                                //回転可能か
    public int tier;                                        //ティア
    [TextArea]
    public string spellDiscription;                         //スペルの説明

    public Spell()                          //コンストラクタ
    {
        /*if (spellPattern.Count > 0)
        {
            startPoint.x = spellPattern[0].x;   //始点のx座標の設定
            startPoint.y = spellPattern[0].y;   //始点のy座標の設定
            cpCost = spellPattern.Count;        //線の長さをCPとする
        }*/
    }
    
    public void CastSpell(Character player, Character enemy)   //スペルの呼び出し
    {
        SpellEffect(player, enemy);     //スペルの効果を発動
    }

    protected virtual void SpellEffect(Character player, Character enemy)  //スペル効果（各スペルでオーバーライド）
    {
        Debug.LogWarning("スペル効果が設定されていません");
    }

}

[System.Serializable]
public class SpellSlot      //スペルスロット（スペルとその数を持つ）
{
    public Spell spellName; //スペル
    public int amount;      //数
    public SpellSlot(Spell _spell, int _amount) //コンストラクタ
    {
        spellName = _spell;
        amount    = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}

