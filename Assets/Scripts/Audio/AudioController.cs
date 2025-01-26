using System.Collections.Generic;
using UnityEngine;
using NEO.Audio;
using NEO.Utils;
using UnityEngine.Pool;
using System.Collections;
using DG.Tweening;
using System;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [SerializeField] private SO_AudioConfig _audioConfig;
    private readonly Dictionary<string, ObjectPool<AudioSource>> _audioPools = new();
    private Dictionary<string, HashSet<AudioSource>> _activeAudioSources = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        InitializePools();
    }

    private void Start()
    {
        PlayAudio("Music", "MusicaTema", fadeDuration: 10);
    }

    private void OnEnable()
    {
        DamageController.OnTakeDamage += BubbleExplode;
    }

    private void OnDisable()
    {
        DamageController.OnTakeDamage -= BubbleExplode;
    }

    public void BubbleExplode()
    {
        // Verifica se o grupo "Bubble" existe no dicionário
        if (!_audioConfig.Audios.TryGetValue("Bubble", out var audioGroup) || audioGroup.Sounds.Count == 0)
        {
            Debug.LogWarning("No sounds available in the 'Bubble' audio group.");
            return;
        }

        // Seleciona uma chave aleatória do dicionário de sons
        List<string> keys = new List<string>(audioGroup.Sounds.Keys);
        string soundKey = keys[UnityEngine.Random.Range(0, keys.Count)];

        // pegar uma key aleatoria do dicionario
        PlayAudio("Bubble", soundKey, fadeDuration: 0, true);
    }

    public void OnButtonPointerEnter()
    {
        PlayAudio("SFX", "Button",fadeDuration:0, true);
    }
    /// <summary>
    /// Plays an audio from the specified group.
    /// </summary>
    /// <param name="groupName">The name of the audio group.</param>
    /// <param name="soundKey">The key of the sound in the group.</param>
    /// <param name="fadeDuration">The duration of the fade-in effect in seconds. Default is 0 (no fade).</param>
    /// <returns>The configured AudioSource playing the sound, or null if an error occurs.</returns>
    public AudioSource PlayAudio(string groupName, string soundKey, int fadeDuration = 0, bool isRandomPitch = false)
    {
        if (!_audioConfig.Audios.TryGetValue(groupName, out var audioGroup))
        {
            Debug.LogWarning($"Audio group '{groupName}' not found.");
            return null;
        }

        if (!audioGroup.Sounds.TryGetValue(soundKey, out var sound))
        {
            Debug.LogWarning($"Sound '{soundKey}' not found in group '{groupName}'.");
            return null;
        }

        if (!_audioPools.TryGetValue(groupName, out var pool))
        {
            Debug.LogWarning($"No audio pool found for group '{groupName}'.");
            return null;
        }

        AudioSource audioSource;

        if (pool.CountActive >= audioGroup.PoolLenght)
        {
            AudioSource lowestPrioritySource = FindLowestPrioritySource(groupName, sound.Priority);
            if (lowestPrioritySource == null)
            {
                Debug.LogWarning($"No available AudioSource with lower priority than '{soundKey}' to replace in group '{groupName}'.");
                return null;
            }

            ConfigureAudioSource(lowestPrioritySource, sound);
            lowestPrioritySource.transform.DOKill();

            return lowestPrioritySource;
        }
 
        audioSource = pool.Get();
        ConfigureAudioSource(audioSource, sound);

        if (isRandomPitch)
            audioSource.NEOPlayWithRandomizePitch(fadeDuration,audioSource.pitch - 0.2f,audioSource.pitch + 0.2f);
        else
            audioSource.NEOPlay(fadeDuration);
        
        if (!audioSource.loop)
            StartCoroutine(ReleaseAudioSourceWhenDone(audioSource, pool));

        return audioSource;
    }

    /// <summary>
    /// Stops a specific sound.
    /// </summary>
    /// <param name="groupName">The name of the audio group.</param>
    /// <param name="soundKey">The key of the sound to stop.</param>
    /// <param name="fadeDuration">The duration of the fade-out effect in seconds. Default is 0 (no fade).</param>
    public void StopAudio(string groupName, string soundKey, int fadeDuration = 0)
    {
        if (!_audioConfig.Audios.TryGetValue(groupName, out var audioGroup))
        {
            Debug.LogWarning($"Audio group '{groupName}' not found.");
            return;
        }

        if (!audioGroup.Sounds.TryGetValue(soundKey, out var sound))
        {
            Debug.LogWarning($"Sound '{soundKey}' not found in group '{groupName}'.");
            return;
        }

        if (!_activeAudioSources.TryGetValue(groupName, out var activeSources))
        {
            Debug.LogWarning($"No active sounds found in group '{groupName}'.");
            return;
        }

        // Find the active AudioSource playing the specified sound
        AudioSource audioSourceToStop = null;
        foreach (var audioSource in activeSources)
        {
            if (audioSource.clip == sound.Clip && audioSource.isPlaying)
            {
                audioSourceToStop = audioSource;
                break;
            }
        }

        if (audioSourceToStop == null)
        {
            Debug.LogWarning($"Sound '{soundKey}' is not currently playing in group '{groupName}'.");
            return;
        }

        // Stop the audio with optional fade-out
        audioSourceToStop.NEOStop(fadeDuration);

        // Release the AudioSource back to the pool after fade-out
        StartCoroutine(ReleaseAudioSourceWhenDone(audioSourceToStop, _audioPools[groupName]));
    }

    /// <summary>
    /// Mutes all AudioSources in a specified group.
    /// </summary>
    /// <param name="groupName">The name of the audio group to mute.</param>
    public void MuteGroup(string groupName)
    {
        if (!_audioConfig.Audios.TryGetValue(groupName, out var audioGroup))
        {
            Debug.LogWarning($"Audio group '{groupName}' not found.");
            return;
        }

        audioGroup.AudioMixerGroup.audioMixer.NEOSetVolume(groupName+"Volume", volume: 0);
    }

    /// <summary>
    /// un mute all AudioSources in a specified group.
    /// </summary>
    /// <param name="groupName">The name of the audio group to un mute.</param>
    public void UnMuteGroup(string groupName)
    {
        if (!_audioConfig.Audios.TryGetValue(groupName, out var audioGroup))
        {
            Debug.LogWarning($"Audio group '{groupName}' not found.");
            return;
        }

        audioGroup.AudioMixerGroup.audioMixer.ClearFloat(groupName + "Volume");
    }

    /// <summary>
    /// Sets the volume of a specific AudioMixer group.
    /// </summary>
    /// <param name="groupName">The name of the audio group to mute.</param>
    /// <param name="volume">The target volume as a linear value (0 to 1).</param>
    public void SetAudioGroupVolume(string groupName, float volume)
    {
        if (!_audioConfig.Audios.TryGetValue(groupName, out var audioGroup))
        {
            Debug.LogWarning($"Audio group '{groupName}' not found.");
            return;
        }

        audioGroup.AudioMixerGroup.audioMixer.NEOSetVolume(groupName + "Volume", volume);
    }

    /// <summary>
    /// Plays a 3D audio from the specified group at the given transform's position.
    /// </summary>
    /// <param name="groupName">The name of the audio group.</param>
    /// <param name="soundKey">The key of the sound in the group.</param>
    /// <param name="targetTransform">The transform where the audio will be played.</param>
    /// <param name="isMoving">Whether the audio source should follow the transform while playing.</param>
    /// <param name="fadeDuration">The duration of the fade-in effect in seconds. Default is 0 (no fade).</param>
    public void PlayAudio3D(string groupName, string soundKey, Transform targetTransform,bool isMoving = false, int fadeDuration = 0)
    {
        AudioSource audioSource = PlayAudio(groupName, soundKey, fadeDuration);

        if (audioSource == null)
        {
            Debug.LogWarning($"Failed to play 3D audio for group '{groupName}' and sound '{soundKey}'.");
            return;
        }

        // Configure the AudioSource for 3D sound
        audioSource.spatialBlend = 1f;
        audioSource.transform.position = targetTransform.position;

        // If the target is moving, start the coroutine to follow its position
        if (isMoving)
            FollowTransformWhilePlaying(audioSource, targetTransform);
    }
    
    #region Internals Configs

    /// <summary>
    /// Setup sound pools
    /// </summary>
    private void InitializePools()
    {
        foreach (var audioGroupEntry in _audioConfig.Audios)
        {
            string groupName = audioGroupEntry.Key;
            AudioGroup audioGroup = audioGroupEntry.Value;

            _activeAudioSources[groupName] = new HashSet<AudioSource>();

            _audioPools[groupName] = new ObjectPool<AudioSource>(
                createFunc: () => CreatePooledAudioSource(audioGroup),
                actionOnGet: (audioSource) =>
                {
                    audioSource.gameObject.SetActive(true);
                    _activeAudioSources[groupName].Add(audioSource);
                },
                actionOnRelease: (audioSource) =>
                {
                    audioSource.transform.DOKill();
                    audioSource.gameObject.SetActive(false);
                    _activeAudioSources[groupName].Remove(audioSource);
                },
                actionOnDestroy: (audioSource) => Destroy(audioSource.gameObject),
                collectionCheck: false,
                defaultCapacity: audioGroup.PoolLenght,
                maxSize: audioGroup.PoolLenght
            );
        }
    }

    /// <summary>
    /// Configures an AudioSource based on the Sound data.
    /// </summary>
    const int SOUND_2D = 0;
    private void ConfigureAudioSource(AudioSource audioSource, Sound sound)
    {
        audioSource.clip = sound.Clip;
        audioSource.volume = sound.Volume;
        audioSource.pitch = sound.Pitch;
        audioSource.panStereo = sound.Pan;
        audioSource.loop = sound.Loop;
        audioSource.priority = sound.Priority;
        audioSource.spatialBlend = SOUND_2D;

        // Configure 3D settings
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = sound.MinDistance;
        audioSource.maxDistance = sound.MaxDistance;
    }

    /// <summary>
    /// Releases the AudioSource back to the pool when playback is finished.
    /// </summary>
    private IEnumerator ReleaseAudioSourceWhenDone(AudioSource audioSource, ObjectPool<AudioSource> pool)
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        pool.Release(audioSource);
    }

    /// <summary>
    /// Creates a new configured AudioSource.
    /// </summary>
    private AudioSource CreatePooledAudioSource(AudioGroup audioGroup)
    {
        GameObject audioSourceGO = new GameObject($"AudioSource_{audioGroup.AudioMixerGroup.name}");
        AudioSource audioSource = audioSourceGO.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioGroup.AudioMixerGroup;
        audioSource.playOnAwake = false;
        // Maintains organized hierarchy
        audioSourceGO.transform.SetParent(transform);

        return audioSource;
    }

    /// <summary>
    /// Finds the lowest priority active AudioSource in the pool that can be replaced.
    /// </summary>
    /// <param name="groupName">The name of the audio group.</param>
    /// <param name="newSoundPriority">The priority of the new sound to be played.</param>
    /// <returns>The AudioSource with the lowest priority, or null if none can be replaced.</returns>
    private AudioSource FindLowestPrioritySource(string groupName, int newSoundPriority)
    {
        if (!_activeAudioSources.TryGetValue(groupName, out var activeSources) || activeSources.Count == 0)
        {
            return null;
        }

        AudioSource lowestPrioritySource = null;

        foreach (var audioSource in activeSources)
        {
            if (audioSource.isPlaying && (lowestPrioritySource == null || audioSource.priority > lowestPrioritySource.priority))
            {
                lowestPrioritySource = audioSource;
            }
        }

        return lowestPrioritySource != null && lowestPrioritySource.priority > newSoundPriority
            ? lowestPrioritySource
            : null;
    }

    /// <summary>
    /// Makes the AudioSource follow the target Transform while it is playing, using DOTween.
    /// </summary>
    /// <param name="audioSource">The AudioSource to move.</param>
    /// <param name="targetTransform">The Transform to follow.</param>
    /// <param name="followSpeed">The speed at which the AudioSource follows the Transform. Default is 5.</param>
    private void FollowTransformWhilePlaying(AudioSource audioSource, Transform targetTransform, float followSpeed = 5f)
    {
        if (audioSource == null || targetTransform == null)
        {
            Debug.LogWarning("AudioSource or targetTransform is null. Cannot follow transform.");
            return;
        }

        // Cancel any previous tweens on the audio source's transform
        audioSource.transform.DOKill();


        audioSource.transform.DOMove(targetTransform.position, 1f / followSpeed)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Restart)
        .OnUpdate(() =>
        {
            // Update the target position each frame
            audioSource.transform.DOMove(targetTransform.position, 1f / followSpeed);
        });
    }

    #endregion
}
