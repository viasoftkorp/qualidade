import { OperacaoOutput } from './operacao-output';

export class OperacaoRetrabalhoOutput {
  public id: string;
  public idNaoConformidade: string;
  public quantidade: number;
  public numeroOperacaoARetrabalhar: string;
  public message: string;
  public success: boolean;
  public operacoes: Array<OperacaoOutput>;
}
