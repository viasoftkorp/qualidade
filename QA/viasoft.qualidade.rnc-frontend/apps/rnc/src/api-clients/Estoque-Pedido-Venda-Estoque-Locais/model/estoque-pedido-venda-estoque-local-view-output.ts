export class EstoquePedidoVendaEstoqueLocalViewOutput {
  public id: string;
  public legacyId: number;
  public idEstoquePedidoVenda: string;
  public idEstoqueLocal: string;
  public isLocalBloquearMovimentacao: boolean;
  public numeroLote: string;
  public codigoLocal: number;
  public dataFabricacao: Date | null;
  public dataValidade: Date | null;
  public idProduto: string;
  public idPedido: string;
  public quantidade: number;
}
