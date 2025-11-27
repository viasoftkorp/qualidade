import { ControleTratamentoTermicoI18N } from "../interfaces/controle-tratamento-termico-i18n.interface";

export const CONTROLE_TRATAMENTO_TERMICO_I18N_PT: ControleTratamentoTermicoI18N = {
  TratamentoTermico: {
    Title: 'Tratamentos Térmicos',
    Add: 'Adicionar',
    ImprimirRelatorio: 'Imprimir Relatório',
    Remove: {
      Title: 'Remover',
      UnknownError: 'Erro desconhecido ao remover o item.',
      Confirm: 'Deseja realmente remover o item?'
    },
    Errors: {
      LoteRequired: 'Lote é obrigatório.',
      haInvalid: 'HA inválido, valor deve conter um número inteiro válido.',
      hpInvalid: 'HP inválido, valor deve ser um número inteiro válido.',
      totalInvalid: 'Total inválido, valor deve ser um número inteiro válido.',
      tMinInvalid: 'TMin inválido, valor deve ser um número inteiro válido.',
      tMaxInvalid: 'TMax inválido, valor deve ser um número inteiro válido.',
      tafInvalid: 'TAF inválido, valor deve ser um número inteiro válido.',
      pesoBrutoTotalInvalid: 'Peso Bruto Total inválido, valor deve ser um número inteiro válido.',
      pesoLiquidoTotalInvalid: 'Peso Líquido Total inválido, valor deve ser um número inteiro válido.',
      velocidadeAquecimentoInvalid: 'Velocidade de Aquecimento inválida, valor deve ser um número inteiro válido.',
      enchimentoTemperaturaInvalid: 'Enchimento de Temperatura inválido, valor deve ser um número inteiro válido.',
      patamarInvalid: 'Patamar inválido, valor deve ser um número inteiro válido.',
      temperaturaPatamarInvalid: 'Temperatura do Patamar inválida, valor deve ser um número inteiro válido.',
      velocidadeResfriamentoInvalid: 'Velocidade de Resfriamento inválida, valor deve ser um número inteiro válido.',
      UnknownError: 'Desculpe, ocorreu um erro desconhecido ao salvar o Tratamento Térmico.'
    },
    Column: {
      Lote: 'Lote',
      DataEmissao: 'Data de Emissão',
      TipoTratamento: 'Tipo Tratamento',
      HA: 'HA',
      HP: 'HP',
      HTotal: 'H Total',
      Total: 'Total',
      TMin: 'TMin',
      TMax: 'TMax',
      Grafico: 'Gráfico',
      Ventilar: 'Ventilar',
      TAF: 'TAF (Temperatura de abertura do forno)',
      PesoBrutoTotal: 'Peso Bruto',
      PesoLiquidoTotal: 'Peso líquido',
      Parametro: 'Parâmetro',
      Calco: 'Calço',
      VelocidadeAquecimento: 'Veloc. Aquec.',
      EnchimentoTemperatura: 'Ench. Temp.',
      Patamar: 'Patamar',
      TemperaturaPatamar: 'Temp. Patamar',
      VelocidadeResfriamento: 'Veloc. Resfr.',
      DataInicio: 'Data Início',
      DataChegada: 'Data Chegada',
      DataDesligamento: 'Data Deslig.',
      DataAbertura: 'Data Abertura',
      DataFechamento: 'Data Fechamento',
    },
    Itens: {
      Column: {
        ODF: 'ODF',
        Operacao: 'OPE',
        Peca: 'Peça',
        Descricao: 'Descrição',
        NI: 'NI',
        Qtde: 'Qtde',
        PesoBruto: 'Peso Bruto',
        PesoLiquido: 'Peso Líquido',
        Cliente: 'Cliente',
        TipoTratamento: 'Tipo Tratamento',
        NumeroTermopares: 'Numero dos Termopares'
      }
    }
  }
};
