import { IKeyTranslate } from '@viasoft/common';

export interface DefeitoI18n extends IKeyTranslate {
  Configuracoes: {
    Defeito: {
      Novo: string,
      Descricao: string,
      Codigo: string,
      Detalhamento: string,
      Causa:string,
      Solucao:string,
      Ativo: string
    },
    DefeitoEditor: {
      Codigo: string,
      Descricao: string,
      Detalhamento: string,
      Causa: string,
      Solucao: string,
      Salvar: string,
      Title: string,
    }
  }
}
