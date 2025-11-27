import { SolucaoServicoI18n } from '../interfaces/solucao-servico-i18n.interface';

export const SOLUCAO_SERVICO_I18N_PT: SolucaoServicoI18n = {
  Solucao: {
    Servico: {
      Codigo: 'Cód',
      Descricao: 'Desc',
      Produto: 'Produto',
      Quantidade: 'Qtd',
      HorasPrevistas: 'Horas Previstas',
      MinutosPrevistos: 'Minutos Previstos',
      Recurso: 'Recurso',
      OpEngenharia: 'Operação Engenharia',
    },
    ServicoEditor: {
      Servico: 'Serviço',
      Produto: 'Produto',
      Quantidade: 'Qtd',
      HorasPrevistas: 'Horas Previstas',
      MinutosPrevistos: 'Minutos Previstos',
      Recurso: 'Recurso',
      OpEngenharia: 'Operação Engenharia',
      Salvar: 'Salvar',
      Title: 'Adição/Edição de Serviços',
      ServicoValidationResult: {
        OperacaoEngenhariaJaUtilizada: 'Operação engenharia já utilizada por outro serviço desta solução',
        TempoInvalido: 'O tempo previsto informado é inválido'
      }
    }
  }
};
