import { TratamentoTermicoPecaInserirInput } from "..";

export interface TratamentoTermicoInserirInput {
    lote: string;
    dataEmissao: Date;
    codigoTratamentoTermicoTipo?: number;
    ha?: number;
    hp?: number;
    total?: number;
    tMin?: number;
    tMax?: number;
    grafico: boolean;
    ventilar: boolean;
    taf?: number;
    codigoParametro?: number;
    codigoCalco?: number;
    velocidadeAquecimento?: string;
    enchimentoTemperatura?: string;
    patamar?: number;
    temperaturaPatamar?: number;
    velocidadeResfriamento?: string;
    dataInicio?: Date;
    dataChegada?: Date;
    dataDesligamento?: Date;
    dataAbertura?: Date;
    pecas: TratamentoTermicoPecaInserirInput[];
}
