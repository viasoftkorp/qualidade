import { IKeyTranslate } from '@viasoft/common';

export interface PreCadastrosI18N extends IKeyTranslate {
  PreCadastros: {
    ParadasMaquina: {
      Title: string,
      Adicionar: string,
      Codigo: string,
      Descricao: string,
      Editar: string,
      Deletar: string,
      Salvar: string,
      ConfirmaDeletarParada: string,
      Programada: string,
      EmManutencao: string,
      HoraInicio: string,
      HoraFim: string,
      TodasMaquinas: string,
      Maquinas: string,
      SemPermissaoParadaProgramada: string;
      ObsComplementar: string;
      RealizarParadaFimSemana: string;
    },
    DestinacoesOdf: {
      Title: string,
      Adicionar: string,
      Editar: string,
      Deletar: string,
      Salvar: string,
      Descricao: string,
      DescricaoTipoDestinacao: string,
      CodigoDestinatario: string,
      DestinacaoClienteFinal: string,
      DestinacaoDestinatarioDefinido: string,
      DestinacaoTransferenciaInterna: string
    },
    TratamentoTermicoGenerico: {
      Adicionar: string,
      Editar: string,
      Deletar: string,
      Salvar: string,
      Codigo: string,
      Descricao: string,
      ConfirmaDeletar: string,
      DescriptionRequired: string
    }
  };
}
