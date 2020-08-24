using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace ARPolis.Data
{
    /// <summary>
    /// Responsible for loading digitial file types
    /// </summary>
    public class DigitalGenericData : MonoBehaviour
    {

        //Load Texture
        public static IEnumerator LoadTextureData<T>(string filename)
        {
            byte[] imgData;
            Texture2D tex = new Texture2D(2, 2);

            if (CommonData.ISiOS())
            {
                yield return LoadTexture(filename);
            }
            else
            {
                UnityWebRequest www = UnityWebRequest.Get(filename);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                    yield return null;
                }
                else
                {
                    imgData = www.downloadHandler.data;
                    //Load raw Data into Texture2D 
                    tex.LoadImage(imgData);
                    yield return tex;
                }
            }
        }

        /// <summary>
        /// Narration or Sound
        /// </summary>
        /// <param name="fileNamePath"></param>
        /// <returns></returns>
        public static IEnumerator LoadAudioData<T>(string fileNamePath)
        {
            if (CommonData.audioFolder == CommonData.LoadFromFolder.RESOURCES)
            {
                int indx = fileNamePath.LastIndexOf(".");
                string nameWithoutExtension = fileNamePath.Remove(indx);
                if (string.IsNullOrEmpty(nameWithoutExtension)) yield break;
                //get audio clip
                AudioClip clip = Resources.Load<AudioClip>(nameWithoutExtension);
                //if clip exists
                if (clip != null)
                {
                    //set the new clip
                    yield return clip;
                }
                yield break;
            }
            else
            if (CommonData.audioFolder == CommonData.LoadFromFolder.STREAMING_ASSETS)
            {
                //replace file extension to ogg if is mp3 from database
                //because all audio clips are converted to ogg in order to play inside unity
                fileNamePath = fileNamePath.Replace(".mp3", ".ogg");
                string url = Path.Combine(Application.streamingAssetsPath, fileNamePath);

                //stop loading audio if audio name is null
                if (string.IsNullOrEmpty(url)) { yield return null;  yield break; }

                //start loading
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS))
                {
                    yield return www.SendWebRequest();

                    if (www.isNetworkError)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        //get audio clip
                        AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                        //if clip exists
                        if (clip != null)
                        {
                            //set the new clip
                            yield return clip;
                        }
                        else
                        {
                            Debug.Log("Audio file " + fileNamePath + "is null");
                            yield return null;
                        }
                    }
                }
            }
            yield break;
        }


        /// <summary>
        /// Load a PNG or JPG file from disk to a Texture2D
        /// Returns null if load fails
        /// </summary>
        static Texture2D LoadTexture(string FilePath)
        {

            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
                if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                    return Tex2D;                 // If data = readable -> return texture
            }

            return null;                     // Return null if load failed
        }


    }

}
