using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPolis.Data
{

    public class CommonData : MonoBehaviour
    {

        public enum LoadFromFolder { STREAMING_ASSETS, RESOURCES, CUSTOM }
        public static LoadFromFolder jsonFolder = LoadFromFolder.STREAMING_ASSETS;
        public static LoadFromFolder imagesFolder = LoadFromFolder.STREAMING_ASSETS;
        public static LoadFromFolder audioFolder = LoadFromFolder.STREAMING_ASSETS;

        public static bool ISiOS()
        {
            return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
        }


    }

}
