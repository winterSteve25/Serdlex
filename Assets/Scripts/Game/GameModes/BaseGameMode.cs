﻿using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rooms;
using UnityEngine.Networking;

namespace Game.GameModes
{
    public class BaseGameMode
    {

        public readonly string DisplayName;

        public BaseGameMode(string displayName)
        {
            DisplayName = displayName;
        }

        public virtual void OnGameStart(RoomManager roomManager, int chances, bool validateWord)
        {
            SceneTransitioner.Instance.TransitionToScene(8);
        }
        
        public virtual void OnInit(GameController controller, WordleGame game)
        {
            
        }

        public virtual IEnumerator OnWordEnter(GameController controller, string word)
        {
            if (word.Length < controller.GetCurrentGame().CharactersCount)
            {
                controller.SetCanProceed(false);
                yield return controller.StartCoroutine(controller.ShowNotEnoughCharactersError());
                yield break;
            }

            yield return controller.StartCoroutine(controller.ShowGuessedWord(word));
        }

        public virtual IEnumerator OnWordCheck(GameController controller, string word)
        {
            yield return controller.CheckWord(word);
        }

        public virtual IEnumerator OnWordFinished(GameController controller, string word)
        {
            yield break;
        }

        public virtual IEnumerator IsWordValid(GameController controller, string word)
        {
            var uwr = UnityWebRequest.Get("https://api.dictionaryapi.dev/api/v2/entries/en/" + word);
            yield return uwr.SendWebRequest();
            
            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                yield break;
            }

            var dataStr = uwr.downloadHandler.text;
            if (string.IsNullOrEmpty(dataStr)) yield break;
            
            var data = JsonConvert.DeserializeObject<dynamic>(dataStr);
            controller.SetValidationState(data is JArray ? WordValidationState.Valid : WordValidationState.NotValid);
        }
    }
}