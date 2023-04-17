public enum AudioType
{
    None,

    #region Character Sounds
    //Merlot
    PlayerAttack,
    PlayerWalk1,
    PlayerWalk2,
    PlayerWalk3,
    PlayerWalk4,
    PlayerWalk5,
    PlayerDeath,
    PlayerDamaged1,
    PlayerDamaged2,
    PlayerDamaged3,
    PlayerDamaged4,
    PlayerAttack1,
    PlayerAttack2,
    PlayerAttack3,
    PlayerAttack4,
    PlayerAttack5,
    PlayerChargedAttack1,
    PlayerChargedAttack2,
    #endregion

    #region Enemy Sounds
    PopperDeath,
    PopperEnemyExplode,
    RotEnemyNoise,
    RotDeath,
    CarrotAttack1,
    CarrotAttack2,
    CarrotAttack3,
    CarrotDamage1,
    CarrotDamage2,
    CarrotDamage3,
    CarrotDeath1,
    CarrotDeath2,
    CarrotDeath3,


    #region Bosses
    BossIntro,
    EnemySpawnBoss,
    BossDeath,
    Carrot_L_Attack1,
    Carrot_L_Attack2,
    Carrot_L_Attack3,
    Carrot_L_Attack4,
    Carrot_L_Damage1,
    Carrot_L_Damage2,
    Carrot_L_Damage3,
    Carrot_L_Death,


    #endregion
    #endregion

    #region Music Tracks
    MainMenu,
    RotBoss,
    DungeonOne,
    #endregion

    #region NPCs
    NpcDialogue1,
    NpcDialouge2,
    NpcDialouge3,
    NpcDialouge4,
    NpcDialouge5,
    NpcDialouge6,
    PotatoMurmur1
    #endregion

}
//Audio files under the same audioSource cannot play at the same time. They can only be handled one at a time.