import { StatusProducaoRetrabalho } from '../../Operacao-Retrabalho/model/status-producao-retrabalho';

export class OrdemRetrabalhoOutput {
  public idNaoConformidade: string;
  public numeroOdfRetrabalho: number;
  public quantidade: number;
  public idLocalOrigem: string;
  public idEstoqueLocalDestino: string | null;
  public idLocalDestino: string;
  public codigoArmazem: string;
  public dataFabricacao: Date | null;
  public dataValidade: Date | null;
  public message: string;
  public success: boolean;
  public status: StatusProducaoRetrabalho
}
