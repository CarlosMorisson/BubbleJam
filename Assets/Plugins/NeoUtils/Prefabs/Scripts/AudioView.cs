using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEO.UiAnimations;

public class AudioView : MonoBehaviour
{
    [SerializeField] private RectTransform _audioConfigPanel;
    public void OpenAudioConfig()
    {
        _audioConfigPanel.NEOBounceIn(duration: 1f);
    }

    public void CloseAudioConfig()
    {
        _audioConfigPanel.NEOBounceOut(duration: 0.5f);
    }
    public void VolumeMusicSlider(float volume)
    {
        VolumeSliderGroup(volume, "Music");
    }
    public void VolumeSFXSlider(float volume)
    {
        VolumeSliderGroup(volume, "SFX");

    }
    private void VolumeSliderGroup(float volume, string groupName)
    {
        AudioController.Instance.SetAudioGroupVolume(groupName, volume);
    }
}
