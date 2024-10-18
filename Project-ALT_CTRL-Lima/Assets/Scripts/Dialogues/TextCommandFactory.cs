public class TextCommandsFactory
{
    public TextCommand CreateCommand(string commandName)
    {
        switch (commandName)
        {
            case "camerashake": return new CameraShake();
            case "textjump": return new textJump();
            case "textrumble": return new textRumble();
            case "textspeed": return new textSpeed();
            case "textpause": return new textPause();
            case "playsound": return new playSound();
        }

        return null;
    }
}
