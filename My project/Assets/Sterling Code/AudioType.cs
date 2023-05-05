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
    //ImpactSounds
    CarrotImpactSound,
    RotImpactSound,
    //SeedSounds
    WaterSeed_Wave, 
    EarthSeed_RockDrag, 
    SunSeed_SunBeam, 
    #endregion

    #region Enemy Sounds
    PopperDeath,
    PopperEnemyExplode,
    RotEnemyNoise,
    RotGrowl_1,
    RotGrowl_2,
    RotGrowl_3,
    RotGrowl_4,
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
    CarrotSwing,
    #region Bosses
    #region VineLord
    VineLordIntro,
    VineLordSpawnEnemy,
    VineLordGrowl_1,
    VineLordGrowl_2,
    VineLordGrowl_3,
    VineLordGrowl_4,
    VineLordGrowl_5,
    BossDeath,
    #endregion
    #region Potato King
    PK_ComingUp,
    PK_DamageTaken,
    PK_DamageTaken2,
    PK_DamageTaken3, 
    PK_Death, 
    PK_GoingDown, 
    PK_Spit,
    #endregion
    #region CarrotLieutenants
    Carrot_L_Attack1,
    Carrot_L_Attack2,
    Carrot_L_Attack3,
    Carrot_L_Attack4,
    Carrot_L_Damage1,
    Carrot_L_Damage2,
    Carrot_L_Damage3,
    Carrot_L_Death,
    #endregion
    #region Carrot Khan
    CK_Swing,
    CK_Slam,
    CK_Spin,
    CK_Punch,
    CK_SunBeam,
    //Qoutes
    CK_IntroQuote,
    CK_Phase1Quote,
    CK_Phase1Quote2,
    CK_Phase2Quote, 
    CK_Phase2Quote2,
    CK_TransformationQuote, 
    CK_DeathQoute,
    #endregion
    #endregion
    #endregion

    #region Music Tracks
    MainMenu,
    VineyardTheme,
    PotatoLands,
    KarrotLands,
    DungeonOne,
    PrisonTheme,
    DungeonTheme_2,
    RotBoss,
    PotatoKingMusic,
    CastleTheme,
    CK_PhaseOne_Music,
    CK_PhaseTwo_Music,
    #endregion

    #region NPCs
    NpcDialogue1,
    NpcDialouge2,
    NpcDialouge3,
    NpcDialouge4,
    NpcDialouge5,
    NpcDialouge6,
    PotatoMurmur1,
    Murmur,
    #region Kiko
    KikoBeginning,
    Kiko_End,
    Kiko_Go,
    KikoGrapeJuice,
    Kiko_HelpPotato,
    Kiko_Horrible,
    Kiko_Potato,
    Kiko_PotEnd,
    KikoSword,
    KikoWater,
    #endregion
    #endregion
}
//Audio files under the same audioSource cannot play at the same time. They can only be handled one at a time when using 
//Audio Controller