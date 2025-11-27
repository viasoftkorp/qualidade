import { IKeyTranslate } from '@viasoft/common';

export interface MaquinaMateriaisTreeTableFormI18 extends IKeyTranslate {
  MaquinasMateriaisTreeTableForm: {
    AdicionarMaquina:string,
    AdicionarMaterial:string,
    Deletar: string,
    Descricao: string,
    Quantidade:string,
    Horas:string,
    Minutos:string,
    Detalhamento:string,
    DeletarConfirmMessage: string,
    MaquinasEditorModal: {
      Title: string,
      Recurso: string,
      HorasPrevistas: string,
      MinutosPrevistos: string,
      Detalhamento: string,
      Salvar: string
    },
    MateriaisEditorModal: {
      Title: string,
      Produto: string,
      Quantidade: string,
      Detalhamento: string,
      Salvar: string
    }
  }
}
