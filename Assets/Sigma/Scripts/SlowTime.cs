using UnityEngine;
using System.Collections;

public class SlowTime : MonoBehaviour
{
    public KeyCode Key = KeyCode.LeftShift;
    private float field_timeScaleNormalValue;
    private float field_fixedDeltaTimeNormalValue;
    public float TimeScaleMinValue = 0.1f;
    public float TimeScaleSlowDownStep = 0.05f;
    public float TimeScaleSpeedUpStep = 0.01f;
    private float field_soundPitchNormalValue;
    public float SoundPitchMinValue = 0.5f;
    public float SoundPitchSlowDownStep = 0.05f;
    public float SoundPitchSpeedUpStep = 0.05f;

    public AudioClip SoundOfTimeSlowingDown;
    public AudioClip SoundOfTimeSpeedingUp;
    [Range(0, 1)]
    public float SoundOfTimeVolume = 1f;

    private AudioSource field_audioSourceWithStaticPitch;
    private AudioSource field_audioSourceWithDynamicPitch;

    // Use this for initialization
    void Start()
    {
        field_timeScaleNormalValue = 1f;
        field_fixedDeltaTimeNormalValue = Time.fixedDeltaTime;
        field_soundPitchNormalValue = 1f;

        //field_gameObjectForAudioWithStaticPitch = new GameObject();
        field_audioSourceWithStaticPitch = gameObject.AddComponent<AudioSource>();
        field_audioSourceWithStaticPitch.volume = SoundOfTimeVolume;
        //field_gameObjectForAudioWithDynamicPitch = new GameObject();
        field_audioSourceWithDynamicPitch = gameObject.AddComponent<AudioSource>();
        field_audioSourceWithDynamicPitch.volume = SoundOfTimeVolume;
    }

    // Update is called once per frame
    void Update()
	{
		if (Time.timeScale < 0.0001f)
			return;

        // Play sounds of time slowing/fastening
        if (Input.GetKeyDown(Key))
        {
            field_audioSourceWithStaticPitch.PlayOneShot(SoundOfTimeSlowingDown);
        }
        if (Input.GetKeyUp(Key))
        {
            field_audioSourceWithStaticPitch.PlayOneShot(SoundOfTimeSpeedingUp);
        }

		float scale = Time.timeScale;
		float pitch = Time.timeScale;

        // Slow time if key is pressed
        if (Input.GetKey(Key) && (Time.timeScale > TimeScaleMinValue || pitch > SoundPitchMinValue))
        {

            scale = Time.timeScale -= TimeScaleSlowDownStep;
            if (scale < TimeScaleMinValue)
            {
                scale = TimeScaleMinValue;
            }
            pitch = field_audioSourceWithDynamicPitch.pitch -= SoundPitchSlowDownStep;
            if (pitch < SoundPitchMinValue)
            {
                pitch = SoundPitchMinValue;
            }

        }
        // Fasten time if key is released
        if (!Input.GetKey(Key) && (Time.timeScale < field_timeScaleNormalValue || pitch < field_soundPitchNormalValue))
        {
            scale = Time.timeScale + TimeScaleSpeedUpStep;
            if (scale > field_timeScaleNormalValue)
            {
                scale = field_timeScaleNormalValue;
            }
            pitch = field_audioSourceWithDynamicPitch.pitch += SoundPitchSpeedUpStep;
            if (pitch > field_soundPitchNormalValue)
            {
                pitch = field_soundPitchNormalValue;
            }
        }

        // Apply the changes to time itself
        Time.timeScale = scale;
        Time.fixedDeltaTime = field_fixedDeltaTimeNormalValue / field_timeScaleNormalValue * Time.timeScale;

        // Apply changes to all sounds
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.pitch = pitch;
        }
        // Revert changes of special sounds of time slowing/fastening
        field_audioSourceWithStaticPitch.pitch = 1;
    }
}
