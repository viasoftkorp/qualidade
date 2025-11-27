using Viasoft.PushNotifications.Abstractions.Contracts;

namespace Viasoft.Qualidade.RNC.Core.Domain.OperacaoRetrabalhoNaoConformidades.Operacoes.UpdateNotifications;

public class StatusOperacaoAtualizadoUpdateNotification : NotificationUpdate
{
    public override string UniqueTypeName => "StatusOperacaoAtualizado";
}