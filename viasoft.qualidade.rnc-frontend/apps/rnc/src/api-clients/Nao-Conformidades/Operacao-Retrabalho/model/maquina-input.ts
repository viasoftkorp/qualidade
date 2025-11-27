import { MaterialInput } from './material-input';

export class MaquinaInput {
  public id: string;
  public idRecurso: string;
  public descricao: string;
  public horas: number;
  public minutos: number;
  public detalhamento: string;
  public materiais: Array<MaterialInput>;
}
