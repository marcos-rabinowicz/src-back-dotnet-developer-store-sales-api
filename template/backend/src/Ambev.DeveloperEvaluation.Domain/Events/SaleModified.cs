using System;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public record SaleModified(Guid SaleId, string SaleNumber, DateTime AtUtc);
