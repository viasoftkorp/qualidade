using System;
using Viasoft.PushNotifications.Abstractions.Contracts;

namespace Viasoft.Qualidade.RNC.Core.Domain.MovimentacaoEstoques.UpdateNotifications;

public class MovimentacaoEstoqueProcessadaUpdateNotification : NotificationUpdate
{
    public override string UniqueTypeName => "MovimentacaoEstoqueProcessada";
    public Guid IdNaoConformidade { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; }

}