using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject MainDialogue;
	public Text nameText;
	public Text dialogueText;

	private Queue<string> sentences;

	void Awake()
	{
		sentences = new Queue<string>();
	}

    void Start()
    {
        
    }

    public void StartDialogue(Dialogue dialogue)
	{
		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}
        DialogueNameMgr.NameCount += 1;
		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
        PuzzlePlayerCtrl.ReadDialogue = false;
        PuzzlePlayerCtrl.ResetMouse = true;
        DialogueNameMgr.NameCount = -1;
        MainDialogue.SetActive(false);
    }
}
