using System;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public record ItemCancelled(Guid SaleId, Guid ItemId, DateTime AtUtc);
