export interface InspecaoEntradaDTO {
  recnoInspecao: number;
  codigoInspecao: number;
  notaFiscal: number;
  dataInspecao: Date;
  inspetor: string;
  resultado: string;
  quantidadeInspecao: number;
  quantidadeLote: number;
  quantidadeAceita: number;
  quantidadeAprovada: number;
  quantidadeReprovada: number;
}

export interface GetInspecaoEntradaDTO {
  items: InspecaoEntradaDTO[];
  totalCount: number;
}
