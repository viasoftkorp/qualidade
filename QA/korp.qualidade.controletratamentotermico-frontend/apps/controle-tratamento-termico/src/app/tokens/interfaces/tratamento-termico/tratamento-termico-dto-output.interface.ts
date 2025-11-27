export interface TratamentoTermicoDtoOutput {
    id: string;
    lote: string;
    dataEmissao: Date;
    descricaoTratamentoTermicoTipo: string;
    codigoTratamentoTermicoTipo: number;
    ha: number;
    hp: number;
    total: number;
    tMin: number;
    tMax: number;
    grafico: boolean;
    ventilar: boolean;
    taf: number;
    descricaoParametro: string;
    codigoParametro: number;
    descricaoCalco: string;
    codigoCalco: number;
    velocidadeAquecimento: string;
    enchimentoTemperatura: string;
    patamar: number;
    temperaturaPatamar: number;
    velocidadeResfriamento: string;
    dataInicio: Date;
    dataChegada: Date;
    dataDesligamento: Date;
    dataAbertura: Date;
    pesoLiquidoTotal: number;
    pesoBrutoTotal: number;

    // Virtual
    hTotal?: number;
}
