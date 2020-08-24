using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace ARPolis.Data
{
    /// <summary>
    /// Responsible for assigning JSON Data to Class Structure
    /// </summary>
    public class JsonGenericData : MonoBehaviour
    {

        /// <summary>
        /// Loads list of objects reading a json file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonFilePath">must be into Resources folder</param>
        /// <returns></returns>
        public static T[] LoadDataFromJson<T>(string jsonFilePath)
        {
            string jsonData = FixJsonItems(LoadResourceTextfile(jsonFilePath));

            if (string.IsNullOrEmpty(jsonData))
            {
                Debug.Log("error reading " + jsonFilePath);
                return null;
            }
            else
            {
                //if json has limited data return null
                //something is wrong or the file is corrupted
                if (jsonData.Length < 10)
                {
                    Debug.Log("error on file " + jsonFilePath);
                    return null;
                }
                else
                {
                    return JsonHelper.FromJson<T>(jsonData);
                }
            }
        }
                
        public static IEnumerator LoadJsonData<T>(string filename)//, List<T> t)
        {
            //ios
            if (CommonData.ISiOS())
            {
                FileInfo fInfo = new FileInfo(filename);
                if (fInfo == null || !fInfo.Exists)
                {
                    Debug.Log("NOT FOUND: " + filename);
                    yield return null;
                }
                else
                {
                    string myjsondata = File.ReadAllText(fInfo.FullName);
                    if (myjsondata.Length > 10)
                    {
                        myjsondata = FixJsonItems(myjsondata);
                        yield return JsonHelper.FromJson<T>(myjsondata).ToList();
                    }
                    else
                    {
                        Debug.Log("empty - NOT FOUND: " + filename);
                        yield return null;
                    }
                }
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
                    string jsonData = www.downloadHandler.text;
                    if (string.IsNullOrEmpty(jsonData) || jsonData.Length < 10)
                    {
                        yield return null;
                    }
                    else
                    {
                        jsonData = FixJsonItems(www.downloadHandler.text);
                        yield return JsonHelper.FromJson<T>(jsonData).ToList();
                    }
                }
            }
        }


        private static string LoadResourceTextfile(string filePath)
        {
            TextAsset targetFile = Resources.Load<TextAsset>(filePath);

            if (targetFile == null)
            {
                Debug.LogWarning(filePath + ".json is missing!!!");
                return string.Empty;
            }
            return targetFile.text;
        }

        private static string FixJsonItems(string value)
        {
            value = "{\"Items\":" + value + "}";
            return value;
        }

    }

}
