using RandomNameGeneratorLibrary;
using UnityEditor;
using UnityEngine;


namespace Mlf.Characters
{
    [CustomEditor(typeof(CharacterSO))]
    public class CharacterSOEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CharacterSO character = (CharacterSO)target;



            if (GUILayout.Button("Generate Random Data"))
            {
                character.age = Random.Range(5, 100);

                if (character.age < 12) character.ageType = CharacterAgeTypes.Child;
                else if (character.age < 19) character.ageType = CharacterAgeTypes.Teen;
                else if (character.age < 70) character.ageType = CharacterAgeTypes.Adult;
                else character.ageType = CharacterAgeTypes.Elderly;

                var rnd = new System.Random();
                character.sex = (CharacterSex)rnd.Next(System.Enum.GetNames(typeof(CharacterSex)).Length);

                var personGenerator = new PersonNameGenerator();
                character.lastName = personGenerator.GenerateRandomLastName();
                if (character.sex == CharacterSex.Male)
                {
                    character.firstName = personGenerator.GenerateRandomMaleFirstName();

                }
                else
                {
                    character.firstName = personGenerator.GenerateRandomFemaleFirstName();
                }


            }
        }
    }

}
