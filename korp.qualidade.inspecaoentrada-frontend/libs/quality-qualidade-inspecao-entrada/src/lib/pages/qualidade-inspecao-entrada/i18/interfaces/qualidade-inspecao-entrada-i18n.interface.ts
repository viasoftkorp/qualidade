import { IKeyTranslate } from '@viasoft/common';

export interface QualidadeInspecaoEntradaI18N extends IKeyTranslate {
  QualidadeInspecaoEntrada: {
    Atencao: string;
    Filtrar: string;
    Filtros: string;
    Limpar: string;
    Lote: string;
    NumeroLote: string;
    NotaFiscal: string;
    Produto: string;
    Quantidade: string;
    QuebrarLote: string;
    InspecaoEntradaGrid: {
      DataInspecao: string;
      Inspetor: string;
      Resultado: string;
      QuantidadeInspecao: string;
      QuantidadeLote: string;
      QuantidadeAceita: string;
      QuantidadeAprovada: string;
      QuantidadeReprovada: string;
      EditarInspecao: string;
      ExcluirInspecao: string;
      ImprimirInspecao: string;
    };
    InspecaoDetails: {
      NovaInspecao: string;
      EditarInspecao: string;
      Finalizar: string;
      CodProduto: string;
      QuantidadeLote: string;
      QuantidadeInspecao: string;
      QuantidadeInspecionada: string;
      QuantidadeInspecionar: string;
      QuantidadeInspecaoValidacao: string;
      Descricao: string;
      Resultado: string;
      MenorValor: string;
      MaiorValor: string;
      MenorValorBase: string;
      MaiorValorBase: string;
      Salvar: string;
      SomaLoteInvalida: string;
      Observacao: string;
      AlterarDadosInspecaoModal: {
        Titulo: string;
        Aprovado: string;
        NaoAplicavel: string;
        NaoConforme: string;
        ParcialmenteAprovado: string;
        Alterar: string;
      };
      AlterarDadosFinalizacaoModal: {
        Titulo: string;
        LocalPrincipal: string;
        LocalReprovado: string;
        NumeroPedido: string;
        Odf: string;
        QuantidadeAprovada: string;
        QuantidadeRejeitada: string;
        QuantidadePedido: string;
        QuantidadeEntrada: string;
        SaldoInspecionar: string;
        QuantidadeInspecao: string;
      };
      FinalizarInspecaoModal: {
        Titulo: string;
        DistribuirQuantidadesCheckbox: string;
        QuantidadeAprovada: string;
        QuantidadeRejeitada: string;
        LocalPrincipal: string;
        LocalReprovado: string;
        NumODF: string;
        NumPedido: string;
        Error: {
          AlocacaoQuantidadeRestante: string;
          AlocacaoQuantidadeSuperiorInspecao: string;
          AlocacaoQuantidadeInferiorInspecao: string;
          AlocacaoQuantidadeEntrada: string;
        }
        GridFinalizacao: {
          Pedido: string;
          Odf: string;
          QuantidadeLote: string;
          QuantidadePedido: string;
          QuantidadeEntrada: string;
          SaldoInspecionar: string;
          QuantidadeAprovada: string;
          QuantidadeReprovada: string;
          LocalAprovado: string;
          LocalReprovado: string;
          AlteraraDadosFinalizacaoGrid: string;
        }
      };
      Error: {
        AlocacaoSuperiorInspecao: string;
        AlocacaoInferiorInspecao: string;
      }
    };
    Titulo: string;
    Pesquisar: string;
    NotaFiscalGrid: {
      CodigoProduto: string;
      DescricaoProduto: string;
      DescricaoForneced: string;
      Lote: string;
      Situacao: string;
      Revisao: string;
      Quantidade: string;
      QuantidadeInspecionada: string;
      QuantidadeInspecionar: string;
      DataEntrada: string;
      Plano: string;
      DescricaoPlano: string;
      Observacao: string;
    };
    Resultados: {
      Aprovado: string;
      NaoAplicavel: string;
      NaoConforme: string;
      ParcialmenteAprovado: string;
    };
  };
  HistoricoInspecao: {
    Metricas: string;
    Plano: string;
    DescricaoPlano: string;
    DescricaoFornecedor: string;
    Transferencias: string;
    Filtrar: string;
    Filtros: string;
    Limpar: string;
    Status: string;
    NumeroExecucoes: string;
    NumeroRetentativas: string;
    Erro: string;
    Resultado: string;
    Quantidade: string;
    QuantidadeTotal: string;
    Produto: string;
    NumeroPedido: string;
    NotaFiscal: string;
    Odf: string;
    LocalOrigem: string;
    LocalDestino: string;
    UsuarioExecucao: string;
    DataExecucao: string;
    CodigoProduto: string;
    DescricaoProduto: string;
    QuantidadeLote: string;
    QuantidadeInspecao: string;
    DataInspecao: string;
    Inspetor: string;
    QuantidadeRetrabalhada: string;
    QuantidadeAprovada: string;
    QuantidadeReprovada: string;
    Lote: string;
    Rnc: string;
    MovimentarInspecaoStatus: {
      Inicio: string;
      EmProcesso: string;
      Falha: string;
      Sucesso: string;
    }
    Estornar: string;
    Transferencia: {
      TipoTransferencia: string;
      TipoTransferencias: {
        Aprovado: string;
        Reprovado: string;
        Retrabalhado: string;
      };
      Quantidade: string;
      DescricaoLocalOrigem: string;
      DescricaoLocalDestino: string;
      NumeroPedido: string;
    }
    Resultados: {
      Aprovado: string;
      NaoAplicavel: string;
      NaoConforme: string;
      ParcialmenteAprovado: string;
    }
  };
  FiltrosInspecao: {
    Filtrar: string;
    Metricas: string;
    Observacao: string;
    Aplicar: string;
    Limpar: string;
  };
}
