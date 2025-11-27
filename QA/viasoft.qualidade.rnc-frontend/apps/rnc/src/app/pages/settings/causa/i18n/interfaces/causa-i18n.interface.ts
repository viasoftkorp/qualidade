import { IKeyTranslate } from '@viasoft/common';

export interface CausaI18n extends IKeyTranslate {
  Configuracoes: {
    Causa: {
      Novo: string,
      Descricao: string,
      Codigo: string,
      Detalhamento: string,
      Ativo: string
    },
    CausaEditor: {
      Codigo: string,
      Descricao: string,
      Salvar: string,
      Title: string,
      Detalhamento: string
    }
  }
}
