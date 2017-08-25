namespace DotnetPJ
{
	public enum Result
	{
		OK = 0,

		Reserved1 = -1,
		UnknownError = -2,
		NotImplemented = -3,
		ProtocolDeprecated = -4,
		Reserved99 = -99,

		// Authentication
		AuthenticationExpired = -1000,
		AuthenticationFailed = -1001,
		PlayerCreationFailed = -1002,
		PlayerNotExists = -1003,
		UserBlocked = -1004,

		// Character
		CharacterCreationFailed = -1101,
		CharacterCreationFull = -1102,
		CharacterSelectionFailed = -1103,
		CharacterNotExists = -1104,
		InvalidCharacterRank = -1105,
		CharacterClassNotMatched = -1106,
		CharacterNotEnoughRank = -1107,
		InvalidCharacterClass = -1108,
		CharacterAlreadyExists = -1109,
		CharacterOpenFailedToPay = -1110,

		// Equipment
		EquipFailed = -1201,
		UnequipFailed = -1202,
		InvalidEquipmentType = -1203,
		InvalidEquipPage = -1204,
		InvalidPageOpenType = -1205,
		EquipmentPageSelectFail = -1206,
		InsufficientOpenValue = -1207,
		SetLockEquipmentFailed = -1208,
		InvalidEquipment = -1209,
		InsufficientRankToEquip = -1210,

		// Skill
		SkillUnknownError = -1400,
		SkillPageInvalid = -1401,
		SkillInvalidTreeSector = -1402,
		SkillInvalidStep = -1403,
		SkillInsufficientSectorPoint = -1404,
		SkillAlreadyMinLevel = -1405,
		SkillAlreadyMaxLevel = -1406,
		SkillInsufficientPoint = -1407,
		SkillPageSelectFail = -1408,
		SkillInvalidSlot = -1409,
		SkillPageAlreadyOpened = -1410,
		SkillPageInvalidOpenType = -1411,
		SkillTreeIdInvalid = -1412,
		SkillTreeResetFailed = -1413,
		SkillTreeSectorNotMatched = -1414,
		SkillStepNotOrdered = -1415,

		// Raid
		RaidNotExists = -1600,

		// Shop
		NotExistsShopProduct = -1700,
		SoldOutProduct = -1701,
		InvalidItemType = -1702,
		InvalidItemValue = -1703,
		InvalidAmount = -1704,
		InsufficientAmount = -1705,
		AmountWasChanged = -1706,
		NotCurrency = -1707,
		PaymentFailed = -1708,
		SlotNotExists = -1709,
		InvalidShop = -1710,
		InvalidShopSlot = -1711,
		InvalidShopProduct = -1712,
		InvalidShopId = -1713,
		CurrencyLack = -1714,
		RefreshShopFailed = -1715,
		EquipmentSellFailed = -1716,
		MaxCurrency = -1717,
		SellValueError = -1718,
		EarnFailed = -1719,

		// Inventory
		InventorySlotMax = -1900,
		IncrementInventorySlotFailed = -1901,

		// Stuff
		InvalidStuffId = -2000,
		WrongArgumentCount = -2001,

		// Quest
		QuestNotOpened = -2100,
		QuestDataNotExists = -2101,
		InvalidQuestId = -2102,
		InvalidInGameSetLink = -2103,
		UnknownQuestRandomType = -2104,
		InvalidInGameSetGroupIndex = -2105,
		QuestAlreadyDayLimitCount = -2106,
		InvalidRewardSetId = -2107,

		// Recipe
		RecipeFailed = -2200,
		UnknownRecipe = -2201,
		InvalidRecipeMaterial = -2202,
		StuffListFailed = -2203,
		UnknownRecipeType = -2204,
		InvalidRecipeId = -2205,

		// Rune
		InvalidRuneId = -2300,
		InvalidRuneSeq = -2301,
		RuneNotExists = -2302,
		RuneIsEquipped = -2303,
		RuneNotOwned = -2304,
		RuneEquippedOrNotOwned = -2305,
		RuneUnequippedOrNotOwned = -2306,
		RuneLevelMax = -2307,
		RuneSellFailed = -2308,

		// Potion
		InvalidPotionId = -2500,
		PotionEquipped = -2501,

		// Resurrection
		InsufficientResurrectionCoin = -2600,
		AlreadyResurrectionMax = -2601,

		// Theme
		InvalidTheme = -2700,
		InvalidThemeQuestId = -2701,
		ThemeQuestAlreadyCleared = -2702,

		// Npc
		NpcStatusNotExists = -2800,

		// Chapter
		ChapterInvalid = -2900,
		ChapterStarsLack = -2901,
		ChapterAlreadyReward = -2902,

		// Companion
		CompanionUnknownError = -3000,
		CompanionNotExists = -3001,
		CompanionAlreadyExists = -3002,
		CompanionInsufficientStamina = -3003,
		CompanionDispatchingSameQuest = -3004,
		CompanionDispatchStartFailed = -3005,
		CompanionDispatchEndFailed = -3006,
		CompanionDispatchCancelFailed = -3007,
		CompanionEquipFailed = -3008,
		CompanionUnequipFailed = -3009,
		CompanionInsufficientTicket = -3010,
		CompanionFeedFailed = -3011,
		CompanionDispatchMissingStar = -3012,
		CompanionDispatchDataNotExists = -3013,
		CompanionRestSlotError = -3014,
		CompanionFoodInvalid = -3015,
		CompanionLevelExpInvalid = -3016,
		CompanionIdInvalid = -3017,
		CompanionMaxLevel = -3018,
		CompanionLockFailed = -3019,
		CompanionDispatchQuestSortNotMatched = -3020,

		// CompanionFood
		CompanionFoodUnknownError = -3200,
		CompanionFoodInsufficientCount = -3201,

		// Oculus
		OculusAuthFailed = -3300,
		OculusNotOwnedItem = -3301,

		// VIP
		VipPointGetTypeNotRegistered = -3400,

		// Item
		ItemImpossibleToSend = -3500,
		ItemImpossibleToPost = -3501,

		// MailBox
		MailAlreadyReceived = -3600,
		MailExpired = -3601,
		MailRemoveFailed = -3602,
		MailReadFailed = -3603,
		UnknownMailBoxType = -3604,
		MailCheckFailed = -3605,
		MailReceiveFailed = -3606,

		// Steam
		SteamAuthFailed = -3700,
		SteamUnknownResponse = -3701,
		SteamVacBanned = -3702,
		SteamPublisherBanned = -3703,
		SteamTransactionOrderIdNotCreated = -3704,
		SteamTransactionMissingProductId = -3705,
		SteamTransactionWrongOrderId = -3706,
		SteamAuthInvalidTicket101 = -3707,
		SteamAuthTicketForOtherApp102 = -3708,

		// Raid Auth & Session
		RaidRegionNotExists = -3800,
		RaidSessionServerFull = -3801,
		RaidMaintenanceNow = -3802,

		// Sales
		SalesInvalidType = -3900,
		SalesCycleLimit = -3901,
		SalesCountLimit = -3902,

		// PlayStation
		PlayStationAuthFailed = -4000,

		// Raid
		RaidInvalidSuid = -10001,
		RaidInvalidHash = -10002,
		RaidPlayerNotExist = -10003,
		RaidCharacterNotExist = -10004,
		RaidInvalidId = -10005,
		RaidInvalidPreviousId = -10006,
		RaidInsufficientPoint = -10007,
		RaidInsufficientPass = -10008,
		RaidAlreadyOpened = -10009,
	}
}
