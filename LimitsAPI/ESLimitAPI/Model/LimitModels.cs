using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ESLimitAPI.Model
{
    public class Limit
    {
        [Key]
        [Column("id")] public int? Id { get; set; }
        [Column("limit_type")] public int? LimitType { get; set; }
        [Column("active")] public int? Active { get; set; }
        [Column("user_id_post")] public int? UserIdPost { get; set; }
        [Column("reserved_int")] public int? ReservedInt { get; set; }
        [Column("limit")] public decimal? AmountLimit { get; set; }
        [Column("name")] public string Name { get; set; } = null;
        [Column("reserved_string")] public string ReservedString { get; set; }
        [Column("column_check")] public string ColumnCheck { get; set; }
        [Column("date_expired")] public DateTime? DateExpired { get; set; }
        [Column("date_post")] public DateTime? DatePost { get; set; }
        [Column("reserved_date")] public DateTime? ReservedDate { get; set; }
    }

    public class LimitType
    {
        [Key]
        [Column("Id")] public int Id { get; set; }
        [Column("Name")] public string Name { get; set; }
    }

    public class CheckLimit
    {
        [Key]
        [Column("id")] public int Id { get; set; }
        [Column("name")] public string Name { get; set; }
        [Column("limit")] public decimal Limit { get; set; }
        [Column("amount_total")] public decimal AmountTotal { get; set; }
        [Column("result")] public string Result { get; set; }
    }

    public class Source
    {
        [Key]
        [Column("Id")] public string Id { get; set; }
        [Column("Name")] public string Name { get; set; }
    }

    public class Transaction
    {
        [Key]
        [Column("Id")] public int Id { get; set; }
        [Column("ExternalId")] public int? ExternalId { get; set; }
        [Column("Source")] public string Source { get; set; }
        [Column("Amount")] public decimal? Amount { get; set; }
        [Column("Okpoa")] public string Okpoa { get; set; }
        [Column("DocumentaType")] public int? DocumentaType { get; set; }
        [Column("Documenta")] public string Documenta { get; set; }
        [Column("Okpob")] public string Okpob { get; set; }
        [Column("DocumentbType")] public int? DocumentbType { get; set; }
        [Column("Documentb")] public string Documentb { get; set; }
        [Column("Phone")] public string Phone { get; set; }
        [Column("Account")] public string Account { get; set; }
        [Column("DatePost")] public DateTime? DatePost { get; set; }
        [Column("WorkDate")] public DateTime? WorkDate { get; set; }
    }

    public class Document
    {
        [Key]
        [Column("Id")] public int Id { get; set; }
        [Column("Name")] public string Name { get; set; }
        [Column("Psptyp")] public string Psptyp { get; set; }
        [Column("Nrf")] public string Nrf { get; set; }
        [Column("Rezid")] public int? Rezid { get; set; }
    }
}