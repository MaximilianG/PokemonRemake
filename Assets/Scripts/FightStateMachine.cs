using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightStateMachine : MonoBehaviour
{
    enum BattleState {START = 0, ACTIONCHOICE = 1, PLAYERTURN = 2, ENEMYTURN = 3, POKEMONCHOICE = 4, WIN = 5, LOSE = 6 }

    [SerializeField] private BattleState battleState;
    [SerializeField] private TeamManager teamManager;

    private PokemonBehaviour playerCurrentPokemon;
    private PokemonBehaviour enemyCurrentPokemon;

    [Header("Enemy UI elements")]
    [SerializeField] private TMP_Text UI_EnemyNameText;
    [SerializeField] private TMP_Text UI_EnemyLevelText;
    [SerializeField] private Slider UI_EnemyHealthBar;
    [SerializeField] private Image UI_EnemyPokemonImage;

    [Header("Player UI elements")]
    [SerializeField] private TMP_Text UI_PlayerNameText;
    [SerializeField] private TMP_Text UI_PlayerLevelText;
    [SerializeField] private Slider UI_PlayerHealthBar;
    [SerializeField] private Image UI_PlayerPokemonImage;

    [Header("Global UI elements")]
    [SerializeField] private TMP_Text UI_DialogueText;
    [SerializeField] private TMP_Text UI_LeftButtonText;
    [SerializeField] private TMP_Text UI_RightButtonText;
    [SerializeField] private GameObject UI_Buttons;

    private void Start()
    {
        battleState = BattleState.START;
        SetupBattle();
    }

    private void SetupBattle()
    {
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        Debug.Log("Le combat commence !");
        // Mettre un message dans la boîte de dialogue comme quoi que le combat commence.
        UI_DialogueText.text = "Le combat commence !";

        yield return new WaitForSeconds(2f);

        playerCurrentPokemon = teamManager.PlayerTeam[0]; // Les premiers pokémon de l'équipe commencent le combat par défaut.
        enemyCurrentPokemon = teamManager.EnemyTeam[0];
        UpdatePokemonUI(playerCurrentPokemon, 0); // playerID 0 - Player | 1 - Enemy
        UpdatePokemonUI(enemyCurrentPokemon, 1); // playerID 0 - Player | 1 - Enemy
        UI_LeftButtonText.text = playerCurrentPokemon.Attacks[0].Name;
        UI_RightButtonText.text = playerCurrentPokemon.Attacks[1].Name;

        yield return new WaitForSeconds(2f);

        battleState = BattleState.ACTIONCHOICE;
        ChoiceSequence(); // choix de l'action à faire
    }

    private void ChoiceSequence()
    {
        Debug.Log("Que voulez-vous faire ?");
        // Mettre un message dans la boîte de dialogue comme quoi que l'on doit choisir une action.
        UI_DialogueText.text = "Que voulez-vous faire ?";
        UI_Buttons.SetActive(true);
    }

    private void UpdatePokemonUI(PokemonBehaviour newPokemon, int playerID) // playerID 0 - Player | 1 - Enemy
    {
        if (playerID == 0)
        {
            playerCurrentPokemon = newPokemon;
            UI_PlayerPokemonImage.color = Color.white;
            UI_PlayerPokemonImage.sprite = newPokemon.BackSprite;
            UI_PlayerNameText.text = newPokemon.Name;
            UI_PlayerLevelText.text = newPokemon.Level.ToString();
            UI_PlayerHealthBar.minValue = 0f;
            UI_PlayerHealthBar.maxValue = newPokemon.MaxHP;
            UI_PlayerHealthBar.value = newPokemon.CurrentHP;
        }
        else if (playerID == 1)
        {
            enemyCurrentPokemon = newPokemon;
            UI_EnemyPokemonImage.color = Color.white;
            UI_EnemyPokemonImage.sprite = newPokemon.FrontSprite;
            UI_EnemyNameText.text = newPokemon.Name;
            UI_EnemyLevelText.text = newPokemon.Level.ToString();
            UI_EnemyHealthBar.value = 0f;
            UI_EnemyHealthBar.maxValue = newPokemon.MaxHP;
            UI_EnemyHealthBar.value = newPokemon.CurrentHP;
        }
    }

    private void UpdatePokemonHp(int playerID)
    {
        if (playerID == 0)
        {
            UI_PlayerHealthBar.value = playerCurrentPokemon.CurrentHP;
        }
        else if (playerID == 1)
        {
            UI_EnemyHealthBar.value = enemyCurrentPokemon.CurrentHP;
        }
    }

    public void StartAttackSequence(int buttonID)
    {
        UI_Buttons.SetActive(false);
        StartCoroutine(AttackSequence(buttonID));
    }

    IEnumerator AttackSequence(int buttonID)
    {
        // Initialisation de l'attaque de l'adversaire
        Attack enemyRandomAttack;
        if (Random.Range(0f, 100f) < 50f) // aléatoire entre les deux attaques
            enemyRandomAttack = enemyCurrentPokemon.Attacks[0];
        else
            enemyRandomAttack = enemyCurrentPokemon.Attacks[1];

        // Initialisation de l'attaque du player
        Attack playerAttack = playerCurrentPokemon.Attacks[buttonID]; // attackID 0 - Left | 1 - Right

        UI_DialogueText.text = ""; // la phase d'attaque commence

        yield return new WaitForSeconds(1f);

        #region Si l'adversaire est plus rapide
        if (enemyCurrentPokemon.Speed > playerCurrentPokemon.Speed)
        {
            #region Attaque de l'adversaire
            battleState = BattleState.ENEMYTURN;
            UI_DialogueText.text = enemyCurrentPokemon.Name + " utilise " + enemyRandomAttack.Name;
            yield return new WaitForSeconds(0.75f);

            enemyCurrentPokemon.LaunchAttack(enemyRandomAttack, playerCurrentPokemon); // l'adversaire attaque le joueur
            UpdatePokemonHp(0); // playerID 0 - Player | 1 - Enemy

            yield return new WaitForSeconds(0.75f);
            #endregion
            #region Si le pokemon du joueur est encore en vie
            if (playerCurrentPokemon.IsAlive) // si le pokemon du joueur est encore en vie après l'attaque
            {
                #region Attaque du joueur
                battleState = BattleState.PLAYERTURN;
                UI_DialogueText.text = playerCurrentPokemon.Name + " utilise " + playerAttack.Name;
                yield return new WaitForSeconds(0.75f);

                playerCurrentPokemon.LaunchAttack(playerAttack, enemyCurrentPokemon); // le joueur attaque l'adversaire à son tour
                UpdatePokemonHp(1); // playerID 0 - Player | 1 - Enemy

                yield return new WaitForSeconds(0.75f);
                #endregion
                #region Si le pokemon adverse est mort
                if (!enemyCurrentPokemon.IsAlive) // si le pokemon adverse est mort apres l'attaque du joueur
                {
                    UI_DialogueText.text = "Le pokemon adverse est mort.";

                    yield return new WaitForSeconds(1f);
                    #region Si toute la team adverse est morte : Victoire
                    if (teamManager.isAllTeamDead(teamManager.EnemyTeam)) // si toute la team de l'adversaire est morte
                    {
                        UI_DialogueText.text = "Il ne reste plus aucun pokemon dans la team adverse...";
                        yield return new WaitForSeconds(1f);

                        UI_DialogueText.text = "Vous avez gagné !";
                        battleState = BattleState.WIN; // **** C'EST WIN ****
                        End();
                    }
                    #endregion
                    #region Sinon, s'il reste un ou plusieurs pokemon dans la team adverse : CHANGEMENT DE POKEMON
                    else
                    {
                        UI_DialogueText.text = "Choix d'un autre pokemon en cours...";
                        battleState = BattleState.POKEMONCHOICE; // **** LAISSER LE CHOIX DU NOUVEAU POKEMON A L'ADVERSAIRE ****
                    }
                    #endregion
                }
                #endregion
                else
                {
                    battleState = BattleState.ACTIONCHOICE;
                    ChoiceSequence(); // choix de l'action à faire
                }
            }
            #endregion
            #region Sinon, le pokemon du joueur est mort
            else // si le pokemon du joueur est mort suite à l'attaque
            {
                UI_DialogueText.text = "Votre pokemon est mort.";

                yield return new WaitForSeconds(1f);

                #region Si toute la team du joueur est morte : DEFAITE
                if (teamManager.isAllTeamDead(teamManager.PlayerTeam)) // si toute la team du joueur est morte
                {
                    UI_DialogueText.text = "Tous vos pokemons sont K.O.";
                    battleState = BattleState.LOSE; // **** C'EST LOSE ****

                    yield return new WaitForSeconds(1f);
                    UI_DialogueText.text = "Vous avez perdu.";
                    End();
                }
                #endregion
                #region Sinon, s'il reste un ou plusieurs pokemon dans la team du joueur : CHANGEMENT DE POKEMON
                else
                {
                    UI_DialogueText.text = "Choisissez un nouveau pokemon...";
                    battleState = BattleState.POKEMONCHOICE; // **** LAISSER LE CHOIX DU NOUVEAU POKEMON AU JOUEUR ****
                }
            }
            #endregion
        }
        #endregion
        #endregion
        #region Si le joueur est plus rapide
        else // si le joueur commence
        {
            #region Attaque du joueur
            battleState = BattleState.PLAYERTURN;

            UI_DialogueText.text = playerCurrentPokemon.Name + " utilise " + playerAttack.Name;
            yield return new WaitForSeconds(0.75f);

            playerCurrentPokemon.LaunchAttack(playerAttack, enemyCurrentPokemon); // attaque l'adversaire
            UpdatePokemonHp(0); // playerID 0 - Player | 1 - Enemy

            yield return new WaitForSeconds(0.75f);
            #endregion
            #region Si le pokemon adverse est encore en vie
            if (enemyCurrentPokemon.IsAlive) // si le pokemon adverse est encore en vie après l'attaque du joueur
            {
                #region Attaque de l'adversaire
                UI_DialogueText.text = enemyCurrentPokemon.Name + " utilise " + enemyRandomAttack.Name;
                yield return new WaitForSeconds(0.75f);

                enemyCurrentPokemon.LaunchAttack(enemyRandomAttack, playerCurrentPokemon); // l'adversaire attaque le joueur à son tour
                UpdatePokemonHp(1); // playerID 0 - Player | 1 - Enemy

                yield return new WaitForSeconds(0.75f);
                #endregion
                #region Si le pokemon du joueur est mort
                if (!playerCurrentPokemon.IsAlive) // si le pokemon du joueur est mort apres l'attaque de l'adversaire
                {
                    UI_DialogueText.text = "Votre pokemon est mort.";

                    yield return new WaitForSeconds(1f);
                    #region Si toute la team du joueur est morte : DEFAITE
                    if (teamManager.isAllTeamDead(teamManager.PlayerTeam)) // si toute la team du joueur est morte
                    {
                        UI_DialogueText.text = "Il ne reste plus aucun pokemon dans votre team...";
                        yield return new WaitForSeconds(1f);

                        UI_DialogueText.text = "Vous avez perdu !";
                        battleState = BattleState.LOSE; // **** C'EST LOSE ****
                        End();
                    }
                    #endregion
                    #region Sinon, si un ou plusieur pokemon est encore en vie : CHANGEMENT DE POKEMON
                    else
                    {
                        UI_DialogueText.text = "Choix d'un autre pokemon en cours...";
                        battleState = BattleState.POKEMONCHOICE; // **** LAISSER LE CHOIX DU NOUVEAU POKEMON AU JOUEUR ****
                    }
                    #endregion
                }
                #endregion
                battleState = BattleState.ACTIONCHOICE;
                ChoiceSequence(); // choix de l'action à faire
            }
            #endregion
            #region Sinon, si le pokemon adverse est mort
            else // si le pokemon adverse est mort suite à l'attaque du joueur
            {
                UI_DialogueText.text = "Le pokemon adverse est mort.";

                yield return new WaitForSeconds(1f);

                #region Si toute la team adverse est morte : VICTOIRE
                if (teamManager.isAllTeamDead(teamManager.EnemyTeam)) // si toute la team adverse est morte
                {
                    UI_DialogueText.text = "Tous les pokemons adverses sont K.O.";
                    battleState = BattleState.WIN; // **** C'EST WIN ****

                    yield return new WaitForSeconds(1f);
                    UI_DialogueText.text = "Vous avez gagné.";
                    End();
                }
                #endregion
                #region Sinon, si un ou plusieurs pokemon adverse est encore en vie : CHANGEMENT DE POKEMON
                else
                {
                    UI_DialogueText.text = "Choisissez un nouveau pokemon...";
                    battleState = BattleState.POKEMONCHOICE; // **** LAISSER LE CHOIX DU NOUVEAU POKEMON A L'ADVERSAIRE ****
                }
                #endregion
            }
            #endregion
        }
        #endregion
    }

    public void ChangePokemon()
    {

    }

    public void End()
    {
        Debug.Log("Combat finis");
    }
}
