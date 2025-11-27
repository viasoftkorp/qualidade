import { IKeyTranslate } from '@viasoft/common';

export interface NaturezaI18n extends IKeyTranslate {
  Configuracoes: {
    Natureza: {
      Novo: string,
      Descricao: string,
      Codigo: string,
      Ativo: string
    },
    NaturezaEditor: {
      Codigo: string,
      Descricao: string,
      Salvar: string,
      Title: string
    }
  }
}
