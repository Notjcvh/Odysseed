public enum AudioType
{
    None,

#region Music Tracks
    MainMenuMusic,
    CharacterSelectMusic,
    GameSong1,
    GameSong2,
    GameSong3,
    GameSong4,
    ArtTheme,
    LawTheme,
    VictoryTrack,
    DefeatTrack,
    #endregion

#region UI SFX
    MenuSelect1,
    MenuSelect2, 
    MenuSelect3,
    MenuMove1,
    MenuMove2,
#endregion

    #region Character Sounds
    //Merlot
    Slash,
    Dash,
    #endregion
}
//Audio files under the same audioSource cannot play at the same time. They can only be handled one at a time.