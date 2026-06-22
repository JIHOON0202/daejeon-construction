namespace DaejeonConstruction.Web.Models.Enums
{
    /// <summary>
    /// 견적문의 처리 상태
    /// </summary>
    public enum EstimateStatus
    {
        Received = 0,   // 접수
        InProgress = 1, // 상담중
        Completed = 2   // 완료
    }

    public static class EstimateStatusExtensions
    {
        public static string ToKorean(this EstimateStatus status) => status switch
        {
            EstimateStatus.Received => "접수",
            EstimateStatus.InProgress => "상담중",
            EstimateStatus.Completed => "완료",
            _ => "알수없음"
        };

        public static string ToBadgeClass(this EstimateStatus status) => status switch
        {
            EstimateStatus.Received => "badge bg-secondary",
            EstimateStatus.InProgress => "badge bg-warning text-dark",
            EstimateStatus.Completed => "badge bg-success",
            _ => "badge bg-light text-dark"
        };
    }
}
