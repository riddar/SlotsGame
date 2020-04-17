using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Services
{
    public class SlotsDTO
    {
        public string GetReelsBySlotId(int id)
        {
            string url = "https://localhost:5001/api/v1/Reels/GetReelsBySlotId/";
            UnityWebRequest request = UnityWebRequest.Get(url + id);
            request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogWarning(request.error);
                return null;
            }

            JSONNode Reels = JSON.Parse(request.downloadHandler.text);
            Debug.Log(Reels);
            return Reels.ToString();
        }
    }

}
