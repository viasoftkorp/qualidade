export type PreCadastroType = 'calco' | 'parametro' | 'tipoTratamento';
export interface PreCadastroMapValue {
    suffixApiUrl: string;
    policies: {
        criarEditar: string;
        excluir: string;
    }
}
export const preCadastrosMap: Record<PreCadastroType, PreCadastroMapValue> = {
    calco: {
        suffixApiUrl: 'calcos',
        policies: {
            criarEditar: 'preCadastrosCalcosCriarEditarCalco',
            excluir: 'preCadastrosCalcosRemoverCalco'
        }
    },
    parametro: {
        suffixApiUrl: 'parametros',
        policies: {
            criarEditar: 'preCadastrosParametrosCriarEditarParametro',
            excluir: 'preCadastrosParametrosRemoverParametro'
        }
    },
    tipoTratamento: {
        suffixApiUrl: 'tipos',
        policies: {
            criarEditar: 'preCadastrosTiposTratamentoCriarEditarTipoTratamento',
            excluir: 'preCadastrosTiposTratamentoRemoverTipoTratamento'
        }
    },
};