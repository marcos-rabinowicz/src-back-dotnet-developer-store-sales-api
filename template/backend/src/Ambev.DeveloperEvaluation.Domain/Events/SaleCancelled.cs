using System;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public record SaleCancelled(Guid SaleId, string SaleNumber, DateTime AtUtc);
