using System;
using Mlf.Characters;
using Mlf.Traffic;
using UnityEngine;



namespace Mlf.City.Buildings
{
    [CreateAssetMenu(menuName = "Mlf/City/Buildings/Lawn")]
    public class LawnSO : ScriptableObject
    {

        [SerializeField] public Mesh[] meshes;
        [SerializeField] public Material[] materials;

        [Help("How much money gained from cutting")]
        public int value;
    }
}
