export interface FinalizarInspecaoInput {
  codigoInspecao: number;
  quantidadeAprovada: number;
  quantidadeRejeitada: number;
  codigoLocalPrincipal: number;
  codigoLocalReprovado: number;
  idRnc?: string;
}
