import { IKeyTranslate } from '@viasoft/common';

export interface ControleTratamentoTermicoI18n extends IKeyTranslate {
  ControleTratamentoTermico: {
    Navigation: {
      Title: string;
      Configuracoes: string;
      PreCadastros: {
        Title: string;
        Calco: string;
        Parametro: string;
        TipoTratamento: string;
      };
    };
    ErroDesconhecido: string;
    ErroCurrentTenant: string;
    ErroCurrentEnvironment: string;
    ErroUserLogin: string;
    ErroDatabaseName: string;
    NenhumaPermissaoAplicativo: string;
    Permissions: {
      Configuracoes: string;
      Administrator: string;
      PolicyManager: string;
      PreCadastros: string;
      TratamentoTermico: string;
      TratamentoTermicoCriarEditarTratamento: string;
      TratamentoTermicoRemoverTratamento: string;
      PreCadastrosCalcos: string;
      PreCadastrosCalcosVisualizar: string;
      PreCadastrosCalcosCriarEditarCalco: string;
      PreCadastrosCalcosRemoverCalco: string;
      PreCadastrosParametros: string;
      PreCadastrosParametrosVisualizar: string;
      PreCadastrosParametrosCriarEditarParametro: string;
      PreCadastrosParametrosRemoverParametro: string;
      PreCadastrosTiposTratamento: string;
      PreCadastrosTiposTratamentoVisualizar: string;
      PreCadastrosTiposTratamentoCriarEditarTipoTratamento: string;
      PreCadastrosTiposTratamentoRemoverTipoTratamento: string;
    };
  };
}
