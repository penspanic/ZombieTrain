using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AppSound : MonoBehaviour {

	// === 외부 파라미터 ======================================
	public static AppSound instance = null;

	// 배경음
	[System.NonSerialized] public audiomanager fm;
	
	//Dictionary
	private Dictionary<string,AudioSource> audiosources = new Dictionary<string,AudioSource>();

	[System.NonSerialized] public AudioSource BGM_TITLE;
	[System.NonSerialized] public AudioSource BGM_STAGE;


	// 효과음
	[System.NonSerialized] public AudioSource SE_bathammer;
	[System.NonSerialized] public AudioSource SE_button;
	[System.NonSerialized] public AudioSource SE_dooropen;
	[System.NonSerialized] public AudioSource SE_Explosion01;
	[System.NonSerialized] public AudioSource SE_fireextinguisher;
	[System.NonSerialized] public AudioSource SE_fish;
	[System.NonSerialized] public AudioSource SE_fryingpan;
	[System.NonSerialized] public AudioSource SE_gameover;
	[System.NonSerialized] public AudioSource SE_get_item;
	[System.NonSerialized] public AudioSource SE_gun;
	[System.NonSerialized] public AudioSource SE_itembox_open;
	[System.NonSerialized] public AudioSource SE_jump;
	[System.NonSerialized] public AudioSource SE_katana;
	[System.NonSerialized] public AudioSource SE_knife;
	[System.NonSerialized] public AudioSource SE_startlever;
	[System.NonSerialized] public AudioSource SE_zombie_die;
	[System.NonSerialized] public AudioSource SE_zombie_hit;
	[System.NonSerialized] public AudioSource SE_zombie_idle;
	

	// === 내부 파라미터 ======================================
	string sceneName = "non";
    public float SoundBGMVolume = 0.7f;
    public float SoundSEVolume = 1.0f;
    float PreSoundBGMVolume = 0.0f;
    float PreSoundSEVolume = 0.0f;

	// === 코드 =============================================
	void Start () {
		// 사운드
		fm = GameObject.Find("audioManager").GetComponent<audiomanager>();

		// 배경음
		fm.CreateGroup("BGM");
		fm.SoundFolder = "Sounds/";
		BGM_TITLE 				= fm.LoadResourcesSound("BGM","ZT_BGM_Menu");
		BGM_STAGE 				= fm.LoadResourcesSound("BGM","ZT_BGM_ingame");
	/*	BGM_STAGE	 	= fm.LoadResourcesSound("BGM","battle_1");
		BGM_GAMEOVER 				= fm.LoadResourcesSound("BGM","gameover_1");
	*/	

		// 효과음
		fm.CreateGroup("SE");
		fm.SoundFolder = "Sounds/";

		SE_bathammer = fm.LoadResourcesSound("SE", "SE_bat,hammer");
        SE_button = fm.LoadResourcesSound("SE", "SE_button");
        SE_dooropen = fm.LoadResourcesSound("SE", "SE_dooropen");
        SE_Explosion01 = fm.LoadResourcesSound("SE", "SE_Explosion01");
        SE_fireextinguisher = fm.LoadResourcesSound("SE", "SE_fireextinguisher");
        SE_fish = fm.LoadResourcesSound("SE", "SE_fish");
        SE_fryingpan = fm.LoadResourcesSound("SE", "SE_fryingpan");
        SE_gameover = fm.LoadResourcesSound("SE", "SE_gameover");
        SE_get_item = fm.LoadResourcesSound("SE", "SE_get_item");
        SE_gun = fm.LoadResourcesSound("SE", "SE_gun");
        SE_itembox_open = fm.LoadResourcesSound("SE", "SE_itembox_open");
        SE_jump = fm.LoadResourcesSound("SE", "SE_jump");
        SE_katana = fm.LoadResourcesSound("SE", "SE_katana");
        SE_knife = fm.LoadResourcesSound("SE", "SE_knife");
        SE_startlever = fm.LoadResourcesSound("SE", "SE_startlever");
        SE_zombie_die = fm.LoadResourcesSound("SE", "SE_zombie_die");
        SE_zombie_hit = fm.LoadResourcesSound("SE", "SE_zombie_hit");
        SE_zombie_idle = fm.LoadResourcesSound("SE", "SE_zombie_idle");

		audiosources.Add("SE_bathammer",SE_bathammer);
		audiosources.Add("SE_button",SE_button);
		audiosources.Add("SE_dooropen",SE_dooropen);
		audiosources.Add("SE_Explosion01",SE_Explosion01);
		audiosources.Add("SE_fireextinguisher",SE_fireextinguisher);
		audiosources.Add("SE_fish",SE_fish);
		audiosources.Add("SE_fryingpan",SE_fryingpan);
		audiosources.Add("SE_gameover",SE_gameover);
		audiosources.Add("SE_get_item",SE_get_item);
		audiosources.Add("SE_gun",SE_gun);
		audiosources.Add("SE_itembox_open",SE_itembox_open);
		audiosources.Add("SE_jump",SE_jump);
		audiosources.Add("SE_katana",SE_katana);
		audiosources.Add("SE_knife",SE_knife);
		audiosources.Add("SE_startlever",SE_startlever);
		audiosources.Add("SE_zombie_die",SE_zombie_die);
		audiosources.Add("SE_zombie_hit",SE_zombie_hit);
		audiosources.Add("SE_zombie_idle",SE_zombie_idle);

		

		instance = this;
	}
	public void SE_PLAY(string SEname)
	{
        if(audiosources.ContainsKey(SEname) == false)
        {
            Debug.LogError("AppSound " + SEname + " not loaded!");
            return;
        }

		AudioSource playse = audiosources[SEname];
		playse.Play();
	}
	public void Mute()
	{
		PreSoundBGMVolume=SoundBGMVolume;
		PreSoundSEVolume=SoundSEVolume;
	}
	public void Unmute()
	{
		SoundBGMVolume = PreSoundBGMVolume;
		SoundSEVolume = PreSoundSEVolume;
	}
	void Update() {
	
        // 볼륨 설정
        fm.SetVolume("BGM", SoundBGMVolume);
        fm.SetVolume("SE", SoundSEVolume);
		// 씬이 바뀌었는지 검사
		if (sceneName != SceneManager.GetActiveScene().name) {
			sceneName = SceneManager.GetActiveScene().name;

			

			// 배경음 재생
			
			if (sceneName == "MainScene") {
				fm.Stop ("BGM");
				fm.FadeOutVolumeGroup("BGM",BGM_TITLE,0.0f,1.0f,false);
				fm.FadeInVolume(BGM_TITLE,SoundBGMVolume,1.0f,true);
				BGM_TITLE.loop = true;
				BGM_TITLE.Play();
			}else
			if (sceneName == "InGame") {
				fm.Stop ("BGM");
				fm.FadeOutVolumeGroup("BGM",BGM_STAGE,0.0f,1.0f,false);
				fm.FadeInVolume(BGM_STAGE,SoundBGMVolume,1.0f,true);
				BGM_STAGE.loop = true;
				BGM_STAGE.Play();
			}
		}
	}
}
