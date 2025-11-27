import { IKeyTranslate } from '@viasoft/common';

export interface ServicosNaoConformidadeI18n extends IKeyTranslate {
  NaoConformidade: {
    ServicosSolucoes:{
      Codigo: string,
      Novo: string,
      Descricao: string,
      Salvar: string,
      Quantidade: string,
      DescricaoRecurso: string,
      Detalhamento: string,
      HorasPrevistas:string,
      MinutosPrevistos:string,
      Recurso:string,
      OpEngenharia:string
    }
    ServicosSolucoesModal: {
        Servicos: string,
        HorasPrevistas: string,
        MinutosPrevistos: string,
        OperacaoEngenharia: string,
        Recursos: string,
        Salvar: string,
        Detalhamento : string,
        Quantidade: string,
        Title: string,
        ServicoValidationResult: {
          OperacaoEngenhariaJaUtilizada: string
          TempoInvalido: string
        }
    }
  }
}
