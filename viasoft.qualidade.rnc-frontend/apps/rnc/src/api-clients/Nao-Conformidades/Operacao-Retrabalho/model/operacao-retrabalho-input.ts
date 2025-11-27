import { MaquinaInput } from './maquina-input';

export class OperacaoRetrabalhoInput {
  public numeroOperacaoARetrabalhar: string;
  public quantidade: number;
  public maquinas: Array<MaquinaInput>;
}
