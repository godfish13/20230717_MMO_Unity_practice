using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> audioClipTable = new Dictionary<string, AudioClip>(); // Effect Sound 캐싱 해시테이블

    public void init()
    {
        GameObject root = GameObject.Find("@Sound");
        if(root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] SoundNames = System.Enum.GetNames(typeof(Define.Sound));

            for(int i = 0; i < SoundNames.Length - 1; i++)          // MaxCount는 Sound종류가 아니므로 -1
            {
                GameObject go = new GameObject { name = SoundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    public void Clear()
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        audioClipTable.Clear();
    }

    public void Play(string path, Define.Sound SoundType = Define.Sound.EffectSound,float pitch = 1.0f)     // AudioClip path받는 버전
    {
        AudioClip audioClip = GetorAddAudioClip(path, SoundType);
        Play(audioClip, SoundType, pitch);
    }

    public void Play(AudioClip audioClip, Define.Sound SoundType = Define.Sound.EffectSound, float pitch = 1.0f)    // AudioClip 직접 받는 버전
    {
        if (audioClip == null)
            return;
        if (SoundType == Define.Sound.Bgm)
        {                 
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];

            if (audioSource.isPlaying)
                audioSource.Stop();
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.EffectSound];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetorAddAudioClip(string path, Define.Sound SoundType = Define.Sound.EffectSound)
    {                                               // 한번 사용한 AudioClip을 해시테이블에 캐싱해둬서 다음에 부를 때 다시 로딩하지 않도록 함
        if (path.Contains("Sounds/") == false)       // 입력한 path가 resource의 Sounds/를 안따라가면 더해줌      
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (SoundType == Define.Sound.Bgm)
        {
            audioClip = Managers.resourceMgr.Load<AudioClip>(path);                 
        }
        else
        {           
            if (audioClipTable.TryGetValue(path, out audioClip) == false)  // dictionary내에 key가 존재하면 out에 해당값을 연결하고 true반환
            {
                audioClip = Managers.resourceMgr.Load<AudioClip>(path);
                audioClipTable.Add(path, audioClip);
            }       
        }

        if (audioClip == null)
            Debug.Log($"missing AudioClip {path}");

        return audioClip;
    }
}
