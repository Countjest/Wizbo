
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace CountJest.Wizbo
{
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
        public override Icon? GetIcon(State s)
        {
            if (iconName == "Highest Status")
                icon = ModEntry.Instance.HStat.Sprite;
            else if (iconName == "HEStat")
                icon = ModEntry.Instance.HEStat.Sprite;
            else if (iconName == "Exhausted Cards")
                icon = ModEntry.Instance.ExhstCards.Sprite;
            else if (iconName == "EHeat")
                icon = ModEntry.Instance.EHeat.Sprite;
            return new Icon(icon, null, Colors.textMain);

        }
        public override List<Tooltip> GetTooltips(State s) =>
            [new TTText(ModEntry.Instance.Localizations.Localize(["action", "name", "description"], new {Amount = displayAmount.ToString() }))];
    }
}