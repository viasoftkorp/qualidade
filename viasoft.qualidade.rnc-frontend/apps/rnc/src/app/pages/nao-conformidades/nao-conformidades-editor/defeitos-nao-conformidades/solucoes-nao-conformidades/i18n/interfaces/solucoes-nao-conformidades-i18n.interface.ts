import { IKeyTranslate } from '@viasoft/common';

export interface SolucoesNaoConformidadeI18n extends IKeyTranslate {
  NaoConformidade: {
    Solucoes:{
      Codigo: string,
      Novo: string,
      Descricao: string,
      Produto: string,
      Salvar: string,
      Quantidade: string,
      Detalhamento: string
    }
    SolucaoEditor: {
        Voltar: string,
        Salvar: string,
        Detalhamento : string,
        CustoEstimado: string,
        Responsavel: string,
        Auditor: string,
        DataAnalise: string,
        DataVerificacao: string,
        DataPrevista: string,
        NovaData: string,
        Solucao:string,
        SolucaoImediata:string,
        Codigo:string,
        Title:string
    }
  }
}
