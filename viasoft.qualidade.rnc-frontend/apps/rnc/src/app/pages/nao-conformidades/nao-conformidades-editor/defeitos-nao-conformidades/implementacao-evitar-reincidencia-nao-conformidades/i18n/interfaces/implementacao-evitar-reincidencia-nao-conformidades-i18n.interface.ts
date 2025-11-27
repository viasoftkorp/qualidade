import { IKeyTranslate } from '@viasoft/common';

export interface IMPLEMENTACAO_EVITAR_REINCIDENCIA_NAO_CONFORMIDADES_I18N extends IKeyTranslate {
  NaoConformidade: {
    ImplementacaoEvitarReincidencia: {
      Descricao: string,
      Responsavel: string,
      Auditor: string,
      DataAnalise: string
    },
    ImplementacaoEvitarReincidenciaEditorModal: {
      Title: string,
      Responsavel:string,
      Auditor:string,
      DataAnalise:string,
      DataPrevista:string,
      DataVerificacao:string,
      NovaData:string,
      Descricao:string,
      Salvar:string,
      AcaoImplementada:string
    }
  }
}
