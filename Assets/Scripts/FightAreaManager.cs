using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightAreaManager : MonoBehaviour
{


    public RectTransform fightArea;                         //�t�@�C�g�G���A
    public RectTransform statusArea;                        //�X�e�[�^�X�G���A

    [SerializeField] private GameObject turnMarker;         //�^�[���}�[�J�[�i�^�[�����v���C���[�E�G���ɉ����ĐF��ω��j
    [SerializeField] private RectTransform turnIndicator;   //�^�[���C���W�P�[�^�[
    [SerializeField] private GameObject turnText;           //�^�[�����e�L�X�g

    [SerializeField] private GameObject playerHpText;       //�v���C���[��HP�e�L�X�g
    [SerializeField] private GameObject playerMpText;       //�v���C���[��MP�e�L�X�g
    [SerializeField] private GameObject playerCpText;       //�v���C���[��CP�e�L�X�g
    [SerializeField] private GameObject enemyHpText;        //�G��HP�e�L�X�g
    [SerializeField] private GameObject enemyMpText;        //�G��MP�e�L�X�g
    [SerializeField] private GameObject enemyCpText;        //�G��CP�e�L�X�g


    //�L�����N�^�[���C���X�^���X�����Ĕz�u����
    public void ArrangementCharacter(GameObject characterGO)
    {

        characterGO.transform.SetParent(fightArea, false);      //�t�@�C�g�G���A�ɃL�����N�^�[��z�u�i�X�P�[���͈ˑ����Ȃ��j

        if (characterGO.GetComponent<Character>().characterType == CharacterType.Player)      //�v���C���[�̏ꍇ
        {
            Vector3 position = new Vector3(150, -100, 0);       //�L�����N�^�[�̈ʒu

            characterGO.transform.localPosition = position;
        }
        else if(characterGO.GetComponent<Character>().characterType == CharacterType.Enemy)   //�G�̏ꍇ
        {
            Vector3 position = new Vector3(900, -100, 0);       //�L�����N�^�[�̈ʒu
            characterGO.transform.localPosition = position;
        }

    }

    //�X�e�[�^�X�G���A�̃X�e�[�^�X���X�V
    public void RefreshStatusUI(GameObject characterGO)
    {
        Character character = characterGO.GetComponent<Character>();

        if(character.characterType == CharacterType.Player)         //�v���C���[�̏ꍇ
        {
            // (���݂�HP)/(�ő�HP)�Ƃ������ɕ\������
            playerHpText.GetComponent<TextMeshProUGUI>().SetText(character.hp.ToString() + "/" + character.maxHp.ToString());  //HP���Z�b�g
            playerMpText.GetComponent<TextMeshProUGUI>().SetText(character.mp.ToString() + "/" + character.maxMp.ToString());  //MP���Z�b�g
            playerCpText.GetComponent<TextMeshProUGUI>().SetText(character.cp.ToString() + "/" + character.maxCp.ToString());  //CP���Z�b�g
        }
        else if (character.characterType == CharacterType.Enemy)    //�G�̏ꍇ
        {
            enemyHpText.GetComponent<TextMeshProUGUI>().SetText(character.hp.ToString() + "/" + character.maxHp.ToString());  //HP���Z�b�g
            enemyMpText.GetComponent<TextMeshProUGUI>().SetText(character.mp.ToString() + "/" + character.maxMp.ToString());  //MP���Z�b�g
            enemyCpText.GetComponent<TextMeshProUGUI>().SetText(character.cp.ToString() + "/" + character.maxCp.ToString());  //CP���Z�b�g
        }
    }

    //�^�[�����X�e�[�^�X�G���A�ɔ��f����
    public void RefreshTurnIndicator(GameManager.TurnType turn, int turnNumber)
    {
        turnText.GetComponent<TextMeshProUGUI>().SetText(turnNumber.ToString());    //�^�[�������Z�b�g

        if (turn == GameManager.TurnType.Player) //�v���C���[�̃^�[���Ȃ�
        {
            turnMarker.GetComponent<Image>().color = new Color32(0, 0, 255, 180);  //�F��ɂ���
            turnIndicator.rotation = Quaternion.Euler(0, 0, 0); //�C���W�P�[�^�\�͍�����
        }
        else if(turn == GameManager.TurnType.Enemy)
        {
            turnMarker.GetComponent<Image>().color = new Color32(255, 0, 0, 180);  //�F��Ԃɂ���
            turnIndicator.rotation = Quaternion.Euler(0, 0, 180); //�C���W�P�[�^�\�͉E����
        }
    }

}
