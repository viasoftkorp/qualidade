import { IKeyTranslate } from '@viasoft/common';

export interface OdfRetrabalhoNaoConformidadeI18n extends IKeyTranslate {
  Retrabalho: {
    OrdemRetrabalho: {
      NumeroOdfRetrabalho: string,
      Quantidade: string,
      Status: string
    };
  };
}
