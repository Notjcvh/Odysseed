public enum AudioType
{
    None,
    #region Music Tracks
    MainMenu,
    RotBoss,
    DungeonOne,
    #endregion

    #region UI SFX
#endregion

    #region Character Sounds
    //Merlot
    PlayerAttack,
    PlayerWalk,
    PlayerDeath,

    #endregion

    #region Enemy Sounds
    PopperDeath,
    PopperEnemyExplode,
    RotEnemyNoise,
    RotDeath,
    #region Bosses
    BossIntro,
    EnemySpawnBoss,
    BossDeath,
    #endregion
    #endregion
}
//Audio files under the same audioSource cannot play at the same time. They can only be handled one at a time.