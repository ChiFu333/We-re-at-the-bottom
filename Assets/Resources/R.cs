using UnityEngine;

public static class R
{
    public static bool isInited = false;
    public static class Audio
    {
        public static AudioClip panelIn;

        public static AudioClip mouseIn;
        public static AudioClip mouseClicked;
        
        public static AudioClip liftSound;
        public static AudioClip hitDoor;
        public static AudioClip showSound;
        public static AudioClip jumpSound;
        public static AudioClip doorTouched;
        public static AudioClip fallOut;
        public static AudioClip openDoor;
        public static AudioClip closedLoud;
        public static AudioClip wrong;
    }

    public static AudioClip musicCreator;
    public static AudioClip mainMenu;
    
    public static VoiceSO tinyVoice;
    public static VoiceSO creatorVoice;

    public static class Sprites
    {
        public static Sprite closedEyes;
        public static Sprite openEyes;
        public static Sprite empty;
    }
    public static void Init()
    {
        isInited = true;
        
        R.Audio.panelIn = Resources.Load<AudioClip>("Audio/" + "PanelIn");

        R.Audio.mouseIn = Resources.Load<AudioClip>("Audio/" + "MouseInButton");
        R.Audio.mouseClicked = Resources.Load<AudioClip>("Audio/" + "MouseClicked");
        
        Audio.liftSound = Resources.Load<AudioClip>("Audio/" + "LiftSound");
        Audio.hitDoor = Resources.Load<AudioClip>("Audio/" + "HitDoor");
        Audio.showSound = Resources.Load<AudioClip>("Audio/" + "ShowSound");
        Audio.jumpSound = Resources.Load<AudioClip>("Audio/" + "Jump");
        Audio.doorTouched = Resources.Load<AudioClip>("Audio/" + "DoorTouched");
        Audio.fallOut = Resources.Load<AudioClip>("Audio/" + "FallOut");
        Audio.openDoor = Resources.Load<AudioClip>("Audio/" + "OpenDoor");
        Audio.closedLoud = Resources.Load<AudioClip>("Audio/" + "CloseLoud");
        
        tinyVoice = Resources.Load<VoiceSO>("Audio/" + "TinyVoice");
        creatorVoice = Resources.Load<VoiceSO>("Audio/" + "CreatorVoice");
        
        Sprites.closedEyes =  Resources.Load<Sprite>("Sprites/" + "ClosedEyes");
        Sprites.openEyes =  Resources.Load<Sprite>("Sprites/" + "OpenEyes");
        Sprites.empty =  Resources.Load<Sprite>("Sprites/" + "Empty");
        
        musicCreator = Resources.Load<AudioClip>("Audio/" + "Music/" + "WithCreator");
        mainMenu = Resources.Load<AudioClip>("Audio/" + "Music/" + "MainMenuMusic");
        Audio.wrong = Resources.Load<AudioClip>("Audio/" + "Wrong");
    }
}
