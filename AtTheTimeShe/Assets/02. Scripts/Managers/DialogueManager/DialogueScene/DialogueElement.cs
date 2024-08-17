using System;

[Serializable]
public class DialogueElement
{
    public string dialogueText;         // ���
    public int choiceID;                // ������ ID
    public int linkedDialogueID;        // ����� ��� ID
    public string sfxName;              // ȿ���� �̸�
    public string bgmName;              // ������� �̸�
    public string displayItemName;      // ǥ�� ������ �̸�
    public string illustrationName;     // ��ȭ �̸�
}
