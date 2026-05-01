using Quintessential;
using Quintessential.Settings;

namespace MagicalParcherPlus;
public class Settei
{
    public static Settei Instance => MagicalParcherPlus.self.Settings as Settei;

    [SettingsLabel("Gerioification Version 2")]
    public bool GerioificationVanilla = false;
    [SettingsLabel("Wear Gerio's Hat")]
    public bool GerioHasHat = false;
    [SettingsLabel("Wear Tric's Headphones")]
    public bool TricHasHeadphones = true;
    [SettingsLabel("Bypass Part Rule Checking")]
    public bool BypassPartRules = false;
}