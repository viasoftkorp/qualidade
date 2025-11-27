import { IKeyTranslate } from '@viasoft/common';

export interface ProdutosNaoConformidadeI18n extends IKeyTranslate {
  NaoConformidade: {
    ProdutosSolucoes:{
      Codigo: string,
      Novo: string,
      Descricao: string,
      Salvar: string,
      UnidadeMedida: string,
      Quantidade: string,
      Detalhamento: string
    }
    ProdutosSolucoesModal: {
        Produtos: string,
        Salvar: string,
        Detalhamento : string,
        Quantidade: string,
        Title: string,
        OpEngenharia: string,
    }
  }
}
