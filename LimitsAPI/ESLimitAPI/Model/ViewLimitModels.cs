using System;

namespace ESLimitAPI
{
    public class LimitTransaction
    {
        public int ExternalId { get; set; }
        public string Source { get; set; }
        public decimal Amount { get; set; }
        public string OkpoA { get; set; }
        public int? DocumentTypeA { get; set; }
        public string DocumentA { get; set; }
        public string OkpoB { get; set; }
        public int? DocumentTypeB { get; set; }
        public string DocumentB { get; set; }
        public string Phone { get; set; }
        public string Account { get; set; }
    }

    public class LimitView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal AmountLimit { get; set; }
        public int LimitType { get; set; }
        public int Active { get; set; }
        public string ColumnCheck { get; set; }
        public DateTime DateExpired { get; set; }
        public int UserIdPost { get; set; }
    }
}
