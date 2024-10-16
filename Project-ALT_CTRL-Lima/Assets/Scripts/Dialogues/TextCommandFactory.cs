public class TextCommandsFactory
{
    public TextCommand CreateCommand(string commandName)
    {
        switch (commandName)
        {
            case "camerashake": return new CameraShake();
            case "textjump": return new textJump();
        }

        return null;
    }
}
