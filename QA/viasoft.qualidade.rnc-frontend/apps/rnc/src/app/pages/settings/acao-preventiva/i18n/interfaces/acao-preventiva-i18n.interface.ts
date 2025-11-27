import { IKeyTranslate } from '@viasoft/common';

export interface AcaoPreventivaI18n extends IKeyTranslate {
  Configuracoes: {
    AcaoPreventiva: {
      Novo: string,
      Descricao: string,
      Codigo: string,
      Detalhamento: string,
      NomeResponsavel:string,
      Ativo: string
    },
    AcaoPreventivaEditor: {
      Codigo: string,
      Descricao: string,
      Salvar: string,
      Title: string,
      Detalhamento: string,
      nomeResponsavel:string
    }
  }
}
