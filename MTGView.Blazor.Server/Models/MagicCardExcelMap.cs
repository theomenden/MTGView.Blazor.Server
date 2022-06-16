using CsvHelper.Configuration;

namespace MTGView.Blazor.Server.Models;

public sealed class MagicCardExcelMap : ClassMap<MagicCard>
{

    public MagicCardExcelMap()
    {
        Map(card => card.index).Name("index");
        Map(card => card.id).Name("id");
        Map(card => card.artist).Name("artist");
        Map(card => card.asciiName).Name("asciiName");
        Map(card => card.availability).Name("availability");
        Map(card => card.borderColor).Name("borderColor");
        Map(card => card.cardParts).Name("cardParts");
        Map(card => card.colorIdentity).Name("colorIdentity");
        Map(card => card.colorIndicator).Name("colorIndicator");
        Map(card => card.colors).Name("colors");
        Map(card => card.duelDeck).Name("duelDeck");
        Map(card => card.edhrecRank).Name("edhrecRank");
        Map(card => card.faceFlavorName).Name("faceFlavorName");
        Map(card => card.finishes).Name("finishes");
        Map(card => card.flavorText).Name("flavorText");
        Map(card => card.frameEffects).Name("frameEffects");
        Map(card => card.frameVersion).Name("frameVersion");
        Map(card => card.hasAlternativeDeckLimit).Convert(row => ConvertFromStringToBool(row.Row.GetField("hasAlternativeDeckLimit")));
        Map(card => card.hasContentWarning).Convert(row => ConvertFromStringToBool(row.Row.GetField("hasContentWarning")));
        Map(card => card.isAlternative).Convert(row => ConvertFromStringToBool(row.Row.GetField("isAlternative")));
        Map(card => card.isFullArt).Convert(row => ConvertFromStringToBool(row.Row.GetField("isFullArt")));
        Map(card => card.isFunny).Convert(row => ConvertFromStringToBool(row.Row.GetField("isFunny")));
        Map(card => card.isOnlineOnly).Convert(row => ConvertFromStringToBool(row.Row.GetField("isOnlineOnly")));
        Map(card => card.isOversized).Convert(row => ConvertFromStringToBool(row.Row.GetField("isOversized")));
        Map(card => card.isPromo).Convert(row => ConvertFromStringToBool(row.Row.GetField("isPromo")));
        Map(card => card.isRebalanced).Convert(row => ConvertFromStringToBool(row.Row.GetField("isRebalanced")));
        Map(card => card.isReprint).Convert(row => ConvertFromStringToBool(row.Row.GetField("isReprint")));
        Map(card => card.isReserved).Convert(row => ConvertFromStringToBool(row.Row.GetField("isReserved")));
        Map(card => card.isStarter).Convert(row => ConvertFromStringToBool(row.Row.GetField("isStarter")));
        Map(card => card.isStorySpotlight).Convert(row => ConvertFromStringToBool(row.Row.GetField("isStorySpotlight")));
        Map(card => card.isTextless).Convert(row => ConvertFromStringToBool(row.Row.GetField("isTextless")));
        Map(card => card.isTimeshifted).Convert(row => ConvertFromStringToBool(row.Row.GetField("isTimeshifted")));
        Map(card => card.keywords).Name("keywords");
        Map(card => card.layout).Name("frameVersion");
        Map(card => card.loyalty).Name("loyalty");
        Map(card => card.manaCost).Name("manaCost");
        Map(card => card.manaValue).Name("manaValue");
        Map(card => card.mtgjsonV4Id).Name("mtgjsonV4Id");
        Map(card => card.mtgoFoilId).Name("mtgoFoilId");
        Map(card => card.mtgoId).Name("mtgoId");
        Map(card => card.multiverseId).Name("multiverseId");
        Map(card => card.name).Name("name");
        Map(card => card.number).Name("number");
        Map(card => card.originalPrintings).Name("originalPrintings");
        Map(card => card.originalReleaseDate).Name("originalReleaseDate");
        Map(card => card.otherFaceIds).Name("otherFaceIds");
        Map(card => card.power).Name("power");
        Map(card => card.printings).Name("printings");
        Map(card => card.promoTypes).Name("promoTypes");
        Map(card => card.purchaseUrls).Name("purchaseUrls");
        Map(card => card.rarity).Name("rarity");
        Map(card => card.rebalancedPrintings).Name("rebalancedPrintings");
        Map(card => card.scryfallId).Name("scryfallId");
        Map(card => card.scryfallIllustrationId).Name("scryfallIllustrationId");
        Map(card => card.scryfallOracleId).Name("scryfallOracleId");
        Map(card => card.securityStamp).Name("securityStamp");
        Map(card => card.setCode).Name("setCode");
        Map(card => card.side).Name("side");
        Map(card => card.signature).Name("signature");
        Map(card => card.subtypes).Name("subtypes");
        Map(card => card.supertypes).Name("supertypes");
        Map(card => card.text).Name("text");
        Map(card => card.toughness).Name("toughness");
        Map(card => card.type).Name("type");
        Map(card => card.types).Name("types");
        Map(card => card.uuid).Name("uuid");
        Map(card => card.variations).Name("variations");
        Map(card => card.watermark).Name("watermark");
    }

    private static bool ConvertFromStringToBool(string valueToConvert)
    {
        if(Int32.TryParse(valueToConvert, out var result))
        {
            return Convert.ToBoolean(result);
        }

        return false;
    }
}