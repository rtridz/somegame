using System;
using System.Configuration;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections;

public class GameMechanics : MonoBehaviour {

	public GameObject MainMenu;
	public GameObject SettingsMenu;
	public GameObject SettingsMenuSliderMusic;
	public GameObject SettingsMenuSliderSound;
	public GameObject MainMenuButtonPlay;
	public GameObject MainMenuButtonResume;

    public NixieTubePack TimeNixieTubePack;
    public NixieTubePack ScoreNixieTubePack;

    public Bumper Bumper1;
    public Bumper Bumper2;
    public Bumper Bumper3;
    public Bumper Bumper4;
    public Bumper Bumper5;
    public Bumper Bumper6;

    public Flag Flag1;
    public Flag Flag2;
    public Flag Flag3;

    private int field_ballCount = 0;

    private bool field_bumper1IsActive = false;
    private bool field_bumper2IsActive = false;
    private bool field_bumper3IsActive = false;
    private bool field_bumper4IsActive = false;
    private bool field_bumper5IsActive = false;
    private bool field_bumper6IsActive = false;

    private bool field_flag1IsActive = false;
    private bool field_flag2IsActive = false;
    private bool field_flag3IsActive = false;

    private int field_score;
	private float field_timeScaleBeforePause;
	private bool field_gameStarted;
	private bool field_gamePaused;

	private string field_configPath;
	private float field_soundVolume;
	private float field_musicVolume;

	// Use this for initialization
	void Start ()
	{
		field_configPath = Path.Combine(Application.persistentDataPath, "config.txt");

        field_score = 0;

        StartCoroutine(ScoreDueTime());

		GameStop();
		MenuMain();

		StartCoroutine(Init());
	}
	void OnDestroy()
	{
		ConfigSave();
	}

	IEnumerator Init()
	{
		yield return new WaitForEndOfFrame();

		ConfigLoad();

		GetComponent<AudioSource>().Play();
	}

