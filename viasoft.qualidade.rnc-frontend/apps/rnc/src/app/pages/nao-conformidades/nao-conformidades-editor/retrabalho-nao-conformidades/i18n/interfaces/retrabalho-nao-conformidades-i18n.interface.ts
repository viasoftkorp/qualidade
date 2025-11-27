import { IKeyTranslate } from '@viasoft/common';

export interface RetrabalhoNaoConformidadeI18n extends IKeyTranslate {
  NaoConformidades : {
    NaoConformidadesEditor: {
      Retrabalho: {
        OperacaoRetrabalho: {
          Title: string,
          NumeroOperacaoARetrabalhar:string,
          Quantidade: string,
          OperacaoEngenharia: string,
          DescricaoRecurso: string,
          Operacoes: string,
          Status: string,
          StatusOption: {
            Aberta: string,
            Produzindo: string,
            Encerrada: string,
            Cancelada: string
          }
        }
        OrdemRetrabalho: {
          Title: string,
        }
      }
    }
  }
}
