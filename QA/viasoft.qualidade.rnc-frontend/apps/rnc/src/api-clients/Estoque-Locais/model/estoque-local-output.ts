export class EstoqueLocalOutput {
  public id: string;
  public legacyId: number;
  public codigoProduto: string;
  public idProduto: string;
  public lote: string;
  public legacyIdEmpresa: number;
  public idEmpresa: string;
  public quantidade: number;
  public codigoLocal: number;
  public idLocal: string;
  public numeroAlocacao?: number;
  public dataFabricacao?: Date;
  public dataValidade?: Date;
  public codigoArmazem: string;
}
