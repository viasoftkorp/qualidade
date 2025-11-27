export interface InspecaoSaidaDTO {
  codigoInspecao: number;
  odf: number;
  odfApontada: number;
  dataInspecao: Date;
  inspetor: string;
  resultado: string;
  quantidadeInspecao: number;
  quantidadeLote: number;
  quantidadeAceita: number;
  quantidadeRetrabalhada: number;
  quantidadeAprovada: number;
  quantidadeReprovada: number;
  lote: string;
  recnoInspecaoSaida: number;
  codigoProduto: string;
  quantidadeOrdem: number;
  numeroPedido: string;
}

export interface GetInspecaoSaidaDTO {
  items: InspecaoSaidaDTO[];
  totalCount: number;
}
