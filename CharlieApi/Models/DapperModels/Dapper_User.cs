﻿
namespace CharlieApi.Models.DapperModels;

public partial class DapperUser
{
    public int SeqNo { get; set; }

    public string? AccountNumber { get; set; }

    public string? PassWordStr { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public DateOnly DateOfBirth { get; set; }
}