import { IKeyTranslate } from '@viasoft/common';

export interface SolucaoI18n extends IKeyTranslate {
  Configuracoes: {
    Solucao: {
      Codigo: string,
      Novo: string,
      Descricao: string,
      Detalhamento: string,
      Imediata: string,
      Ativo: string
    },
  }
}
