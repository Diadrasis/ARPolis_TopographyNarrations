using ARPolis.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace ARPolis.Examples
{

    public class ExampleCases : MonoBehaviour
    {
        AudioClip myAudioClip;
        RawImage image;
        List<JsonClassTest> jsonClassTests;

        //Json
        private IEnumerator JsonToClass()
        {
            string jsonTestFile = Path.Combine(Application.streamingAssetsPath, "jsonTest");
            jsonTestFile += ".json";

            CoroutineWithData cd = new CoroutineWithData(this, StaticData.LoadJsonData<JsonClassTest>(jsonTestFile));
            yield return cd.Coroutine;
            if (cd.result == null) { Debug.LogWarning("Error reading " + jsonTestFile); }
            else
            {
                jsonClassTests = cd.result as List<JsonClassTest>;
            }
        }

        //Image
        private IEnumerator GetImageFileExample()
        {
            string imageFile = Path.Combine(Application.streamingAssetsPath, "image.png");//file must be into StreamingAssets folder
            Debug.Log(imageFile);

            CoroutineWithData cd = new CoroutineWithData(this, DigitalGenericData.LoadTextureData<Texture2D>(imageFile));
            yield return cd.Coroutine;
            if (cd.result == null)
            {
                Debug.LogWarning("Error reading " + imageFile);
            }
            else
            {
                image.texture = (Texture2D)cd.result;
            }
        }
        
        //Audio
        private IEnumerator GetAudioFileExample()
        {
            string imageFile = Path.Combine(Application.streamingAssetsPath, "audio.ogg");//file must be into StreamingAssets folder
            Debug.Log(imageFile);

            CoroutineWithData cd = new CoroutineWithData(this, DigitalGenericData.LoadAudioData<AudioClip>(imageFile));
            yield return cd.Coroutine;
            if (cd.result == null)
            {
                Debug.LogWarning("Error reading " + imageFile);
            }
            else
            {
                myAudioClip = (AudioClip)cd.result;
            }
        }
    }

    [Serializable]
    public class JsonClassTest
    {
        public string id, topic_ID, attribute_ID, testimony_ID, attribute_Lang, attribute_Label, attribute_Value;
    }

}
