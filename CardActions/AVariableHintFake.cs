
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
            else if (iconName == "Sum Highest Status")
                icon = ModEntry.Instance.SumHStat.Sprite;
            else if (iconName == "Exhausted Cards")
                icon = ModEntry.Instance.ExhstCards.Sprite;
            else if (iconName == "Enemy Heat")
                icon = ModEntry.Instance.EHeat.Sprite;
            return new Icon(icon, null, Colors.textMain);

        }
        public override List<Tooltip> GetTooltips(State s)
        {
            if (iconName != null && iconName == "Highest Status")
            {
                return [new TTText(ModEntry.Instance.Localizations.Localize(["action", "Highest Status", "description"], new { Amount = displayAmount.ToString() }))];

            }
            else if (iconName != null && iconName == "Sum Highest Status")
            {
                return [new TTText(ModEntry.Instance.Localizations.Localize(["action", "Sum Highest Status", "description"], new { Amount = displayAmount.ToString() }))];
            }
            else if (iconName != null && iconName == "Exhausted Cards")
            {
                return [new TTText(ModEntry.Instance.Localizations.Localize(["action", "Exhausted Cards", "description"], new { Amount = displayAmount.ToString() }))];
            }
            else if (iconName != null && iconName == "Enemy Heat")
            {
                return [new TTText(ModEntry.Instance.Localizations.Localize(["action", "Enemy Heat", "description"], new { Amount = displayAmount.ToString() }))];
            }
            else
            {
                throw new NotImplementedException($"Unkown AVariableHint icon");
            }
        }
    }
}