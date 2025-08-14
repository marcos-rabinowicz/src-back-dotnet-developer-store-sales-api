using System;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public record SaleCreated(Guid SaleId, string SaleNumber, DateTime AtUtc);
