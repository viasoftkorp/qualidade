import { IKeyTranslate } from '@viasoft/common';

export interface QualidadeInspecaoSaidaI18N extends IKeyTranslate {
  QualidadeInspecaoSaida: {
    Cliente: string;
    NumeroPedido: string;
    Filtrar: string;
    Filtros: string;
    Limpar: string;
    Odf: string;
    Lote: string;
    Produto: string;
    InspecaoSaidaGrid: {
      DataInspecao: string;
      Inspetor: string;
      Resultado: string;
      QuantidadeInspecao: string;
      QuantidadeLote: string;
      QuantidadeAceita: string;
      QuantidadeRetrabalhada: string;
      QuantidadeAprovada: string;
      QuantidadeReprovada: string;
      EditarInspecao: string;
      ExcluirInspecao: string;
    };
    InspecaoDetails: {
      QuantidadePedidoVenda: string;
      AlterarDados: string;
      NovaInspecao: string;
      Finalizar: string;
      CodProduto: string;
      QuantidadeInspecao: string;
      QuantidadeInspecionada: string;
      QuantidadeInspecionar: string;
      QuantidadeInspecaoValidacao: string;
      QuantidadeLote: string;
      Descricao: string;
      Resultado: string;
      MenorValor: string;
      MaiorValor: string;
      MenorValorBase: string;
      MaiorValorBase: string;
      Salvar: string;
      FinalizarInspecao: string;
      Odf: string;
      PedidoVenda: string;
      Observacao: string;
      AlterarDadosInspecaoModal: {
        Titulo: string;
        Aprovado: string;
        NaoAplicavel: string;
        NaoConforme: string;
        Alterar: string;
      };
      FinalizarInspecaoModal: {
        Titulo: string;
        DistribuirQuantidadesCheckbox: string;
        QuantidadeAprovada: string;
        QuantidadeReprovada: string;
        QuantidadeRetrabalhada: string;
      };
      AlocacaoPedidoVenda: {
        Title: string;
        NumeroPedido: string;
        OrdemFabricacao: string;
        QuantidadeAlocadaPedido: string;
        QuantidadeAprovada: string;
        QuantidadeReprovada: string;
        QuantidadeRetrabalho: string;
        LocalAprovado: string;
        LocalReprovado: string;
        LocalRetrabalho: string;
        Error: {
          AlocacaoNecessidadePedido: string;
          AlocacaoQuantidadeInspecao: string;
        }
      };
      Error: {
        AlocacaoSuperiorLote: string;
        AlocacaoInferiorInspecao: string;
        AlocacaoSuperiorPedido: string;
      }
    };
    Titulo: string;
    Pesquisar: string;
    OrdemProducaoGrid: {
      Plano: string;
      DescricaoPlano: string;
      CodigoProduto: string;
      DescricaoProduto: string;
      Situacao: string;
      Revisao: string;
      QuantidadeOrdem: string;
      QuantidadeProduzida: string;
      QuantidadeInspecionada: string;
      QuantidadeInspecionar: string;
      DataInicio: string;
      DataEntrega: string;
      DataEmissao: string;
      DataNegociada: string;
      NovaInspecao: string;
    };
  };
  HistoricoInspecao: {
    Metricas: string;
    OdfRetrabalho: string;
    CodigoRnc: string;
    Rnc: string;
    Revisao: string;
    Plano: string;
    DescricaoPlano: string;
    Cliente: string;
    Transferencias: string;
    Filtrar: string;
    Filtros: string;
    Limpar: string;
    Status: string;
    Lote: string;
    NumeroExecucoes: string;
    NumeroRetentativas: string;
    Erro: string;
    Resultado: string;
    Quantidade: string;
    QuantidadeTotal: string;
    Produto: string;
    NumeroPedido: string;
    Odf: string;
    LocalOrigem: string;
    LocalDestino: string;
    UsuarioExecucao: string;
    DataExecucao: string;
    CodigoProduto: string;
    DescricaoProduto: string;
    QuantidadeLote: string;
    QuantidadeInspecao: string;
    TipoInspecao: string;
    DataInspecao: string;
    Inspetor: string;
    QuantidadeRetrabalhada: string;
    QuantidadeAprovada: string;
    QuantidadeReprovada: string;
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
  };
}
