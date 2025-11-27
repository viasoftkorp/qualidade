import { VsMaskBase } from '@viasoft/components';

export function setMaskDecimalPlace(casasDecimais: number): any {
  return {
    mask: Number,
    scale: casasDecimais,
    signed: false,
    padFractionalZeros: true,
    normalizeZeros: true,
    min: 0,
    max: 999999999999,
    radix: ',',
    thousandsSeparator: '.'
  };
}

export function getMascaraCampoNumerico(casasDecimais: number): VsMaskBase {
  return {
    type: 'custom',
    imask: setMaskDecimalPlace(casasDecimais)
  } as VsMaskBase;
}
