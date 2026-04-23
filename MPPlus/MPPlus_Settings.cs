using Quintessential;
using Quintessential.Settings;

namespace MagicalParcherPlus;
public class Settei
{
    public static Settei Instance => MagicalParcherPlus.self.Settings as Settei;

    [SettingsLabel("Gerioification Version 2")]
    public bool GerioificationVanilla = true;

    public static bool DoesModExist(string modName) => Quintessential.QuintessentialLoader.Mods.Any(mod => mod.Name == modName);
}