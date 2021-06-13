using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class AVManager : MonoBehaviour
{
	private VideoPlayer player;
	private AudioSource audio;
	public Image controlBar;
	public Image soundButton;

	public Sprite pauseImage;
	public Sprite[] soundImages;
	private Sprite bufferImage;


	public bool isPlaying = true;


    private void Start() {
		player = GetComponent<VideoPlayer>();
		audio = GetComponent<AudioSource>();

		soundButton = GameObject.Find("SoundButton").GetComponent<Image>();
    }

    private void Update() {
		if (audio.volume > 0.5){
			if (soundButton.sprite != soundImages[0])
				soundButton.sprite = soundImages[0];
		} else if (audio.volume != 0){
			if (soundButton.sprite != soundImages[1])
				soundButton.sprite = soundImages[1];
		} else {
			if (soundButton.sprite != soundImages[2])
				soundButton.sprite = soundImages[2];
		}
    }

	public void TogglePlayState(){
		isPlaying = !isPlaying;

		if (isPlaying) {
			player.Play();

			foreach (Image child in controlBar.GetComponentsInChildren<Image>()){
				if (child.transform.name == "PlayPauseButton"){
						bufferImage = child.sprite;
						child.sprite = pauseImage;
						pauseImage = bufferImage;
						bufferImage = null;
				}
			}
		} else {
			player.Pause();

			foreach (Image child in controlBar.GetComponentsInChildren<Image>()){
				if (child.transform.name == "PlayPauseButton"){
						bufferImage = child.sprite;
						child.sprite = pauseImage;
						pauseImage = bufferImage;
						bufferImage = null;
				}
			}
		}
	}

	public void SetAudioSource(){
		player.SetTargetAudioSource(0, audio);
	}

}
