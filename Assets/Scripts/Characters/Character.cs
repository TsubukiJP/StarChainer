using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType { Player, Enemy }; //キャラクタータイプ


public class Character : MonoBehaviour
{

    //画像・ステータス
    public int characterID;             //キャラクターID
    public string characterName;        //名前
    public CharacterType characterType; //タイプ
    public Vector2Int fieldSize;        //フィールドサイズ(横 x+1 マス, 縦 y+1 マス)
    public int maxHp;                   //最大HP
    public int maxMp;                   //最大MP
    public int maxCp;                   //最大CP (チェインポイント)
    public int hp;                      //現在のHP
    public int mp;                      //現在のMP
    public int cp;                      //現在のCP
    public List<SpellSlot> spellSlot;   //所持スペル


    public void AddSpell(Spell _spell, int _amount)     //スペルをスペルスロットに追加するメソッド
    {
        bool hasSpell = false;  //追加するスペルは所有済みか

        for(int i = 0; i < spellSlot.Count; i++)
        {
            if(spellSlot[i].spellName == _spell)        //もし持っているなら
            {
                spellSlot[i].AddAmount(_amount);    //数を増やす
                hasSpell = true;
                break;
            }
        }
        if (!hasSpell)  //もし持っていないなら
        {
            spellSlot.Add(new SpellSlot(_spell, _amount));  //新たにスペルを追加
        }
    }
}


