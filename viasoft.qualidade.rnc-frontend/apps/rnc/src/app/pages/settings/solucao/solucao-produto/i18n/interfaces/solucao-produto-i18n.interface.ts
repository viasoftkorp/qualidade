import { IKeyTranslate } from '@viasoft/common';

export interface SolucaoProdutoI18n extends IKeyTranslate {
  Solucao: {
    Produto: {
      Codigo:string,
      Descricao:string,
      Produto: string,
      Unidade: string,
      Quantidade: string,
    },
    ProdutoEditor: {
      Produtos: string,
      Quantidade: string,
      Title: string,
      Salvar: string,
      OpEngenharia: string
    }
  }
}
