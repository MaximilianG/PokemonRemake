using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonBehaviour : MonoBehaviour
{
    [SerializeField] private PokemonData _data;

    private int _id;
    private string _name;
    private int _level;
    private float _maxHP;
    private float _currentHP;
    private int _attack;
    private int _defense;
    private int _speed;
    private int _specialAttack;
    private int _specialDefense;
    private bool _isAlive = true;
    private Sprite _frontSprite;
    private Sprite _backSprite;
    private Attack[] _attacks;
    private List<PokemonData> _evolves = new List<PokemonData>();

    public PokemonData Data { get { return _data; } }
    public int Id { get { return _id; } }
    public string Name { get { return _name; } }
    public int Level { get { return _level; } }
    public float MaxHP { get { return _maxHP; } set { _maxHP = value; } }
    public int Attack { get { return _attack; } }
    public int Defense { get { return _defense; } }
    public int Speed { get { return _speed; } }
    public int SpecialAttack { get { return _specialAttack; } }
    public int SpecialDefense { get { return _specialDefense; } }
    public bool IsAlive { get { return _isAlive; } set { _isAlive = value; } }
    public Sprite FrontSprite { get { return _frontSprite; } }
    public Sprite BackSprite { get { return _backSprite; } }
    public Attack[] Attacks { get { return _attacks; } }
    public List<PokemonData> Evolves { get { return _evolves; } }
    public float CurrentHP { get { return _currentHP; } set { _currentHP = value; } }

    private void Start()
    {
        _id = _data.Id;
        _name = _data.Name;
        _level = _data.Level;
        _maxHP = _data.HealthPoints;
        _attack = _data.Attack;
        _defense = _data.Defense;
        _speed = _data.Speed;
        _specialAttack = _data.SpecialAttack;
        _specialDefense = _data.SpecialDefense;
        _frontSprite = _data.FrontSprite;
        _backSprite = _data.BackSprite;
        _attacks = _data.Attacks;
        _evolves = _data.Evolves;
        _currentHP = _maxHP;
    }

    public void LaunchAttack(Attack attackData, PokemonBehaviour target)
    {
        float damage = 0f;
        damage = this._attack * attackData.AttackRatio; // on Calcul les dégâts à infliger brut
        damage = damage - target.Defense; // on converti les dégâts brut en fonction de la défense de l'adversaire.

        if (damage <= 0f)
            damage = 1f;

        target.CurrentHP -= damage;
        if (target.CurrentHP <= 0f)
        {
            target.CurrentHP = 0f;
            target.IsAlive = false;
        }
    }
}
