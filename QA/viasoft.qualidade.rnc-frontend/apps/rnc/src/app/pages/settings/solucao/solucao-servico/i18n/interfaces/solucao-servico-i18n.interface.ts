import { IKeyTranslate } from '@viasoft/common';

export interface SolucaoServicoI18n extends IKeyTranslate {
  Solucao: {
    Servico: {
      Codigo:string,
      Descricao:string,
      Produto: string,
      Quantidade: string,
      HorasPrevistas: string,
      MinutosPrevistos: string,
      Recurso: string,
      OpEngenharia: string,
    },
    ServicoEditor: {
      Servico:string,
      Produto: string,
      Quantidade: string,
      HorasPrevistas: string,
      MinutosPrevistos: string,
      Recurso: string,
      OpEngenharia: string,
      Salvar: string,
      Title: string,
      ServicoValidationResult: {
        OperacaoEngenhariaJaUtilizada: string,
        TempoInvalido: string
      }
    }
  }
}
