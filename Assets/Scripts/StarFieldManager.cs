using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class StarFieldManager : MonoBehaviour
{
    public GameManager gameManager;             //�Q�[���}�l�[�W���[

    public Character player;                    //�v���C���[
    public Character enemy;                     //�G

    public LineRenderer lineRenderer;           //���C�������_���[
    public RectTransform mainStarField;         //���C���X�^�[�t�B�[���h
    public RectTransform subStarField;          //�T�u�X�^�[�t�B�[���h

    private Star currentStar;                   //���݂̎n�_�ƂȂ��Ă���X�^�[
    private List<Star> connectedStars = new();  //�ڑ��ς݂̃X�^�[

    private Star[,] playerStarField;             //�v���C���[�̃X�^�[�t�B�[���h
    private Star[,] enemyStarField;              //�G�̃X�^�[�t�B�[���h

    //�v���p�e�B
    public Star CurrentStar { get => currentStar; set => currentStar = value; }
    public List<Star> ConnectedStars { get => connectedStars; set => connectedStars = value; }
    public Star[,] PlayerStarField { get => playerStarField; set => playerStarField = value; }
    public Star[,] EnemyStarField { get => enemyStarField; set => enemyStarField = value; }

    private bool isHolding = false;             //�z�[���h����
    public bool isAcceptButtonPushed = false;   //�m��{�^���������ꂽ��

    [SerializeField] private GameObject acceptButton;             //����{�^��
    [SerializeField] private GameObject retryButton;              //���Z�b�g�{�^��
    [SerializeField] private GameObject errorText;                //�G���[�e�L�X�g
    [SerializeField] private GameObject spellDescriptionArea;     //�X�y�������G���A UI
    [SerializeField] private GameObject activeSpellArea;          //�L���X�y���G���A UI





    void Update()
    {
        mainStarField.GetComponent<Image>().color = new Color32(255, 255, 255, 100);    //���C���t�B�[���h�𔒂ɂ���

        if (gameManager.Phase == GameManager.Phases.PlayerChainPhase)   //�v���C���[�`�F�[���t�F�[�Y��
        {

            mainStarField.GetComponent<Image>().color = new Color32(0, 255, 255, 100);  //���C���t�B�[���h�������

            //---------------------------------------------------------------
            //���N���b�N�z�[���h���ɃX�^�[�Ԃɐ���`��
            //---------------------------------------------------------------

            errorText.SetActive(false); //�G���[���b�Z�[�W���\��

            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);      //�}�E�X�̍��W�̕ϊ��i�X�N���[�����W�����[���h���W�j
            cursorPosition.z = 0;                                                              //z���W��0�ɌŒ�
            RaycastHit2D hit = Physics2D.Raycast(cursorPosition, Vector2.zero);                //�}�E�X�̔���

            if (!isHolding && connectedStars.Count == 0)    //�ڑ��ς݂̃X�^�[���Ȃ��Ƃ�
            {
                if (hit.collider != null && hit.collider.CompareTag("Star") && Input.GetMouseButtonDown(0))    //�X�^�[���N���b�N�����Ƃ�
                {
                    Star hitStar = hit.collider.gameObject.GetComponent<Star>();

                    if (hitStar.isActive == false && hitStar.owner == Star.StarOwner.Player)   //�v���C���[�X�^�[���N���b�N�����Ƃ�
                    {
                        isHolding = true;       //�z�[���h��ԂƂȂ�
                        SetCurrentStar(hit);    //�N���b�N�����X�^�[�����݂̃X�^�[�ɂ��āA�L�����A�L���ς݃X�^�[���X�g�ɒǉ�
                    }
                }
            }

            if (isHolding)  //�z�[���h��
            {
                if (CurrentStar != null)    //���݂̃X�^�[������Ȃ�
                {
                    LineCreate(connectedStars.Count, CurrentStar.transform.position, cursorPosition);       //���݂̃X�^�[�ƃ}�E�X����Ō��� 
                }
                else
                {
                    Debug.LogWarning("currentStar is null");
                }

                if (hit.collider != null && hit.collider.CompareTag("Star"))    //�J�[�\�����X�^�\�Ɠ��������Ƃ�
                {
                    Star hitStar = hit.collider.gameObject.GetComponent<Star>();

                    if (hitStar.isActive == false && hitStar.owner == Star.StarOwner.Player)   //���̃X�^�[����L����Ԃ̃v���C���[�X�^�[�Ȃ�
                    {
                        if (CanConnectStar(hit.collider.gameObject.GetComponent<Star>()))      //���������邩�`�F�b�N
                        {
                            LineCreate(connectedStars.Count, CurrentStar.transform.position, hit.collider.transform.position);  //�X�^�[���m����Ō���
                            SetCurrentStar(hit);    //���������X�^�[�����݂̃X�^�[�ɂ��āA�L�����A�L���ς݃X�^�[���X�g�ɒǉ�
                        }
                    }
                }
            }

            if (isHolding && Input.GetMouseButtonUp(0) && connectedStars.Count != 0)  //�}�E�X�𗣂����Ƃ�
            {
                isHolding = false;  //�z�[���h��ԉ���
                Debug.Log("Not holding");

                if (lineRenderer.positionCount > 0)     //��������������Ă�����
                {
                    lineRenderer.positionCount--;       //�X�^�[�ƃJ�[�\���Ԃ̐���؂�i�J�[�\���̍��W�𔲂��j
                }

                acceptButton.SetActive(true);           //����{�^��       :�\��
                retryButton.SetActive(true);            //���Z�b�g�{�^��   :�\��
                spellDescriptionArea.SetActive(false);  //�X�y������UI     :��\��
                activeSpellArea.SetActive(true);        //�L���X�y��UI     :�\��

                if (ConnectedStars.Count == 1)          //�������΂�Ă��Ȃ��ꍇ
                {
                    PuchResetButton();                  //���̂܂܃��Z�b�g
                }

            }
        }

    }

    //������
    public void Initialize()
    {
        //�v���C���[�E�G�̏���GameManager����󂯎��
        player = gameManager.Player.GetComponent<Character>();      //�v���C���[���Q�[���}�l�[�W���[����擾
        enemy = gameManager.Enemy.GetComponent<Character>();       //�G���Q�[���}�l�[�W���[����擾


        lineRenderer.positionCount = 0; //���̏�����

        PlayerStarField = InitializeStarField(player.fieldSize, player);    //�v���C���[�̃X�^�[�t�B�[���h
        GenerateStarFieldToMain(PlayerStarField);                           //���C���t�B�[���h�ɔz�u

        EnemyStarField = InitializeStarField(enemy.fieldSize, enemy);       //�G�̃X�^�[�t�B�[���h
        GenerateStarFieldToSub(EnemyStarField);                             //�T�u�t�B�[���h�ɔz�u
    }


    //�X�^�[�t�B�[���h�̏�����
    Star[,] InitializeStarField(Vector2Int fieldSize, Character character)
    {
        int maxX = fieldSize.x;                       //x���W�ő�l
        int maxY = fieldSize.y;                       //y���W�ő�l
        Star[,] starField = new Star[maxX, maxY];     //�X�^�[���i�[����z��
        int starCount = 1;                            //�X�^�[�̐��i�ʂ��ԍ��p�j

        for (int y = 0; y < maxY; y++)
        {
            for(int x = 0; x < maxX; x++)
            {
                if((x + y) % 2 == 0)            //x���W��y���W�̋���v����ꍇ
                {
                    GameObject star = null;     //�Q�[���I�u�W�F�N�g��p��   

                    if (character.characterType == CharacterType.Player)                //�L�����N�^�[���v���C���[�̏ꍇ
                    {
                        star = Instantiate(Star.playerNormalStarPrefab);                //�v���C���[�̃X�^�[�̃v���n�u���擾�A�C���X�^���X��
                    }
                    else if(character.characterType == CharacterType.Enemy)             //�L�����N�^�[���G�̏ꍇ
                    {
                        star = Instantiate(Star.enemyNormalStarPrefab);                 //�G�̃X�^�[�̃v���n�u���擾�A�C���X�^���X��
                    }
                    star.AddComponent<Star>();                                          //�I�u�W�F�N�g�ɃX�^�[�N���X��ǉ�
                    star.GetComponent<Star>().InitializeStar(x, y);                     //�X�^�[��������
                    star.name = character.characterType + "Star_" + starCount;          //�X�^�[�ɒʂ��ԍ�������
                    star.GetComponent<Star>().gridPosition.x = x;                       //�i�q���Wx��ݒ�
                    star.GetComponent<Star>().gridPosition.y = y;                       //�i�q���Wy��ݒ�

                    if (character.characterType == CharacterType.Player)                //�����L�����N�^�[���v���C���[�Ȃ�
                    {
                        star.GetComponent<Star>().owner = Star.StarOwner.Player;        //�X�^�[�̏��L�҂��v���C���[�ɂ���
                    }
                    else if (character.characterType == CharacterType.Player)           //�����L�����N�^�[���G�Ȃ�
                    {
                        star.GetComponent<Star>().owner = Star.StarOwner.Enemy;         //�X�^�[�̏��L�҂�G�ɂ���
                    }

                    starField[x, y] = star.GetComponent<Star>();                        //�i�q���W(x, y)�ɃX�^�[��z�u
                    starCount++;                                                        //�ʂ��ԍ��𑝂₷
                }
                else
                {
                    starField[x, y] = null;
                }
            }
        }

        return starField;
    }



    //�X�^�[�t�B�[���h�̐����i���C���t�B�[���h�j
    void GenerateStarFieldToMain(Star[,] starField)
    {

        float fieldWidth    = mainStarField.rect.width;     //���C���X�^�[�t�B�[���h�̕�
        float fieldHeight   = mainStarField.rect.height;    //���C���X�^�[�t�B�[���h�̍���

        float topMargin     = 40.0f;                        //��̗]��
        float bottomMargin  = 80.0f;                        //���̗]��
        float leftMargin    = 100.0f;                       //���̗]��
        float rightMargin   = 100.0f;                       //�E�̗]��

        float horizontalSpacing   = (fieldWidth - (leftMargin + rightMargin)) / ((starField.GetLength(0) - 1) / 2);  //�������̃X�^�[�̊Ԋu
        float verticalSpacing = (fieldHeight -  (topMargin + bottomMargin)) / ((starField.GetLength(1) - 1) / 2);    //�c�����̃X�^�[�̊Ԋu

        Vector3 starScale = new Vector3(18, 18, 18);        //�X�^�[�̑傫��

        for (int y = 0; y < starField.GetLength(1); y++)
        {
            for (int x = 0; x < starField.GetLength(0); x++)
            {
                Star star = starField[x, y];

                if (star != null)
                {
                    star.transform.SetParent(mainStarField);                //���C���X�^�[�t�B�[���h��e�I�u�W�F�N�g�Ɏw��
                    
                    Vector3 position = new();                               //�X�^�[�̍��W
                    position.x = leftMargin   + horizontalSpacing * x / 2;  //�X�^�[��x���W
                    position.y = bottomMargin + verticalSpacing * y / 2;    //�X�^�[��y���W
                    position.z = -20.0f;                                    //�X�^�[��z���W

                    star.transform.localPosition = position;                //���[�J�����W�ɕϊ�
                    star.transform.localScale = starScale;                  //�X�P�[���̒���
                }
            }
        }

    }


    //�X�^�[�t�B�[���h�̐����i�T�u�t�B�[���h�j
    void GenerateStarFieldToSub(Star[,] starField)
    {

        float fieldWidth = subStarField.rect.width;     //�T�u�X�^�[�t�B�[���h�̕�
        float fieldHeight = subStarField.rect.height;   //�T�u�X�^�[�t�B�[���h�̍���

        float topMargin = 30.0f;                        //��̗]��
        float bottomMargin = 30.0f;                     //���̗]��
        float leftMargin = 50.0f;                       //���̗]��
        float rightMargin = 50.0f;                      //�E�̗]��
        float horizontalSpacing = (fieldWidth - (leftMargin + rightMargin)) / ((starField.GetLength(0) - 1) / 2);  //�������̃X�^�[�̊Ԋu
        float verticalSpacing = (fieldHeight - (topMargin + bottomMargin)) / ((starField.GetLength(1) - 1) / 2);   //�c�����̃X�^�[�̊Ԋu

        Vector3 starScale = new Vector3(10, 10, 10);    //�X�^�[�̑傫��

        for (int y = 0; y < starField.GetLength(1); y++)
        {
            for (int x = 0; x < starField.GetLength(0); x++)
            {
                Star star = starField[x, y];

                if (star != null)
                {
                    star.transform.SetParent(subStarField);                 //�T�u�X�^�[�t�B�[���h��e�I�u�W�F�N�g�Ɏw��

                    Vector3 position = new();                               //�X�^�[�̍��W
                    position.x = leftMargin + horizontalSpacing * x / 2;    //�X�^�[��x���W
                    position.y = bottomMargin + verticalSpacing * y / 2;    //�X�^�[��y���W
                    position.z = -20.0f;                                    //�X�^�[��z���W

                    star.transform.localPosition = position;                //���[�J�����W�ɕϊ�
                    star.transform.localScale = starScale;                  //�X�P�[���̒���
                }
            }
        }

    }


    //���������X�^�[�����݂̃X�^�[�ɐݒ肷��
    void SetCurrentStar(RaycastHit2D hit)
    {
        //���������X�^�[�����݂̃X�^�[�ɂ��āA�L�����A�L���ς݃X�^�[���X�g�ɒǉ�
        CurrentStar = hit.collider.gameObject.GetComponent<Star>();         //���������X�^�[�����݂̃X�^�[�ɐݒ�
        hit.collider.gameObject.GetComponent<Star>().ActivateStar();        //���������X�^�[��L����
        connectedStars.Add(hit.collider.gameObject.GetComponent<Star>());   //���������X�^�[��ڑ��ς݂̃X�^�[���X�g�ɒǉ�

        //Debug.Log("�X�^�[" + connectedStars.Count + ":" + CurrentStar + CurrentStar.isActive);
    }


    //���̐�������
    void LineCreate(int startIndex, Vector3 start, Vector3 end) //(�n�_�̔ԍ�, �n�_�̍��W, �I�_�̍��W)
    {
        if (lineRenderer.positionCount < startIndex + 1)  // ���_��������Ȃ��ꍇ�ɒǉ�(��2�ł���΃J�[�\���ƍ��킹��3�̒��_���K�v)
        {
            lineRenderer.positionCount = startIndex + 1;
        }
        lineRenderer.SetPosition(startIndex - 1, start);    //�n�_�̐ݒ�
        lineRenderer.SetPosition(startIndex, end);      //�I�_�̐ݒ�
    }
    

    //�X�^�[���������Ȃ����A�����������Ȃ������m�F����
    bool CanConnectStar(Star targetStar)
    {

        int currentStarX = CurrentStar.gridPosition.x;  //���݂̃X�^�[�̊i�q���Wx
        int currentStarY = CurrentStar.gridPosition.y;  //���݂̃X�^�[�̊i�q���Wy
        int targetStarX  = targetStar.gridPosition.x;   //�����������Ƃ��Ă���X�^�[�̊i�q���Wx
        int targetStarY  = targetStar.gridPosition.y;   //�����������Ƃ��Ă���X�^�[�̊i�q���Wy
        int distance = (int)Mathf.Pow((currentStarX - targetStarX),2) + (int)Mathf.Pow((currentStarY - targetStarY),2);     //2�X�^�[�Ԃ̋�����2��
        //Debug.Log(distance);

        if (distance == 2 || distance == 4) //������2�悪2��4�̏ꍇ 
        {
            if (ConnectedStars.Count == 1)   //����1�݂̂̏ꍇtrue��Ԃ�
            {
                return true;
            }

            //�����������Ă��Ȃ����m�F
            for (var i = 1; i < ConnectedStars.Count; i++)   //�X�^�[1���X�^�[2�A�X�^�[2���X�^�[3�A...�A�X�^�[n-1���X�^�[n�̐��ɃX�^�[n���X�^�[n+1�̐����������Ȃ������m�F
            {
                Vector3 starA = ConnectedStars[i - 1].GetComponent<Transform>().position;   //���肷����̎n�_�̍��WA
                Vector3 starB = ConnectedStars[i].GetComponent<Transform>().position;       //���肷����̏I�_�̍��WB
                Vector3 starC = CurrentStar.GetComponent<Transform>().position;             //���݂̃X�^�[�̍��WC
                Vector3 starD = targetStar.GetComponent<Transform>().position;              //�����������Ƃ��Ă���X�^�[�̍��WD

                float ta = (starD.x - starC.x) * (starA.y - starC.y) - (starD.y - starC.y) * (starA.x - starC.x); //����CD�̎���starA�̍��W����
                float tb = (starD.x - starC.x) * (starB.y - starC.y) - (starD.y - starC.y) * (starB.x - starC.x); //����CD�̎���starB�̍��W����
                float tc = (starB.x - starA.x) * (starC.y - starA.y) - (starB.y - starA.y) * (starC.x - starA.x); //����AB�̎���starC�̍��W����
                float td = (starB.x - starA.x) * (starD.y - starA.y) - (starB.y - starA.y) * (starD.x - starA.x); //����AB�̎���starD�̍��W����

                //�������������ta * tb < 0 ���@tc * td < 0

                if (!(ta * tb < 0 && tc * td < 0)) //�������Ȃ��ꍇ
                {
                    if (i == ConnectedStars.Count - 1) //���ׂĂ̊����̐��ɂ��Č������Ă��Ȃ��ꍇ
                    {
                        return true; //true��Ԃ�
                    }
                    else //�܂����ׂĂ̊����̐��ɂ��Ċm�F���Ă��Ȃ��ꍇ
                    {
                        continue;   //�J��Ԃ��𑱂���
                    }
                }
                else //��������ꍇ
                {
                    break; //�J��Ԃ����𔲂��o����false��Ԃ�
                }

            }
        }
        else
        {
            Debug.Log("The star is too far.");
            errorText.GetComponent<TextMeshProUGUI>().SetText("�X�^�[���������܂�");    //�G���[���b�Z�[�W
            errorText.SetActive(true);                                                  //�G���[���b�Z�[�W��\��
            return false;
        }

        Debug.Log("Intersected");
        errorText.GetComponent<TextMeshProUGUI>().SetText("���C�����������Ă��܂�");    //�G���[���b�Z�[�W
        errorText.SetActive(true);                                                      //�G���[���b�Z�[�W��\��
        return false;
    }


    //����{�^�����������Ƃ�
    public void PushAcceptButton()
    {
        isAcceptButtonPushed = true;
    }

    //���Z�b�g�{�^�����������Ƃ�
    public void PuchResetButton()
    {
        foreach(var star in connectedStars) //�ڑ�����Ă����X�^�[�𖳌���
        {
            star.DeactivateStar();
        }

        ConnectedStars.Clear();                 //�ڑ������X�^�[���N���A
        CurrentStar = null;                     //���݂̃X�^�[���N���A
        lineRenderer.positionCount = 0;         //�����N���A

        acceptButton.SetActive(false);          //����{�^��       :��\��
        retryButton.SetActive(false);           //���Z�b�g�{�^��   :��\��
        spellDescriptionArea.SetActive(true);   //�X�y������UI     :�\��
        activeSpellArea.SetActive(false);       //�L���X�y��UI     :��\��

        Debug.Log("Reset");
    }





}
