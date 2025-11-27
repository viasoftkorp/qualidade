import { VsColoredTextData } from '@viasoft/components';
import { StatusProducaoRetrabalho } from './status-producao-retrabalho';

export class OperacaoViewOutput {
  public id: string;
  public numeroOperacao: string;
  public idRecurso: string;
  public descricaoRecurso: string;
  public idOperacaoRetrabalhoNaoConformidade: string;
  public status: StatusProducaoRetrabalho;
  public coloredStatus: VsColoredTextData;
}
