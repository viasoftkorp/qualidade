import { IKeyTranslate } from '@viasoft/common';

export interface AcoesPreventivasNaoConformidadeI18n extends IKeyTranslate {
  NaoConformidade: {
    AcoesPreventivas:{
      Codigo: string,
      Novo: string,
      Descricao: string,
      Detalhamento: string
    }
    AcoesPreventivasModal: {
        AcoesPreventivas: string,
        Responsavel:string,
        Auditor:string,
        DataPrevista:string,
        DataVerificacao:string,
        NovaData:string,
        DataAnalise:string,
        Implementada:string,
        Salvar: string,
        Detalhamento : string,
        Title: string
    }
  }
}
