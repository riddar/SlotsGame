using Assets.Scripts.Helpers;
using Newtonsoft.Json;
using SlotGame.Types.Models;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.DTO
{
    public class ReelsDTO
    {
        private const string url = "https://localhost:5001/api/v1/Reels/GetReelsBySlotId/";

        public ReelsDTO()
        {
            
        }
        public IEnumerator GetReelsBySlotId(int id)
        {
            UnityWebRequest request = UnityWebRequest.Get(url + id);
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
                Debug.LogWarning(request.error);

            var result = JsonConvert.DeserializeObject<List<Reel>>(request.downloadHandler.text);

            var t = result;
        }
    }
}
