import { IKeyTranslate } from '@viasoft/common';

export interface CausasNaoConformidadeI18n extends IKeyTranslate {
  NaoConformidade: {
    Causas:{
      Codigo: string,
      Novo: string,
      Descricao: string,
      Detalhamento: string
    }
    CausasModal: {
        Causas: string,
        Salvar: string,
        Detalhamento : string,
        CentroCusto : string,
        Title: string
    }
  }
}
