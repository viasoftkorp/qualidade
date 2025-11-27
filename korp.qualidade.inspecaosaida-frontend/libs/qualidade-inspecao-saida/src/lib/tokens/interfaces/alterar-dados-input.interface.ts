import { InspecaoSaidaItemDTO } from './get-inspecao-saida-itens-dto.interface';
import { PlanoInspecaoDTO } from './get-planos-inspecao-dto.interface';
import { InspecaoDetailsDTO } from './inspecao-details-dto.class';

export interface AlterarDadosDTO {
  dto: PlanoInspecaoDTO | InspecaoSaidaItemDTO,
  inspecaoDetais: InspecaoDetailsDTO
}
