using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class UXManager : MonoBehaviour
{
	private enum State {Idle, Opening, Menu, VideoPlayer, AR};
	private enum FadeType {Opacity, Color};

	[SerializeField]
	private State state = State.Opening;
	[SerializeField]
	private State previousState = State.Opening;

	private AVManager audioVisualManager;

	public Image openingTitle;
	public Image background;
	public RawImage videoCanvas;
	public Image[] buttons;

	public VideoPlayer videoPlayer;
	public string[] clips;

	public float titleCardDuration = 1f;
	public float fadeSpeed = 1f;
	public float currentFade = 0f;
	public bool transitionInProgress = false;
	public bool fadingIn = false;

	public float waitTimeForPulseTitle = 2f;
	public float waitTimeForButtons = 2f;

	public float lightnessToReach = 1f;
	private bool colorChangeInProgress = false;
	//private FadeType currentFadeType = FadeType.Opacity;

    private void Start() {
		audioVisualManager = GameObject.FindObjectOfType<AVManager>();
		videoCanvas.gameObject.SetActive(false);

		HideAllElements();
    }

	private void HideAllElements() {
		Color transparent = new Color(0f, 0f, 0f, 0f);

		openingTitle.color = transparent;

		for (int i = 0; i < buttons.Length; i++){
			buttons[i].color =  transparent;
			buttons[i].GetComponentInChildren<TextMeshProUGUI>().color =  transparent;
		}

		StartCoroutine(Opening(openingTitle, 2f)); //Delay for Pulse title.
	}

    private void Update() {
			switch (previousState){
				case State.Opening: // previousState is Opening at the moment but only gets in the if once the actual state as moved from the opening to the idle state.
					if (state == State.Idle){
						previousState = state;
						state = State.Menu;
						StartCoroutine(ShowOptions(buttons, 2f));
					}
					break;
				case State.Menu:
					lightnessToReach = 0f;
					StartCoroutine(HideOptions(buttons));
					StartCoroutine(ChangeImageColor(background));
					StartCoroutine(PlayVideo(4f));

					previousState = State.Idle;
					break;
			}
    }

	private void FadeInImage(Image current) {
		if (currentFade <= 1f) {
			currentFade += Time.deltaTime * fadeSpeed;
			current.color = new Color(1f, 1f, 1f, currentFade);

			if (current.GetComponentInChildren<TextMeshProUGUI>() != null){
				current.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, currentFade);
			}
		} else {
			transitionInProgress = true;
			fadingIn = false;
			currentFade = 1f;
		}
	}

	private void FadeOutImage(Image current) {
		if(currentFade >= 0f) {
			currentFade -= Time.deltaTime * fadeSpeed;
			current.color = new Color(1f, 1f, 1f, currentFade);

			if (current.GetComponentInChildren<TextMeshProUGUI>() != null){
				current.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0f, 0f, 0f, currentFade);
			}
		} else {
			transitionInProgress = false;
			currentFade = 0f;
		}
	}

	public void Selection(int selection){
		previousState = state;
		state = State.VideoPlayer;

		switch (selection){
			case 0:
				videoPlayer.url = clips[0];
				break;
			case 1:
				videoPlayer.url = clips[1];
				break;
			case 2:
				break;
		}

	}

	// After hiding all the
	private IEnumerator Opening(Image current, float delay){
		fadingIn = true;

		yield return new WaitForSeconds(delay);

		StartCoroutine(ChangeImageColor(background));

		// Line 103 to 116 would need to be repeated for every opening title card.
		// If there are enough this could be made automatically with a list of those titles rather than a single image.

		while (!transitionInProgress){
			FadeInImage(current);

			yield return null;

		}

		yield return new WaitForSeconds(titleCardDuration);

		while (transitionInProgress){
			FadeOutImage(current);

			yield return null;
		}

		state = State.Idle;
	}

	// private void TransitionImage() {
	//
	// }


	private IEnumerator ShowOptions(Image[] currents, float delay){
		yield return new WaitForSeconds(delay);

		while (!transitionInProgress){
			foreach (Image current in currents){
				FadeInImage(current);
			}

			yield return null;

		}

		transitionInProgress = false;
	}

	private IEnumerator HideOptions(Image[] currents) {
		transitionInProgress = true;

		while (transitionInProgress){
			foreach (Image current in currents){
				FadeOutImage(current);
			}

			yield return null;

		}
	}

	private IEnumerator ChangeImageColor(Image current){
		colorChangeInProgress = true;

		while (colorChangeInProgress){

			if (lightnessToReach == 0f) {
				if (currentFade >= lightnessToReach) {
					currentFade -= Time.deltaTime * fadeSpeed;
					current.color = new Color(currentFade, currentFade, currentFade, 1f);
				} else {
					colorChangeInProgress = false;
				}
			} else if (lightnessToReach == 1f) {
				if (currentFade <= lightnessToReach) {
					current.color = new Color(currentFade, currentFade, currentFade, 1f);
				} else {
					colorChangeInProgress = false;
				}
			}

			yield return null;
		}
	}

	private IEnumerator PlayVideo(float delay){
		yield return new WaitForSeconds(delay);

		videoCanvas.gameObject.SetActive(true);

		audioVisualManager.SetAudioSource();

		videoPlayer.Play();
		while (videoPlayer.frameCount == 0){
			yield return null;
		}


	}

}
