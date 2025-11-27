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
      Detalhamento: string
    }
    ServicosSolucoesModal: {
        Servicos: string,
        Horas: string,
        Minutos: string,
        OperacaoEngenharia: string,
        Recursos: string,
        Salvar: string,
        Detalhamento : string,
        Quantidade: string,
        Title: string,
        ServicoValidationResult: {
          OperacaoEngenhariaJaUtilizada:string,
          TempoInvalido: string
        }
    }
  }
}
