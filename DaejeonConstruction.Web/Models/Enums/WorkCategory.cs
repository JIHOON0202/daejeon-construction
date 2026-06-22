namespace DaejeonConstruction.Web.Models.Enums
{
    /// <summary>
    /// 시공품목 분류 (어닝 / 데크)
    /// </summary>
    public enum WorkCategory
    {
        Awning = 0, // 어닝
        Deck = 1    // 데크
    }

    public static class WorkCategoryExtensions
    {
        public static string ToKorean(this WorkCategory category) => category switch
        {
            WorkCategory.Awning => "어닝",
            WorkCategory.Deck => "데크",
            _ => "기타"
        };
    }
}
