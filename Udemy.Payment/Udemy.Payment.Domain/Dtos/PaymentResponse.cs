﻿using Udemy.Payment.Domain.Entities;
using Udemy.Payment.Domain.Enums;

namespace Udemy.Payment.Domain.Dtos;

public class PaymentResponse
{
    public string PaymentId { get; set; }
    public PaymentStatus Status { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime TransactionDate { get; set; }
    public string BasketId { get; set; }
    public CardEntity Card { get; set; }
}