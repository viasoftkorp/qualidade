export interface InspecaoDetailsDTO {
  id: string;
  odf: number;
  odfApontada: number;
  codInspecao: number;
  codProduto: string;
  novaInspecao: boolean;
  quantidadeInspecionar: number;
  quantidadeInspecionada: number;
  quantidadeLote: number;
  plano: string;
  lote: string;
  recnoProcesso: number;
}
