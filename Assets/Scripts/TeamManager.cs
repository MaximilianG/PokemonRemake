using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private PokemonBehaviour[] playerTeam = new PokemonBehaviour[6];
    public PokemonBehaviour[] PlayerTeam { get { return playerTeam; } }

    [SerializeField] private PokemonBehaviour[] enemyTeam = new PokemonBehaviour[6];
    public PokemonBehaviour[] EnemyTeam { get { return enemyTeam; } }

    public bool isAllTeamDead(PokemonBehaviour[] team)
    {
        foreach (PokemonBehaviour pokemon in team)
        {
            if (pokemon)
            {
                if (pokemon.IsAlive == true)
                    return false;
            }
        }
        return true;
    }
}
