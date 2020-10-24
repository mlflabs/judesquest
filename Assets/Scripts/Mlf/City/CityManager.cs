using UnityEngine;
using System.Collections.Generic;
using System;
using Mlf.City.Buildings;
using Mlf.Characters;
using Mlf.TimeDate;
using Mlf.Traffic;
using Mlf.City.Actions;

namespace Mlf.City
{
    public class CityManager : MonoBehaviour
    {
        public static CityManager instance;

        [SerializeField] private List<BuildingCmp> _buildings = new List<BuildingCmp>();
        [SerializeField] private List<CharacterContext> _characters = new List<CharacterContext>();

        [SerializeField]
        private Dictionary<CharacterContext, GameObject> onStageCharacters =
            new Dictionary<CharacterContext, GameObject>();
        //[Help("Where to instantiate characters")]
        //public Transform charactersParent;

        public int maxOnStageCharacters = 20;

        void Awake()
        {

            if (instance == null)
            {

                instance = this;
                //DontDestroyOnLoad(this.gameObject);

                //Rest of your Awake code

            }
            else
            {
                Destroy(this);
            }
        }


        private void Start()
        {
            GameTimeManager.instance.minInterval += onTimeTick;
        }

        private void onTimeTick(int hour, int min)
        {
            //Debug.Log("On Time Tick: " + hour + ": " + min);

            for (int i = 0; i < _characters.Count; i++)
            {
                if (_characters[i].onStage)
                    continue;

                CharactersEventActions.CharacterEvents(_characters[i], hour, min);

            }


        }



        #region City Management Functions

        public void instantiateCharacter(CharacterContext character, Transform start)
        {

            if (onStageCharacters.ContainsKey(character))
            {
                Debug.LogError("Duplicate Character being added: " + character.characterSO.name);
                return;

            }

            var obj = Instantiate(character.characterSO.prefab);

            obj.GetComponent<CharacterCmp>().characterContext = character;

            character.onStage = true;
            onStageCharacters.Add(character, obj);

            obj.transform.position = start.position;
            obj.transform.rotation = start.rotation;
        }

        public void destroyCharacter(CharacterContext character)
        {

            character.onStage = false;
            Destroy(onStageCharacters[character]);
            onStageCharacters.Remove(character);

        }

        public bool canAddCharacterToStage()
        {
            return (onStageCharacters.Count < maxOnStageCharacters);
        }



        #endregion




        #region List Setters

        public void AddBuilding(BuildingCmp b)
        {
            if (_buildings.Contains(b)) return;

            _buildings.Add(b);
        }

        public void RemoveBuilding(BuildingCmp b)
        {
            _buildings.Remove(b);
        }

        public void AddCharacter(CharacterContext context)
        {
            if (_characters.Contains(context)) return;

            _characters.Add(context);
        }

        public void RemoveCharacter(CharacterContext context)
        {
            _characters.Remove(context);
        }

        #endregion

    }

}
