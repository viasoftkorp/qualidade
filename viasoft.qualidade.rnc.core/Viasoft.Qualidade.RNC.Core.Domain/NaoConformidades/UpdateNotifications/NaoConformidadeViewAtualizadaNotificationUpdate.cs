using System;
using Viasoft.PushNotifications.Abstractions.Contracts;

namespace Viasoft.Qualidade.RNC.Core.Domain.NaoConformidades.UpdateNotifications;

public class NaoConformidadeViewAtualizadaNotificationUpdate : NotificationUpdate
{
    public override string UniqueTypeName => "NaoConformidadeViewAtualizadaNotificationUpdate";
    public Guid IdNaoConformidade { get; set; }
}