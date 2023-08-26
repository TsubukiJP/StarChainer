using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject InstantiateCharacter(CharacterData characterData) //�f�[�^�����ƂɃL�����N�^�[�𐶐�
    {
        GameObject characterGO = Instantiate(characterData.characterPrefab);    //�v���n�u����C���X�^���X��
        Character character = characterGO.AddComponent<Character>();            //�Q�[���I�u�W�F�N�g�ɃL�����N�^�[�N���X��ǉ�
        character.characterID = characterData.characterID;                      //ID
        character.characterName = characterData.characterName;                  //���O
        character.characterType = characterData.characterType;                  //�^�C�v
        character.fieldSize = characterData.fieldSize;                          //�t�B�[���h�T�C�Y
        character.maxHp = characterData.maxHp;                                  //�ő�HP
        character.maxMp = characterData.maxMp;                                  //�ő�MP
        character.maxCp = characterData.maxCp;                                  //�ő�CP (�`�F�C���|�C���g)
        character.hp = character.maxHp;                                         //HP
        character.mp = character.maxMp;                                         //MP
        character.cp = character.maxCp;                                         //CP
        character.spellSlot = new List<SpellSlot>(characterData.spellSlot);     //���L�X�y��

        return characterGO;     //�Q�[���I�u�W�F�N�g��Ԃ�
    }
}
