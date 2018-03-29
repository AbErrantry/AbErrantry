using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TravelMenu : MonoBehaviour
{
	public Animator anim;
	public GameObject container;
	public TMP_Text text;
	public CanvasGroup group;

	public Button respawnButton;

	private Character2D.Player player;

	public bool isTravelling;

	private void Start()
	{
		player = GetComponent<Character2D.Player>();
		isTravelling = false;
		group.interactable = false;
		container.SetActive(false);
	}

	private IEnumerator OpenDelay()
	{
		yield return new WaitForSeconds(1.0f); //wait for travel/death screen to fade in completely
		player.Respawn();
	}

	private IEnumerator CloseDelay()
	{
		yield return new WaitForSeconds(1.0f); //wait for travel/death screen to fade out completely
		container.SetActive(false);
		isTravelling = false;
		ElementFocus.focus.RemoveFocus();
	}

	public void Open(string text, bool dying)
	{
		this.text.text = text;
		isTravelling = true;
		container.SetActive(true);
		group.interactable = false;
		anim.SetBool("isOpen", true);
		if (dying)
		{
			StartCoroutine(OpenDelay());
		}
		else
		{
			anim.Play("Open");
		}
	}

	public void EnableButtons()
	{
		group.interactable = true;
		ElementFocus.focus.SetItemFocus(respawnButton.gameObject);
	}

	public void CloseTravelScreen()
	{
		anim.SetBool("isOpen", false);
		StartCoroutine(CloseDelay());
	}

	public void Respawn()
	{
		group.interactable = false;
		CloseTravelScreen();
	}

	public void MainMenu()
	{
		group.interactable = false;
		SceneManager.LoadScene("MainMenu");
	}
}
