using UnityEngine;

namespace Mlf.Characters
{



    [CreateAssetMenu(menuName = "Mlf/City/Schedule")]
    public class ScheduleSO : ScriptableObject
    {

        public string firstName;
        public string lastName;
        public string nickName;

        public CharacterAgeTypes ageType;
        public CharacterSex sex;
        public int age;


        public GameObject prefab;




    }

}
