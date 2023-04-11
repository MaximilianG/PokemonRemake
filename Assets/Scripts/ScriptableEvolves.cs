using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableEvolves : ScriptableObject
{
    [SerializeField] private List<EvolveStruct> evolvesList = new List<EvolveStruct>();

    public void AddEvolve()
    {
        evolvesList.Add(new EvolveStruct());
    }

    public void RemoveEvolve(int i)
    {
        evolvesList.Remove(evolvesList[i]);
    }

    public void RemoveEvolve(EvolveStruct _learnableAttackStruct)
    {
        evolvesList.Remove(_learnableAttackStruct);
    }

    [System.Serializable]
    public struct EvolveStruct
    {
        public PokemonData pokemon;
        public int level;
    }

    public List<EvolveStruct> EvolvesStructList
    {
        get { return evolvesList; }
        set { evolvesList = value; }
    }

    public List<PokemonData> pokemonList
    {
        get
        {
            List<PokemonData> pokemonListToReturn = new List<PokemonData>();
            foreach (EvolveStruct evolve in evolvesList)
            {
                pokemonListToReturn.Add(evolve.pokemon);
            }
            return pokemonListToReturn;
        }
        set
        {
            pokemonList = value;
        }
    }

    public List<int> evolveLevelList
    {
        get
        {
            List<int> evolveLevelListToReturn = new List<int>();
            foreach (EvolveStruct evolve in evolvesList)
            {
                evolveLevelListToReturn.Add(evolve.level);
            }
            return evolveLevelListToReturn;
        }
    }
}
