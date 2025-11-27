import { ServicosNaoConformidadeI18n } from "../interfaces/servicos-nao-conformidades-i18n.interface";

export const SERVICOS_NAO_CONFORMIDADE_I18N_PT: ServicosNaoConformidadeI18n = {
  NaoConformidade: {
    ServicosSolucoes: {
      Codigo: 'Cód. Serviço',
      Descricao: 'Desc. Serviço',
      Novo: 'Novo',
      Salvar: 'Salvar',
      Quantidade: 'Qtd',
      DescricaoRecurso: 'Recurso',
      Detalhamento: 'Detalhamento'
    },
    ServicosSolucoesModal: {
      Servicos: 'Serviço',
      Horas: 'Horas',
      Minutos: 'Minutos',
      OperacaoEngenharia: 'Operação Engenharia',
      Recursos: 'Recurso',
      Detalhamento: 'Detalhamento',
      Quantidade: 'Qtd',
      Salvar: 'Salvar',
      Title: 'Adição/Edição Serviços',
      ServicoValidationResult: {
        OperacaoEngenhariaJaUtilizada: "Operação engenharia já utilizada por outro serviço",
        TempoInvalido: 'O tempo previsto informado é inválido'
      }
    }
  }
};
