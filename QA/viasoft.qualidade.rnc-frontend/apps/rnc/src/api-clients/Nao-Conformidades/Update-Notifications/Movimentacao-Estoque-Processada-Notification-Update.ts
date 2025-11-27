import { DataLivelyUpdate } from '@viasoft/common';

export class MovimentacaoEstoqueProcessadaNotificationUpdate implements DataLivelyUpdate {
  public uniqueTypeName = 'MovimentacaoEstoqueProcessada';
  public idNaoConformidade:string;
  public success: boolean;
  public message: string;
}

