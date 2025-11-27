import { IKeyTranslate } from '@viasoft/common';

export interface DefeitosNaoConformidadeI18n extends IKeyTranslate {
  NaoConformidade: {
    Defeitos:{
      Codigo: string,
      Novo: string,
      Descricao: string,
      Salvar: string,
      Quantidade: string,
      Detalhamento: string
    }
    DefeitosModal: {
        Defeitos: string,
        Salvar: string,
        Detalhamento : string,
        Quantidade: string,
        Title: string
    }
  }
}
