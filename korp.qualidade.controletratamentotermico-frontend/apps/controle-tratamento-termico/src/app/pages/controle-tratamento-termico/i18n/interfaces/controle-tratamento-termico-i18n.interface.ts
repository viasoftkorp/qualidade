import { IKeyTranslate } from '@viasoft/common';

export interface ControleTratamentoTermicoI18N extends IKeyTranslate {
    TratamentoTermico: {
        Title: string;
        Add: string;
        ImprimirRelatorio: string;
        Remove: {
            Title: string;
            UnknownError: string;
            Confirm: string;
        };
        Errors: {
            LoteRequired: string;
            haInvalid: string;
            hpInvalid: string;
            totalInvalid: string;
            tMinInvalid: string;
            tMaxInvalid: string;
            tafInvalid: string;
            pesoBrutoTotalInvalid: string;
            pesoLiquidoTotalInvalid: string;
            velocidadeAquecimentoInvalid: string;
            enchimentoTemperaturaInvalid: string;
            patamarInvalid: string;
            temperaturaPatamarInvalid: string;
            velocidadeResfriamentoInvalid: string;
            UnknownError: string;
        };
        Column: {
            Lote: string;
            DataEmissao: string;
            TipoTratamento: string;
            HA: string;
            HP: string;
            HTotal: string;
            Total: string;
            TMin: string;
            TMax: string;
            Grafico: string;
            Ventilar: string;
            TAF: string;
            PesoBrutoTotal: string;
            PesoLiquidoTotal: string;
            Parametro: string;
            Calco: string;
            VelocidadeAquecimento: string;
            EnchimentoTemperatura: string;
            Patamar: string;
            TemperaturaPatamar: string;
            VelocidadeResfriamento: string;
            DataInicio: string;
            DataChegada: string;
            DataDesligamento: string;
            DataAbertura: string;
            DataFechamento: string;
        };
        Itens: {
            Column: {
                ODF: string;
                Operacao: string;
                Peca: string;
                Descricao: string;
                NI: string;
                Qtde: string;
                PesoBruto: string;
                PesoLiquido: string;
                Cliente: string;
                TipoTratamento: string;
                NumeroTermopares: string;
            };
        };
    };
}