	public void ConfigLoad()
	{
		try
		{
			string config = File.ReadAllText(field_configPath);
			config = config.Replace("\r\n", "\n");
			string[] lines = config.Split('\n');
			string line;
			for (int i = 0; i < lines.Length; i++)
			{
				lines[i] = lines[i].Trim();
				if (lines[i].Length == 0)
					continue;
				if (lines[i][0] == '#')
					continue;

				string[] parts = lines[i].Split(new char[]{'='}, 2);
				if (parts.Length != 2)
					continue;
				parts[0] = parts[0].Trim();
				parts[1] = parts[1].Trim();

				if ("SoundVolume".CompareTo(parts[0]) == 0)
				{
					int volume;
					SetSoundVolume(int.TryParse(parts[1], out volume) ? volume / 100f : 1f);
				}
				else if ("MusicVolume".CompareTo(parts[0]) == 0)
				{
					int volume;
					SetMusicVolume(int.TryParse(parts[1], out volume) ? volume / 100f : 1f);
				}
			}

			SettingsMenuSliderMusic.GetComponent<UISlider>().value = GetComponent<AudioSource>().volume;
			SettingsMenuSliderSound.GetComponent<UISlider>().value = AudioListener.volume;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}
	public void ConfigSave()
	{
		try
		{
			string config = "# Audio configuration";
			config += "\r\nSoundVolume=" + Convert.ToInt32(field_soundVolume * 100);
			config += "\r\nMusicVolume=" + Convert.ToInt32(field_musicVolume * 100);
			File.WriteAllText(field_configPath, config);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	public void SetMusicVolume(float param_volume)
	{
		field_musicVolume = param_volume;
		GetComponent<AudioSource>().volume = param_volume;
	}
	public void SetSoundVolume(float param_volume)
	{
		field_soundVolume = param_volume;
		AudioListener.volume = param_volume;
	}

	public void MenuMain()
	{
		MainMenu.SetActive(true);
		SettingsMenu.SetActive(false);
	}

	public void MenuSettings()
	{
		MainMenu.SetActive(false);
		SettingsMenu.SetActive(true);
	}

	public void GameExit()
	{
		Application.Quit();
	}

	public void GamePause()
	{
		object[] objects = FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in objects)
		{
			go.SendMessage("OnPause", SendMessageOptions.DontRequireReceiver);
		}

		field_timeScaleBeforePause = Time.timeScale;
		Time.timeScale = 0;
		MainMenu.SetActive(true);
		MainMenuButtonPlay.SetActive(false);
		MainMenuButtonResume.SetActive(true);

		field_gamePaused = true;
	}
	public void GameStop()
	{
		object[] objects = FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in objects)
		{
			go.SendMessage("OnPause", SendMessageOptions.DontRequireReceiver);
		}

		field_timeScaleBeforePause = 1;
		Time.timeScale = 0;
		MainMenu.SetActive(true);
		MainMenuButtonPlay.SetActive(true);
		MainMenuButtonResume.SetActive(false);

		field_gameStarted = false;
		field_gamePaused = false;
	}
	public void GameStart()
	{
		GameResume();

		field_gameStarted = true;
	}
	public void GameResume()
	{
		object[] objects = FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in objects)
		{
			go.SendMessage("OnResume", SendMessageOptions.DontRequireReceiver);
		}

		Time.timeScale = field_timeScaleBeforePause;
		MainMenu.SetActive(false);

		field_gamePaused = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (TimeNixieTubePack != null)
            TimeNixieTubePack.SendMessage("SetValue", Time.time);
        if (ScoreNixieTubePack != null)
            ScoreNixieTubePack.SendMessage("SetValue", field_score);

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (field_gamePaused)
			{
				GameResume();
			}
			else if (field_gameStarted)
			{
				GamePause();
			}
		}
	}

    void FixedUpdate()
    {
        int incrementScoreByBumpersTimes = 0;
        if (Bumper1.IsActive() != field_bumper1IsActive)
        {
            field_bumper1IsActive = !field_bumper1IsActive;
            if (field_bumper1IsActive)
            {
                incrementScoreByBumpersTimes++;
            }
        }
        if (Bumper2.IsActive() != field_bumper2IsActive)
        {
            field_bumper2IsActive = !field_bumper2IsActive;
            if (field_bumper2IsActive)
            {
                incrementScoreByBumpersTimes++;
            }
        }
        if (Bumper3.IsActive() != field_bumper3IsActive)
        {
            field_bumper3IsActive = !field_bumper3IsActive;
            if (field_bumper3IsActive)
            {
                incrementScoreByBumpersTimes++;
            }
        }
        if (Bumper4.IsActive() != field_bumper4IsActive)
        {
            field_bumper4IsActive = !field_bumper4IsActive;
            if (field_bumper4IsActive)
            {
                incrementScoreByBumpersTimes++;
            }
        }
        if (Bumper5.IsActive() != field_bumper5IsActive)
        {
            field_bumper5IsActive = !field_bumper5IsActive;
            if (field_bumper5IsActive)
            {
                incrementScoreByBumpersTimes++;
            }
        }
        if (Bumper6.IsActive() != field_bumper6IsActive)
        {
            field_bumper6IsActive = !field_bumper6IsActive;
            if (field_bumper6IsActive)
            {
                incrementScoreByBumpersTimes++;
            }
        }
        IncrementScoreByBumpers(incrementScoreByBumpersTimes);

        bool flagJustGotActivated = false;
        if (Flag1.IsActive() != field_flag1IsActive)
        {
            field_flag1IsActive = !field_flag1IsActive;
            if (field_flag1IsActive)
            {
                flagJustGotActivated = true;
            }
        }
        if (Flag2.IsActive() != field_flag2IsActive)
        {
            field_flag2IsActive = !field_flag2IsActive;
            if (field_flag2IsActive)
            {
                flagJustGotActivated = true;
            }
        }
        if (Flag3.IsActive() != field_flag3IsActive)
        {
            field_flag3IsActive = !field_flag3IsActive;
            if (field_flag3IsActive)
            {
                flagJustGotActivated = true;
            }
        }
        if (flagJustGotActivated)
        {
            IncrementScoreByFlags((field_flag1IsActive ? 1 : 0) + (field_flag2IsActive ? 1 : 0) + (field_flag3IsActive ? 1 : 0));
        }
        if (field_flag1IsActive && field_flag2IsActive && field_flag3IsActive)
        {
            Flag1.Deactivate();
            Flag2.Deactivate();
            Flag3.Deactivate();

            field_flag1IsActive = false;
            field_flag2IsActive = false;
            field_flag3IsActive = false;
        }
    }

    void IncrementScoreByBumpers(int param_times)
    {
        int multiplier = 0;
        multiplier += field_bumper1IsActive ? 1 : 0;
        multiplier += field_bumper2IsActive ? 1 : 0;
        multiplier += field_bumper3IsActive ? 1 : 0;
        multiplier += field_bumper4IsActive ? 1 : 0;
        multiplier += field_bumper5IsActive ? 1 : 0;
        multiplier += field_bumper6IsActive ? 1 : 0;
        multiplier *= param_times;
        field_score += 100 * multiplier * field_ballCount;
    }
    void IncrementScoreByFlags(int param_times)
    {
        field_score += (int)Math.Pow(10, param_times + 2) * field_ballCount;
    }

    public void IncrementBallsCount()
    {
        field_ballCount++;
    }
    public void DecrementBallsCount()
    {
        field_ballCount--;
    }

    IEnumerator ScoreDueTime()
    {
        YieldInstruction yieldInstruction = new WaitForSeconds(0.1f);
        while (true)
        {
            field_score += 1 * field_ballCount;
            yield return yieldInstruction;
        }
    }
}
