using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System.Runtime;
using Random = UnityEngine.Random;

public class GameControler : MonoBehaviour
{
    public static GameControler instance;

    public Text debug;
    public float[] audFloat;
    public byte[] audByte;
    private Dictionary<string, int> ConvertToNumber = new Dictionary<string, int>()
    {
        {"zero", 0 },
        {"um", 1 },
        {"dois", 2},
        {"tres", 3},
        {"quatro", 4 },
        {"cinco", 5 },
        {"seis", 6 },
        {"sete", 7 },
        {"oito", 8 },
        {"novo", 9 },
    };
    //Emitir som
    AudioSource aud;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Audio Source responsavel por emitir a voz
        aud = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            AnimationControler.instance.PlaySword();
        }
        if (Input.GetKey(KeyCode.W))
        {
            AnimationControler.instance.PlayShield();
        }
        if (Input.GetKey(KeyCode.A))
        {
            AnimationControler.instance.PlayPotion();
        }
        if (Input.GetKey(KeyCode.S))
        {
            AnimationControler.instance.PlayFirebol();
        }

    }
    // Start recording with built-in Microphone and play the recorded audio right away
    public void GravarAudio()
    {
        aud.clip = Microphone.Start("Built-in Microphone", true, 2, 8000);
        //dar um tempo de 2 segundos para desativar esse botão
        // LeanTween.delayedCall(2f, PararAudio);
        Invoke("PararAudio", 2f);
    }

    private void PararAudio()
    {
        Microphone.End("Built-in Microphone");
        //aud.Play();
        byte[] audByte = new byte[2 * aud.clip.samples];
        audByte = ConvertAudioToByteArray(aud.clip);
        StartCoroutine(ClientSkt.SendVoice(audByte));   
    }

    private byte[] ConvertAudioToByteArray(AudioClip clip)
    {
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);
        Int16[] intData = new Int16[samples.Length];
        //converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]
        Byte[] bytesData = new Byte[samples.Length * 2];
        //bytesData array is twice the size of dataSource array because a float converted in Int16 is 2 bytes.

        int rescaleFactor = 32767; //to convert float to Int16
        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }
        return bytesData;
    }

    public void OnRecognize(string audioRecognized)
    {
        newChecarResp(audioRecognized);
    }

    //usa palavra reconhecida para chamar uma ação no jogo
    private void newChecarResp(string recognized)
    {
        Debug.Log("recognized = " + recognized + ".");

        if (ConvertToNumber.ContainsKey(recognized) && ConvertToNumber[recognized] <= 4 && ConvertToNumber[recognized] > 0)
        {
            int number = ConvertToNumber[recognized];
            if (number == 1)
            {
                AnimationControler.instance.PlaySword();
            }
            else if (number == 2)
            {
                AnimationControler.instance.PlayShield();
            }
            else if (number == 3)
            {
                AnimationControler.instance.PlayFirebol();
            }
            else if (number == 4)
            {
                AnimationControler.instance.PlayPotion();

            }
            debug.text =  number.ToString();
        }
       
    }
    


    
}