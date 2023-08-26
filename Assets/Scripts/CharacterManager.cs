using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject InstantiateCharacter(CharacterData characterData) //データをもとにキャラクターを生成
    {
        GameObject characterGO = Instantiate(characterData.characterPrefab);    //プレハブからインスタンス化
        Character character = characterGO.AddComponent<Character>();            //ゲームオブジェクトにキャラクタークラスを追加
        character.characterID = characterData.characterID;                      //ID
        character.characterName = characterData.characterName;                  //名前
        character.characterType = characterData.characterType;                  //タイプ
        character.fieldSize = characterData.fieldSize;                          //フィールドサイズ
        character.maxHp = characterData.maxHp;                                  //最大HP
        character.maxMp = characterData.maxMp;                                  //最大MP
        character.maxCp = characterData.maxCp;                                  //最大CP (チェインポイント)
        character.hp = character.maxHp;                                         //HP
        character.mp = character.maxMp;                                         //MP
        character.cp = character.maxCp;                                         //CP
        character.spellSlot = new List<SpellSlot>(characterData.spellSlot);     //所有スペル

        return characterGO;     //ゲームオブジェクトを返す
    }
}
