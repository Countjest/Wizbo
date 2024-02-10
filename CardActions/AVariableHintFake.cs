using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CountJest.Wizbo;

[JsonConverter(typeof(StringEnumConverter))]
public class AVariableHintFake : AVariableHint
{
    [JsonIgnore]
    public Spr icon = ModEntry.Instance.HStat.Sprite;
    public int displayAmount;
    public string? iconName;
    public AVariableHintFake() : base()
    {
        hand = true;
    }
    public override List<Tooltip> GetTooltips(State s)
    {
        Spr iconTT = ModEntry.Instance.HStat.Sprite;
        switch (iconName)
        {
            case ("Highest Status"):
            iconTT = ModEntry.Instance.HStat.Sprite;
                break;
            case ("Sum Highest Status"):
            iconTT = ModEntry.Instance.SumHStat.Sprite;
                break;
            case ("Exhausted Cards"):
            iconTT = ModEntry.Instance.ExhstCards.Sprite;
                break;
            case ("Enemy Heat"):
            iconTT = ModEntry.Instance.EHeat.Sprite;
                break;
        }

        var resultTT = new List<Tooltip>()
        {
            new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => iconTT,
                () => ModEntry.Instance.Localizations.Localize(["Action", $"{iconName}", "name"]),
                () => ModEntry.Instance.Localizations.Localize(["Action", $"{iconName}", "description"]),
                key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::Icon{iconName}"
            )
        };
        return resultTT;
    }
}