export class ConclusoesNaoConformidadesInput {
  public id:string;
  public idNaoConformidade:string;
  public novaReuniao:boolean;
  public dataReuniao: Date;
  public dataVerificacao: Date;
  public auditor: string;
  public idAuditor: string;
  public evidencia: string;
  public eficaz: boolean;
  public cicloDeTempo: number;
}
